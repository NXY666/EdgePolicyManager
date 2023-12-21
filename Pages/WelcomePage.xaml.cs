using System;
using System.Collections.Generic;
using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
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
}