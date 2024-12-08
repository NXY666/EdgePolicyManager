using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Win32;
using PolicyManager.Models.Policy;
using static System.String;

namespace PolicyManager.Utils;

public interface IPolicyManager
{
    /// <summary>
    ///     注册表值类型（永远不变）
    /// </summary>
    RegistryValueKind RegistryValueKind { get; }

    /// <summary>
    ///     注册表值
    /// </summary>
    object RegistryValue { get; }

    /// <summary>
    ///     策略值
    /// </summary>
    object PolicyValue { get; set; }

    bool PolicyDataOptionsExists { get; }

    List<PolicyDataOption> PolicyDataOptions { get; }

    /// <summary>
    ///     策略默认值（永远不变）
    /// </summary>
    object PolicyDefaultValue { get; }

    /// <summary>
    ///     策略显示值
    /// </summary>
    string PolicyShowValue { get; }

    /// <summary>
    ///     策略显示值后缀
    /// </summary>
    string PolicyShowValueSuffix { get; }

    /// <summary>
    ///     策略设定级别
    /// </summary>
    int PolicyLevel { get; set; }

    /// <summary>
    ///     策略默认值是否存在（永远不变）
    /// </summary>
    bool PolicyDefaultValueExists { get; }

    /// <summary>
    ///     策略值是否存在
    /// </summary>
    bool PolicyValueExists { get; }

    /// <summary>
    ///     是否使用策略默认值
    /// </summary>
    bool UsingPolicyDefaultValue { get; }

    /// <summary>
    ///     是否使用非策略默认值
    /// </summary>
    bool UsingPolicyCustomValue { get; }

    /// <summary>
    ///     恢复策略默认值
    /// </summary>
    void RevertPolicyDefaultValue();
}

public class PolicyManager(PolicyDetail policyDetail) : IPolicyManager
{
    public RegistryValueKind RegistryValueKind
    {
        get
        {
            return policyDetail.Registry.ValueKind switch
            {
                "REG_DWORD" => RegistryValueKind.DWord,
                "REG_SZ" => RegistryValueKind.String,
                _ => throw new Exception($"Unsupported value kind: {policyDetail.Registry.ValueKind}.")
            };
        }
    }

    public object RegistryValue
    {
        get
        {
            return PolicyLevel switch
            {
                1 when policyDetail.Registry.CanRecommended => RegistryUtil.GetRegistryValue(
                    policyDetail.Registry.RecommendedPath, policyDetail.Registry.Name).Value,
                2 when policyDetail.Registry.CanMandatory => RegistryUtil.GetRegistryValue(
                    policyDetail.Registry.MandatoryPath, policyDetail.Registry.Name).Value,
                _ => null
            };
        }
    }

    public object PolicyValue
    {
        get
        {
            var registryValue = RegistryValue;
            if (registryValue == null) return PolicyDefaultValue;

            return policyDetail.DataType switch
            {
                "Integer" => registryValue,
                "Boolean" => (int)registryValue == 1,
                "String" => registryValue,
                _ => throw new Exception($"Unsupported data type {policyDetail.DataType}.")
            };
        }
        set
        {
            Console.WriteLine($"Set policy value to {value}.");
            switch (policyDetail.DataType)
            {
                case "Integer":
                case "Boolean":
                case "String":
                    switch (PolicyLevel)
                    {
                        case 1 when policyDetail.Registry.CanRecommended:
                            RegistryUtil.SetRegistryValue(
                                policyDetail.Registry.RecommendedPath,
                                policyDetail.Registry.Name,
                                RegistryValueKind,
                                value);
                            if (policyDetail.Registry.CanMandatory)
                            {
                                RegistryUtil.DeleteRegistryValue(
                                    policyDetail.Registry.MandatoryPath,
                                    policyDetail.Registry.Name);
                            }

                            break;
                        case 2 when policyDetail.Registry.CanMandatory:
                            RegistryUtil.SetRegistryValue(
                                policyDetail.Registry.MandatoryPath,
                                policyDetail.Registry.Name,
                                RegistryValueKind,
                                value);
                            if (policyDetail.Registry.CanRecommended)
                            {
                                RegistryUtil.DeleteRegistryValue(
                                    policyDetail.Registry.RecommendedPath,
                                    policyDetail.Registry.Name);
                            }

                            break;
                    }

                    break;
                default:
                    throw new Exception($"Unsupported data type {policyDetail.DataType}.");
            }
        }
    }

