using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.Win32.UI.Shell;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy;

public sealed class SettingsPageModel : INotifyPropertyChanged
{
    public string PolicyType { get; init; }

    public string PolicyRegistryPath { init; get; }

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
        NavigationCacheMode = NavigationCacheMode.Disabled;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not SettingsPageModel settingsPageModel) return;

        _dataContext = settingsPageModel;
    }

    private async void ImportButton_OnClick(object sender, RoutedEventArgs e)
    {
        var openFile = new OpenFilePicker()
            .SetTitle(ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/OpenPicker/Title"))
            .SetFileFilter($"{_dataContext.PolicyType} {ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/OpenPicker/PolicyConfig")}|*.json")
            .Open();
        if (openFile == null) return;
        if (!RegistryUtil.ImportRegistryValues(await File.ReadAllTextAsync(openFile), _dataContext.PolicyRegistryPath))
        {
            await new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = XamlRoot,
                Title = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/FailDialog/Title"),
                Content = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/FailDialog/Content"),
                PrimaryButtonText = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/FailDialog/PrimaryButtonText"),
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();
        }
        else
        {
            await new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = XamlRoot,
                Title = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/SuccessDialog/Title"),
                Content = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/SuccessDialog/Content"),
                PrimaryButtonText = ResourceUtil.GetString("SettingsPage/ImportButton_OnClick/SuccessDialog/PrimaryButtonText"),
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();
        }
    }

    private async void ExportButton_OnClick(object sender, RoutedEventArgs e)
    {
        // 询问导出路径
        var saveFile = new SaveFilePicker()
            .SetTitle(ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SavePicker/Title"))
            .SetFileName($"{_dataContext.PolicyType}_Policy_Config_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.json")
            .SetDefaultExtension("json")
            .SetFileFilter($"{_dataContext.PolicyType} {ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SavePicker/PolicyConfig")}|*.json")
            .SetOptions((uint)FILEOPENDIALOGOPTIONS.FOS_OVERWRITEPROMPT)
            .Open();
        if (saveFile == null) return;

        // 导出策略
        await File.WriteAllTextAsync(saveFile, RegistryUtil.ExportRegistryValues(_dataContext.PolicyRegistryPath));

        // 提示导出成功
        var result = await new ContentDialog
        {
            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            XamlRoot = XamlRoot,
            Title = ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SuccessDialog/Title"),
            Content = ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SuccessDialog/Content"),
            PrimaryButtonText = ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SuccessDialog/PrimaryButtonText"),
            SecondaryButtonText = ResourceUtil.GetString("SettingsPage/ExportButton_OnClick/SuccessDialog/SecondaryButtonText"),
            DefaultButton = ContentDialogButton.Primary
        }.ShowAsync();

        if (result != ContentDialogResult.Secondary) return;

        FileUtil.OpenFolder(saveFile);
    }

    private static bool CloseProcess(string processName, bool entireProcessTree = true)
    {
        var hasClosed = false;
        foreach (var process in Process.GetProcessesByName(processName))
        {
            process.CloseMainWindow();
            if (!process.HasExited) process.Kill(entireProcessTree);

            hasClosed = true;
        }

        return hasClosed;
    }

    private static void StartProcess(string fileName, string arguments = "")
    {
        var pWeb = new Process();
        pWeb.StartInfo.UseShellExecute = true;
        pWeb.StartInfo.FileName = fileName;
        pWeb.StartInfo.Arguments = arguments;
        pWeb.Start();
    }

    private async void RestartButton_OnClick(object sender, RoutedEventArgs e)
    {
        RestartButton.IsEnabled = false;

        // 关闭edge
        if (!CloseProcess("msedge"))
        {
            // 确认是否重启
            var result = await new ContentDialog
            {
                // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
                XamlRoot = XamlRoot,
                Title = ResourceUtil.GetString("SettingsPage/RestartButton_OnClick/ConfirmDialog/Title"),
                Content = ResourceUtil.GetString("SettingsPage/RestartButton_OnClick/ConfirmDialog/Content"),
                PrimaryButtonText = ResourceUtil.GetString("SettingsPage/RestartButton_OnClick/ConfirmDialog/PrimaryButtonText"),
                CloseButtonText = ResourceUtil.GetString("SettingsPage/RestartButton_OnClick/ConfirmDialog/CloseButtonText"),
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                RestartButton.IsEnabled = true;
                return;
            }
        }

        // 启动edge
        StartProcess("msedge");

        RestartButton.IsEnabled = true;
    }

    private void OpenPolicyButton_OnClick(object sender, RoutedEventArgs e)
    {
        StartProcess("msedge", "edge://policy/");
    }
}