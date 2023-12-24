using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PolicyManager.Models.Policy;
using PolicyManager.Pages.Policy;
using PolicyManager.Utils;

namespace PolicyManager.Pages;

public class PolicyPageModel
{
    public string PolicyType { init; get; }

    public string PolicyRegistryPath { init; get; }

    public PolicyDetailMap PolicyDetailMap { init; get; }

    public PolicyMenu PolicyMenuList { init; get; }
}

public sealed partial class PolicyPage
{
    private PolicyPageModel _dataContext;

    public PolicyPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not GoPageParameter goPageParameter) return;

        var parameters = goPageParameter.Parameters;

        var policyType = (string)parameters["policyType"];

        _dataContext = new PolicyPageModel
        {
            PolicyType = policyType,
            PolicyRegistryPath = (string)parameters["policyRegistryPath"],
            PolicyDetailMap = ResourceUtil.GetEmbeddedJson<PolicyDetailMap>($"StaticModels.Policy.{policyType}.{{LangCode}}.PolicyDetailMap.json"),
            PolicyMenuList = ResourceUtil.GetEmbeddedJson<PolicyMenu>($"StaticModels.Policy.{policyType}.{{LangCode}}.PolicyMenuList.json")
        };

        foreach (
            var navigationViewItem
            in _dataContext.PolicyMenuList.Select(
                policyMenuItem => new NavigationViewItem
                {
                    Content = policyMenuItem.Name,
                    Icon = IconUtil.GetIconByName(policyMenuItem.Icon),
                    Tag = policyMenuItem
                }
            )
        )
        {
            PolicyNavigationView.MenuItems.Add(navigationViewItem);
        }

        DataContext = _dataContext;
    }

    private void DetailNavigate(PolicyMenuItem policyMenuItem)
    {
        var detailPageModel = new DetailPageModel
        {
            ActivePolicyMenu = policyMenuItem,
            SearchPolicyHandler = SearchPolicy,
            PolicyType = _dataContext.PolicyType
        };
        DetailFrame.Navigate(typeof(DetailPage), detailPageModel);
    }
    
    private void SettingsNavigate()
    {
        var settingsPageModel = new SettingsPageModel
        {
            PolicyType = _dataContext.PolicyType,
            PolicyRegistryPath = _dataContext.PolicyRegistryPath
        };
        DetailFrame.Navigate(typeof(SettingsPage), settingsPageModel);
    }

    private void SearchPolicy(string rawKeyword)
    {
        if (rawKeyword == string.Empty)
        {
            return;
        }

        if (AutoSuggestBox.Text != rawKeyword)
        {
            AutoSuggestBox.Text = rawKeyword;
        }

        var policyMenuItem = new PolicyMenuItem
        {
            Name = ResourceUtil.GetString("PolicyPage/SearchPolicy/SearchResultName"),
            Icon = "Search",
            Identifier = "special:searchresult",
            Items = []
        };

        var splitKeyword = rawKeyword.ToLower().Split(" ");

        List<string> perfectResult = [], betterResult = [], normalResult = [], shitResult = [];

        foreach (var key in _dataContext.PolicyDetailMap.Keys)
        {
            var policyDetail = _dataContext.PolicyDetailMap[key];
            bool foundPerfect = true, foundBetter = true, foundNormal = true, foundShit = true;
            foreach (var keyword in splitKeyword)
            {
                var lowerKeyword = keyword.ToLower();

                if (lowerKeyword != policyDetail.Name && lowerKeyword != key)
                {
                    foundPerfect = false;
                }

                if (
                    !policyDetail.Name.Contains(lowerKeyword, StringComparison.OrdinalIgnoreCase)
                )
                {
                    foundBetter = false;
                }

                if (!policyDetail.ShortDescription.Contains(lowerKeyword, StringComparison.OrdinalIgnoreCase))
                {
                    foundNormal = false;
                }

                if (
                    !policyDetail.Description.Contains(lowerKeyword, StringComparison.OrdinalIgnoreCase)
                )
                {
                    foundShit = false;
                }
            }

            if (foundPerfect)
            {
                perfectResult.Add(key);
            }
            else if (foundBetter)
            {
                betterResult.Add(key);
            }
            else if (foundNormal)
            {
                normalResult.Add(key);
            }
            else if (foundShit)
            {
                shitResult.Add(key);
            }
        }

        policyMenuItem.Items.AddRange(perfectResult);
        policyMenuItem.Items.AddRange(betterResult);
        policyMenuItem.Items.AddRange(normalResult);
        policyMenuItem.Items.AddRange(shitResult);

        PolicyNavigationView.SelectedItem = null;
        DetailNavigate(policyMenuItem);
    }

    private void ConfiguredNavigate()
    {
        var configuredPolicy = new PolicyMenuItem
        {
            Name = ResourceUtil.GetString("PolicyPage/ConfiguredNavigate/ConfiguredPolicyName"),
            Icon = "Flag",
            Identifier = "special:configured",
            Items = []
        };

        foreach (
            var policyKey
            in from key in RegistryUtil.GetRegistryValues(_dataContext.PolicyRegistryPath)
            select key.Name
            into policyKey
            where _dataContext.PolicyDetailMap.ContainsKey(policyKey)
            select policyKey
        )
        {
            configuredPolicy.Items.Add(policyKey);
        }

        DetailNavigate(configuredPolicy);
    }

    private void PolicyNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is not NavigationViewItem selectedItem) return;

        // ÅÐ¶ÏÊÇ²»ÊÇsettings
        if (selectedItem.Tag is string tag)
        {
            switch (tag)
            {
                case "Settings":
                    SettingsNavigate();
                    break;
                case "Configured":
                    ConfiguredNavigate();
                    break;
            }
        }
        else
        {
            DetailNavigate((PolicyMenuItem)selectedItem.Tag);
        }
    }

    private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen) return;

        if (sender.Text == string.Empty)
        {
            sender.ItemsSource = new List();
            return;
        }

        var splitText = sender.Text.ToLower().Split(" ");

        var suggestList = (
            from key in _dataContext.PolicyDetailMap.Keys
            let found = splitText.All(keyword => key.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            where found
            select _dataContext.PolicyDetailMap[key].Name
        ).ToList();

        sender.ItemsSource = suggestList;
    }

    private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        sender.Text = args.SelectedItem.ToString();
    }

    private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        SearchPolicy(args.QueryText);
    }
}