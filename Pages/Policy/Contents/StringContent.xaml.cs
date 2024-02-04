using Microsoft.UI.Xaml;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public sealed partial class StringContent
{
    public StringContent(NotifyPolicyManager policyManager) : base(policyManager)
    {
        InitializeComponent();
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        AcceptButton.Visibility = Visibility.Visible;
    }

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        AcceptButton.Visibility = Visibility.Collapsed;
    }
}