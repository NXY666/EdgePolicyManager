using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy;

public sealed class SettingsPageModel : INotifyPropertyChanged
{
    public List<ComboBoxItem> LanguageList { get; } = new()
    {
        new ComboBoxItem {Content = "English", Tag = "en-US"},
        new ComboBoxItem {Content = "中文（简体）", Tag = "zh-CN"},
        new ComboBoxItem {Content = "中文（台湾）", Tag = "zh-TW"},
    };
    
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed partial class SettingsPage
{
    private SettingsPageModel _dataContext = new();

    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        _dataContext = new SettingsPageModel();
    }

    private void LanguageComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedItem is not ComboBoxItem selectedItem) return;

        ResourceUtil.SetLanguage(selectedItem.Tag.ToString());
    }
}