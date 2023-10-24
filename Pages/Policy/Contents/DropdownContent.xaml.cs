using System;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using PolicyManager.Models.Policy;
using PolicyManager.Utils;

namespace PolicyManager.Pages.Policy.Contents;

public sealed partial class DropdownContent
{
    public DropdownContent(NotifyPolicyManager policyManager) : base(policyManager)
    {
        InitializeComponent();

        var policyDataOptions = policyManager.PolicyDataOptions;

        // 设置下拉框选项
        var comboBoxItems = policyDataOptions.Select(policyDataOption => new ComboBoxItem
        {
            Content = policyDataOption.Name,
            Tag = policyDataOption
        }).ToList();
        ComboBox.ItemsSource = comboBoxItems;

        // 设置默认选中项
        if (policyManager.UsingPolicyCustomValue)
        {
            foreach (var comboBoxItem in comboBoxItems)
            {
                if (comboBoxItem.Tag is not PolicyDataOption policyDataOption) continue;
                
                if (Convert.ToString(policyDataOption.Value) != Convert.ToString(policyManager.PolicyValue)) continue;
                
                ComboBox.SelectedItem = comboBoxItem;
                break;
            }
        }

        policyManager.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != nameof(PolicyManager.PolicyLevel)) return;
            
            foreach (var comboBoxItem in comboBoxItems)
            {
                if (comboBoxItem.Tag is not PolicyDataOption policyDataOption) continue;
                
                if (Convert.ToString(policyDataOption.Value) != Convert.ToString(PolicyManager.PolicyValue)) continue;

                ComboBox.SelectedItem = comboBoxItem;
                break;
            }
        };
    }

    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBoxItem = ComboBox.SelectedItem as ComboBoxItem;
        if (comboBoxItem?.Tag is not PolicyDataOption policyDataOption) return;
        
        if (Convert.ToString(policyDataOption.Value) == Convert.ToString(PolicyManager.PolicyValue)) return;
        
        PolicyManager.PolicyValue = policyDataOption.Value;
    }
}