using PolicyManager.Utils;

namespace PolicyManager;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        Title = ResourceUtil.GetString("MainWindow/Title");

        var mainPage = new MainPage();

        SetTitleBar(mainPage.Placeholder);

        MainFrame.Content = mainPage;
    }
}