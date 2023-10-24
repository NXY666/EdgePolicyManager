using Microsoft.UI.Xaml.Controls;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public abstract class PolicyContent : Page
{
    public NotifyPolicyManager PolicyManager { get; }

    public PolicyContent This => this;

    protected PolicyContent(NotifyPolicyManager policyManager)
    {
        PolicyManager = policyManager;

        DataContext = This;
    }
}