namespace PolicyManager;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        var mainPage = new MainPage();
        
        SetTitleBar(mainPage.Placeholder);
        
        MainFrame.Content = mainPage;
    }
}