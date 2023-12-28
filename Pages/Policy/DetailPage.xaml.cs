using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Models.Policy;
using PolicyManager.Pages.Policy.Contents;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy;

public sealed class DetailPageModel : INotifyPropertyChanged
{
    public delegate void SearchPolicyEventHandler(string keyword);

    public event SearchPolicyEventHandler SearchPolicyEvent;

    public string PolicyType { get; init; }

    public PolicyDetailMap PolicyDetailMap { get; set; }

    private readonly PolicyMenuItem _activePolicyMenu;

    public PolicyMenuItem ActivePolicyMenu
    {
        init
        {
            _activePolicyMenu = value;
            OnPropertyChanged(nameof(ActivePolicyMenuIconGlyph));
            OnPropertyChanged(nameof(ActivePolicyMenuName));
        }
        get => _activePolicyMenu;
    }

    public string ActivePolicyMenuIconGlyph => IconUtil.GetGlyphByName(ActivePolicyMenu.Icon);

    public string ActivePolicyMenuName => ActivePolicyMenu?.Name;

    public ObservableCollection<ExpanderListItem> ExpanderListItems { get; } = [];

    public bool IsExpanderListItemsEmpty => ExpanderListItems.Count == 0;

    public SearchPolicyEventHandler SearchPolicyHandler { get; init; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal void OnSearchPolicyEvent(string keyword)
    {
        SearchPolicyEvent?.Invoke(keyword);
    }
}

public class ExpanderListItem
{
    public NotifyPolicyManager PolicyManager { get; init; }
    public PolicyDetail PolicyDetail { get; init; }
    public Page StatusFrame { get; init; }
    public bool IsSupported { get; init; }
}

public sealed partial class DetailPage
{
    private DetailPageModel _dataContext;

    public DetailPage()
    {
        InitializeComponent();
        DataContext = new DetailPageModel();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not DetailPageModel detailPageModel) return;

        _dataContext = detailPageModel;

        detailPageModel.PolicyDetailMap = ResourceUtil.GetEmbeddedJson<PolicyDetailMap>($"StaticModels.Policy.{detailPageModel.PolicyType}.{{LangCode}}.PolicyDetailMap.json");

        // PolicyPage开始监听SearchPolicyEvent事件
        detailPageModel.SearchPolicyEvent += detailPageModel.SearchPolicyHandler;

        // 加载数据
        foreach (var item in detailPageModel.ActivePolicyMenu.Items)
        {
            var policy = detailPageModel.PolicyDetailMap[item];
            var policyManager = new NotifyPolicyManager(new Utils.PolicyManager(policy));

            Page statusFrame = policyManager.PolicyDataOptionsExists
                ? new DropdownContent(policyManager)
                : policy.DataType switch
                {
                    "Boolean" => new BooleanContent(policyManager),
                    "Integer" => new IntegerContent(policyManager),
                    "String" => new StringContent(policyManager),
                    _ => new UnsupportedContent()
                };

            var expanderListItem = new ExpanderListItem { PolicyDetail = policy, PolicyManager = policyManager, StatusFrame = statusFrame, IsSupported = statusFrame is not UnsupportedContent };

            detailPageModel.ExpanderListItems.Add(expanderListItem);
        }
    }

    private void SearchPolicy(string keyword)
    {
        _dataContext.OnSearchPolicyEvent(keyword);
    }

    private async void MarkdownTextBlock_OnLinkClicked(object sender, LinkClickedEventArgs e)
    {
        if (e.Link.StartsWith('#'))
        {
            // 如果link开头是#，则弹出对话框，允许用户复制#后面的内容
            var dialog = new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = XamlRoot,
                Title = ResourceUtil.GetString("DetailPage/MarkdownTextBlock_OnLinkClicked/DialogTitle"),
                PrimaryButtonText = ResourceUtil.GetString("DetailPage/MarkdownTextBlock_OnLinkClicked/DialogSearchButtonText"),
                SecondaryButtonText = ResourceUtil.GetString("DetailPage/MarkdownTextBlock_OnLinkClicked/DialogCopyButtonText"),
                CloseButtonText = ResourceUtil.GetString("DetailPage/MarkdownTextBlock_OnLinkClicked/DialogCancelButtonText"),
                DefaultButton = ContentDialogButton.Primary,
                Content = e.Link
            };

            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.Primary:
                {
                    SearchPolicy(e.Link[1..]);
                    break;
                }
                case ContentDialogResult.Secondary:
                {
                    // 复制
                    var dataPackage = new DataPackage();
                    dataPackage.SetText(e.Link[1..]);
                    Clipboard.SetContent(dataPackage);
                    break;
                }
                case ContentDialogResult.None:
                default:
                {
                    break;
                }
            }
        }
        else
        {
            // 否则，使用默认浏览器打开链接
            await Launcher.LaunchUriAsync(new Uri(new Uri("https://learn.microsoft.com/deployedge/microsoft-edge-policies"), e.Link));
        }
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = (ComboBox)sender;

        var dataContext = (ExpanderListItem)comboBox.DataContext;
        if (dataContext == null) return;

        dataContext.PolicyManager.PolicyLevel = comboBox.SelectedIndex;
    }
}