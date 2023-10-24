using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using PolicyManager.Utils;

namespace PolicyManager;

public class GoPageParameter
{
    public MainPageModel FrameModel { get; init; }
    public Dictionary<string, object> Parameters { get; init; }
}

public class MainPageModel : INotifyPropertyChanged
{
    private readonly MainPage _mainPage;

    public MainPageModel(MainPage mainPage)
    {
        _mainPage = mainPage;
    }

    public bool CanGoBack => _mainPage.MainFrame.CanGoBack;

    public bool CantGoBack => !CanGoBack;

    private string _activePageName;

    public string ActivePageName
    {
        get => _activePageName;
        set
        {
            if (value == _activePageName) return;
            _activePageName = value;
            OnPropertyChanged(nameof(ActivePageName));
        }
    }

    public void GoBack()
    {
        if (!CanGoBack) return;

        _mainPage.MainFrame.GoBack();

        OnRouteUpdated();
    }
    
    public void GoPage(Type pageType, Dictionary<string, object> parameters)
    {
        _mainPage.MainFrame.Navigate(pageType, new GoPageParameter
        {
            FrameModel = this,
            Parameters = parameters
        });

        OnRouteUpdated();
    }

    public void GoPage(Type pageType)
    {
        GoPage(pageType, new Dictionary<string, object>());
    }

    private void OnRouteUpdated()
    {
        ActivePageName = ResourceUtil.GetString($"{_mainPage.MainFrame.Content.GetType().Name}/Name");
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CantGoBack));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed partial class MainPage
{
    private readonly MainPageModel _dataContext;

    public MainPage()
    {
        InitializeComponent();

        _dataContext = new MainPageModel(this);

        _dataContext.GoPage(typeof(WelcomePage));
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _dataContext.GoBack();
    }
}