    public bool PolicyDataOptionsExists => policyDetail.DataOptions != null;

    public List<PolicyDataOption> PolicyDataOptions => policyDetail.DataOptions;

    public string PolicyShowValue
    {
        get
        {
            var policyValue = PolicyValue;

            string showValue;

            // 没有默认值，又未配置策略值，则显示未配置
            if (!PolicyDefaultValueExists && policyValue == null)
            {
                showValue = ResourceUtil.GetString("RegistryUtil/ShowValue/UnconfiguredText");
            }
            // 是枚举类型，且策略值在枚举中，则显示枚举值
            else if (PolicyDataOptionsExists && PolicyDataOptions.Exists(option => option.Value == policyValue.ToString()))
            {
                showValue = PolicyDataOptions.Find(option => option.Value == policyValue.ToString()).Name;
            }
            // 一般
            else
            {
                showValue = policyDetail.DataType switch
                {
                    "Boolean" => (bool)policyValue
                        ? ResourceUtil.GetString("RegistryUtil/ShowValue/EnabledText")
                        : ResourceUtil.GetString("RegistryUtil/ShowValue/DisabledText"),
                    "Integer" => policyValue.ToString(),
                    "String" => policyValue.ToString(),
                    _ => ResourceUtil.GetString("RegistryUtil/ShowValue/UnsupportedText")
                };
            }

            return showValue;
        }
    }

    public string PolicyShowValueSuffix
    {
        get
        {
            string showValue;
            // 默认值后缀
            if (UsingPolicyDefaultValue)
            {
                showValue = ResourceUtil.GetString("RegistryUtil/ShowValue/DefaultText");
            }
            // 推荐级别后缀
            else if (PolicyLevel == 1)
            {
                showValue = ResourceUtil.GetString("RegistryUtil/ShowValue/RecommendedText");
            }
            else
            {
                showValue = Empty;
            }

            return showValue;
        }
    }

    public int PolicyLevel
    {
        get
        {
            if (policyDetail.Registry.CanMandatory && RegistryUtil.GetRegistryValueExists(policyDetail.Registry.MandatoryPath, policyDetail.Registry.Name)) return 2;

            if (policyDetail.Registry.CanRecommended && RegistryUtil.GetRegistryValueExists(policyDetail.Registry.RecommendedPath, policyDetail.Registry.Name)) return 1;

            return 0;
        }
        set
        {
            var nowLevel = PolicyLevel;
            if (nowLevel == value) return;

            // 当前策略值 > 策略默认值 > 类型默认值
            var policyValue = (PolicyValue ?? PolicyDefaultValue) ?? (PolicyDataOptionsExists
                ? PolicyDataOptions[0].Value
                : policyDetail.DataType switch
                {
                    "String" => "",
                    "Integer" => 0,
                    "Boolean" => false,
                    _ => throw new Exception($"Unsupported data type {policyDetail.DataType}.")
                });

            Console.WriteLine($"Set policy level to {value}, value is {policyValue}.");
            switch (value)
            {
                case 0:
                    RevertPolicyDefaultValue();
                    break;
                case 1 when policyDetail.Registry.CanRecommended:
                    switch (policyDetail.DataType)
                    {
                        case "Integer":
                        case "Boolean":
                        case "String":
                            RegistryUtil.SetRegistryValue(
                                policyDetail.Registry.RecommendedPath,
                                policyDetail.Registry.Name,
                                RegistryValueKind,
                                policyValue);
                            break;
                        default:
                            throw new Exception($"Unsupported data type {policyDetail.DataType}.");
                    }

                    if (policyDetail.Registry.CanMandatory)
                    {
                        RegistryUtil.DeleteRegistryValue(
                            policyDetail.Registry.MandatoryPath,
                            policyDetail.Registry.Name);
                    }

                    break;
                case 2 when policyDetail.Registry.CanMandatory:
                    switch (policyDetail.DataType)
                    {
                        case "Boolean":
                        case "Integer":
                        case "String":
                            RegistryUtil.SetRegistryValue(
                                policyDetail.Registry.MandatoryPath,
                                policyDetail.Registry.Name,
                                RegistryValueKind,
                                policyValue);
                            break;
                        default:
                            throw new Exception($"Unsupported data type {policyDetail.DataType}.");
                    }

                    if (policyDetail.Registry.CanRecommended)
                    {
                        RegistryUtil.DeleteRegistryValue(
                            policyDetail.Registry.RecommendedPath,
                            policyDetail.Registry.Name);
                    }

                    break;
            }
        }
    }

