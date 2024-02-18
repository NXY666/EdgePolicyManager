using PolicyManager.Utils;
using WinUIEx;

namespace PolicyManager;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        this.SetIcon("Assets/icon.ico");
        Title = ResourceUtil.GetString("MainWindow/Title");

        var mainPage = new MainPage();

        SetTitleBar(mainPage.Placeholder);

        MainFrame.Content = mainPage;
    }
}