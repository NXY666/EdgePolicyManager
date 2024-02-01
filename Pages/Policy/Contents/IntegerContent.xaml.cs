using Microsoft.UI.Xaml;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public sealed partial class IntegerContent
{
    public IntegerContent(NotifyPolicyManager policyManager) : base(policyManager)
    {
        InitializeComponent();
    }

    private void NumberBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        AcceptButton.Visibility = Visibility.Visible;
    }

    private void NumberBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        AcceptButton.Visibility = Visibility.Collapsed;
    }
}