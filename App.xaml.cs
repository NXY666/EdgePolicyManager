﻿using System;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PolicyManager;

/// <summary>
///     Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App
{
    /// <summary>
    ///     Initializes the singleton application object.  This is the first line of authored code
    ///     executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        // Set base directory env var for PublishSingleFile support (referenced by SxS redirection)
        Environment.SetEnvironmentVariable("MICROSOFT_WINDOWSAPPRUNTIME_BASE_DIRECTORY", AppContext.BaseDirectory);
        InitializeComponent();
    }

    public static Window MWindow { get; private set; }

    /// <summary>
    ///     Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MWindow = new MainWindow();
        MWindow.ExtendsContentIntoTitleBar = true;
        MWindow.Activate();
    }
}