    public bool PolicyValueExists => PolicyValue != null;

    public void RevertPolicyDefaultValue()
    {
        if (policyDetail.Registry.CanMandatory)
        {
            RegistryUtil.DeleteRegistryValue(policyDetail.Registry.MandatoryPath, policyDetail.Registry.Name);
        }

        if (policyDetail.Registry.CanRecommended)
        {
            RegistryUtil.DeleteRegistryValue(policyDetail.Registry.RecommendedPath, policyDetail.Registry.Name);
        }
    }

    public object PolicyDefaultValue => policyDetail.Registry.DefaultValue;

    public bool PolicyDefaultValueExists => PolicyDefaultValue != null;

    public bool UsingPolicyDefaultValue => PolicyLevel == 0;

    public bool UsingPolicyCustomValue => PolicyLevel != 0;
}

public sealed partial class NotifyPolicyManager(IPolicyManager policyManager) : INotifyPropertyChanged, IPolicyManager
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    public RegistryValueKind RegistryValueKind => policyManager.RegistryValueKind;

    public object RegistryValue => policyManager.RegistryValue;

    public object PolicyValue
    {
        get => policyManager.PolicyValue;

        set
        {
            policyManager.PolicyValue = value;

            OnPropertyChanged(nameof(PolicyValue));
            OnPropertyChanged(nameof(RegistryValue));
            OnPropertyChanged(nameof(PolicyShowValue));
        }
    }

    public bool PolicyDataOptionsExists => policyManager.PolicyDataOptionsExists;

    public List<PolicyDataOption> PolicyDataOptions => policyManager.PolicyDataOptions;

    public object PolicyDefaultValue => policyManager.PolicyDefaultValue;

    public string PolicyShowValue => policyManager.PolicyShowValue;

    public string PolicyShowValueSuffix => policyManager.PolicyShowValueSuffix;

    public int PolicyLevel
    {
        get => policyManager.PolicyLevel;
        set
        {
            policyManager.PolicyLevel = value;
            OnPropertyChanged(nameof(PolicyLevel));
            OnPropertyChanged(nameof(PolicyValue));
            OnPropertyChanged(nameof(PolicyShowValue));
            OnPropertyChanged(nameof(PolicyShowValueSuffix));
            OnPropertyChanged(nameof(RegistryValue));
            OnPropertyChanged(nameof(PolicyValueExists));
            OnPropertyChanged(nameof(UsingPolicyDefaultValue));
            OnPropertyChanged(nameof(UsingPolicyCustomValue));
        }
    }

    public bool PolicyDefaultValueExists => policyManager.PolicyDefaultValueExists;

    public bool PolicyValueExists => policyManager.PolicyValueExists;

    public void RevertPolicyDefaultValue()
    {
        policyManager.RevertPolicyDefaultValue();
        OnPropertyChanged(nameof(PolicyLevel));
        OnPropertyChanged(nameof(PolicyShowValue));
        OnPropertyChanged(nameof(UsingPolicyDefaultValue));
        OnPropertyChanged(nameof(UsingPolicyCustomValue));
    }

    public bool UsingPolicyDefaultValue => policyManager.UsingPolicyDefaultValue;

    public bool UsingPolicyCustomValue => policyManager.UsingPolicyCustomValue;

    private void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}