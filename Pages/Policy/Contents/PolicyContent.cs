using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public abstract class PolicyContent : Page
{
    public NotifyPolicyManager PolicyManager { get; }

    public PolicyContent This => this;

    protected PolicyContent(NotifyPolicyManager policyManager)
    {
        NavigationCacheMode = NavigationCacheMode.Disabled;
        
        PolicyManager = policyManager;

        DataContext = This;
    }
}