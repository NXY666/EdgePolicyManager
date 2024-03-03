using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public abstract class PolicyContent : Page
{
    protected PolicyContent(NotifyPolicyManager policyManager)
    {
        NavigationCacheMode = NavigationCacheMode.Disabled;

        PolicyManager = policyManager;

        DataContext = This;
    }

    public NotifyPolicyManager PolicyManager { get; }

    public PolicyContent This => this;
}