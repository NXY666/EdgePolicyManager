using System;
using System.Collections.Generic;
using System.Net.Http;
using Windows.System;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using PolicyManager.Utils;

namespace PolicyManager.Pages;

public sealed partial class WelcomePage
{
    private MainPageModel _frameModel;

    public WelcomePage()
    {
        InitializeComponent();

        Name = ResourceUtil.GetString($"{GetType().Name}/Name");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not GoPageParameter goPageParameter) return;

        _frameModel = goPageParameter.FrameModel;
    }

    private void EdgeButton_Click(object sender, RoutedEventArgs e)
    {
        _frameModel.GoPage(typeof(PolicyPage), new Dictionary<string, object>
        {
            { "policyType", "Edge" },
            { "policyRegistryPath", @"SOFTWARE\Policies\Microsoft\Edge" }
        });
    }

    private void EdgeUpdateButton_Click(object sender, RoutedEventArgs e)
    {
        _frameModel.GoPage(typeof(PolicyPage), new Dictionary<string, object>
        {
            { "policyType", "EdgeUpdate" },
            { "policyRegistryPath", @"SOFTWARE\Policies\Microsoft\EdgeUpdate" }
        });
    }

    private void EdgeWebviewButton_Click(object sender, RoutedEventArgs e)
    {
        _frameModel.GoPage(typeof(PolicyPage), new Dictionary<string, object>
        {
            { "policyType", "EdgeWebview" },
            { "policyRegistryPath", @"SOFTWARE\Policies\Microsoft\Edge\WebView2" }
        });
    }

    private async void GithubButton_OnClick(object sender, RoutedEventArgs e)
    {
        // 打开 Github 仓库
        await Launcher.LaunchUriAsync(new Uri("https://github.com/NXY666/EdgePolicyManager"));
    }

    private async void FeedbackButton_OnClick(object sender, RoutedEventArgs e)
    {
        // 打开 Github Issues
        await Launcher.LaunchUriAsync(new Uri("https://github.com/NXY666/EdgePolicyManager/issues/new/choose"));
    }

    private static int CompareVersion(string v1, string v2)
    {
        var v1S = v1.Split('.');
        var v2S = v2.Split('.');
        var len = Math.Max(v1S.Length, v2S.Length);
        for (var i = 0; i < len; i++)
        {
            var v1I = i < v1S.Length ? int.Parse(v1S[i]) : 0;
            var v2I = i < v2S.Length ? int.Parse(v2S[i]) : 0;
            if (v1I > v2I)
            {
                return 1;
            }

            if (v1I < v2I)
            {
                return -1;
            }
        }

        return 0;
    }

    private async void UpdateButton_OnClick(object sender, RoutedEventArgs e)
    {
        var senderButton = (Button)sender;
        senderButton.IsEnabled = false;

        // 获取电脑中的 Edge 版本（注册表Computer\HKEY_CURRENT_USER\Software\Microsoft\Edge\BLBeacon）
        var edgeVersion = "";
        var rr = RegistryUtil.GetRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Edge\BLBeacon", "version");
        if (rr != null)
        {
            edgeVersion = (string)rr.Value;
        }

        // 获取支持 Edge 版本
        var supportVersion = ResourceUtil.GetEmbeddedPlainText("StaticModels.Policy.SUPPORT_VERSION");

        // 获取最新版本（从网页https://raw.githubusercontent.com/NXY666/EdgePolicyManager/master/StaticModels/Policy/SUPPORT_VERSION获取）
        var latestVersion = "获取失败";
        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync("https://raw.githubusercontents.com/NXY666/EdgePolicyManager/master/StaticModels/Policy/SUPPORT_VERSION");
                if (response.IsSuccessStatusCode)
                {
                    latestVersion = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        // 创建Grid
        var grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());

        grid.RowSpacing = 8;

        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        var titleColor = new SolidColorBrush(Colors.Black);
        var valueColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0x64, 0x64));

        var title1 = new TextBlock { Text = "Edge 版本", Foreground = titleColor };
        title1.SetValue(Grid.RowProperty, 0);
        title1.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title1);

        var value1 = new TextBlock { Text = edgeVersion, Foreground = valueColor };
        value1.SetValue(Grid.RowProperty, 0);
        value1.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value1);

        var title2 = new TextBlock { Text = "当前支持版本", Foreground = titleColor };
        title2.SetValue(Grid.RowProperty, 1);
        title2.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title2);

        var value2 = new TextBlock { Text = supportVersion, Foreground = valueColor };
        value2.SetValue(Grid.RowProperty, 1);
        value2.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value2);

        var title3 = new TextBlock { Text = "最新支持版本", Foreground = titleColor };
        title3.SetValue(Grid.RowProperty, 2);
        title3.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title3);

        var value3 = new TextBlock { Text = latestVersion, Foreground = valueColor };
        value3.SetValue(Grid.RowProperty, 2);
        value3.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value3);


        var dialog = new ContentDialog
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = XamlRoot,
            Title = "版本信息",
            CloseButtonText = "确定",
            // Content = $"本机 Edge 版本：{edgeVersion}\n当前配置版本：{supportVersion}\n最新配置版本：{latestVersion}"
            Content = grid
        };
        await dialog.ShowAsync();

        senderButton.IsEnabled = true;
    }
}