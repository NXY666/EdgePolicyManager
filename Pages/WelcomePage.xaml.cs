using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Text;
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

        var grid = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());

        grid.RowSpacing = 8;

        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        var titleColor = new SolidColorBrush(Colors.Black);
        var valueColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0x64, 0x64));

        var mainTitle1 = new TextBlock { Text = "工具", FontWeight = FontWeights.Bold };
        mainTitle1.SetValue(Grid.RowProperty, 0);
        mainTitle1.SetValue(Grid.ColumnProperty, 0);
        mainTitle1.SetValue(Grid.ColumnSpanProperty, 2);
        grid.Children.Add(mainTitle1);

        var title1 = new TextBlock { Text = "当前版本", Foreground = titleColor };
        title1.SetValue(Grid.RowProperty, 1);
        title1.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title1);

        var title2 = new TextBlock { Text = "最新版本", Foreground = titleColor };
        title2.SetValue(Grid.RowProperty, 2);
        title2.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title2);

        var mainTitle2 = new TextBlock { Text = "策略兼容性", FontWeight = FontWeights.Bold };
        mainTitle2.SetValue(Grid.RowProperty, 3);
        mainTitle2.SetValue(Grid.ColumnProperty, 0);
        mainTitle2.SetValue(Grid.ColumnSpanProperty, 2);
        grid.Children.Add(mainTitle2);

        var title3 = new TextBlock { Text = "Edge 版本", Foreground = titleColor };
        title3.SetValue(Grid.RowProperty, 4);
        title3.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title3);

        var title4 = new TextBlock { Text = "当前兼容版本", Foreground = titleColor };
        title4.SetValue(Grid.RowProperty, 5);
        title4.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title4);

        var title5 = new TextBlock { Text = "最新兼容版本", Foreground = titleColor };
        title5.SetValue(Grid.RowProperty, 6);
        title5.SetValue(Grid.ColumnProperty, 0);
        grid.Children.Add(title5);

        var value1 = new TextBlock { Text = "正在获取……", Foreground = valueColor };
        value1.SetValue(Grid.RowProperty, 1);
        value1.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value1);

        var value2 = new TextBlock { Text = "正在获取……", Foreground = valueColor };
        value2.SetValue(Grid.RowProperty, 2);
        value2.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value2);

        var value3 = new TextBlock { Text = "正在获取……", Foreground = valueColor };
        value3.SetValue(Grid.RowProperty, 4);
        value3.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value3);

        var value4 = new TextBlock { Text = "正在获取……", Foreground = valueColor };
        value4.SetValue(Grid.RowProperty, 5);
        value4.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value4);

        var value5 = new TextBlock { Text = "正在获取……", Foreground = valueColor };
        value5.SetValue(Grid.RowProperty, 6);
        value5.SetValue(Grid.ColumnProperty, 1);
        grid.Children.Add(value5);

        var dialog = new ContentDialog
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = XamlRoot,
            Title = "版本信息",
            CloseButtonText = "确定",
            // Content = $"本机 Edge 版本：{edgeVersion}\n当前配置版本：{supportVersion}\n最新配置版本：{latestVersion}"
            Content = grid
        };

        var dialogTask = dialog.ShowAsync();

        // 当前工具版本
        try
        {
            value1.Text = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }
        catch (Exception)
        {
            value1.Text = "未知";
        }
        
        // 最新工具版本
        var task2 = Task.Run(() =>
        {
            using var client = new HttpClient();
            try
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Hello/1.0");
                    
                var response = client.GetAsync("https://api.github.com/repos/NXY666/EdgePolicyManager/releases/latest").Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                // 解析 JSON
                var jsonContent = response.Content.ReadAsStringAsync().Result;
                var jsonElement = JsonDocument.Parse(jsonContent).RootElement;
                return jsonElement.GetProperty("tag_name").GetString()?[1..];
            }
            catch (Exception)
            {
                return "未知";
            }
        });

        // 获取 Edge 版本
        try
        {
            value3.Text = (string)RegistryUtil.GetRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Edge\BLBeacon", "version").Value;
        }
        catch (Exception)
        {
            value3.Text = "未知";
        }

        // 获取当前兼容版本
        try
        {
            value4.Text = ResourceUtil.GetEmbeddedPlainText("StaticModels.Policy.SUPPORT_VERSION");
        }
        catch (Exception)
        {
            value4.Text = "未知";
        }

        // 获取最新版本
        var task5 = Task.Run(() =>
        {
            using var client = new HttpClient();
            try
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Hello/1.0");
                    
                var response = client.GetAsync("https://raw.githubusercontents.com/NXY666/EdgePolicyManager/master/StaticModels/Policy/SUPPORT_VERSION").Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                return "未知";
            }
        });

        senderButton.IsEnabled = true;
        
        value2.Text = await task2;
        value5.Text = await task5;
        await dialogTask;
    }
}