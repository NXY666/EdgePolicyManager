using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Win32;
using PolicyManager.Models.Policy;

namespace PolicyManager.Utils;

public interface IPolicyManager
{
    /// <summary>
    /// 注册表值类型（永远不变）
    /// </summary>
    RegistryValueKind RegistryValueKind { get; }

    /// <summary>
    /// 注册表值
    /// </summary>
    object RegistryValue { get; }

    /// <summary>
    /// 策略值
    /// </summary>
    object PolicyValue { get; set; }

    bool PolicyDataOptionsExists { get; }

    List<PolicyDataOption> PolicyDataOptions { get; }

    /// <summary>
    /// 策略默认值（永远不变）
    /// </summary>
    object PolicyDefaultValue { get; }

    /// <summary>
    /// 策略显示值
    /// </summary>
    string PolicyShowValue { get; }

    /// <summary>
    /// 策略设定级别
    /// </summary>
    int PolicyLevel { get; set; }

    /// <summary>
    /// 策略默认值是否存在（永远不变）
    /// </summary>
    bool PolicyDefaultValueExists { get; }

    /// <summary>
    /// 策略值是否存在
    /// </summary>
    bool PolicyValueExists { get; }

    /// <summary>
    /// 恢复策略默认值
    /// </summary>
    void RevertPolicyDefaultValue();

    /// <summary>
    /// 是否使用策略默认值
    /// </summary>
    bool UsingPolicyDefaultValue { get; }

    /// <summary>
    /// 是否使用非策略默认值
    /// </summary>
    bool UsingPolicyCustomValue { get; }
}

public class PolicyManager : IPolicyManager
{
    private readonly PolicyDetail _policyDetail;

    public PolicyManager(PolicyDetail policyDetail)
    {
        _policyDetail = policyDetail;
    }

    public RegistryValueKind RegistryValueKind
    {
        get
        {
            return _policyDetail.Registry.ValueKind switch
            {
                "REG_DWORD" => RegistryValueKind.DWord,
                "REG_SZ" => RegistryValueKind.String,
                _ => throw new Exception($"Unsupported value kind: {_policyDetail.Registry.ValueKind}.")
            };
        }
    }

    public object RegistryValue
    {
        get
        {
            return PolicyLevel switch
            {
                1 when _policyDetail.Registry.CanRecommended => RegistryUtil.GetRegistryValue(
                    _policyDetail.Registry.RecommendedPath, _policyDetail.Registry.Name).Value,
                2 when _policyDetail.Registry.CanMandatory => RegistryUtil.GetRegistryValue(
                    _policyDetail.Registry.MandatoryPath, _policyDetail.Registry.Name).Value,
                _ => null
            };
        }
    }

    public object PolicyValue
    {
        get
        {
            var registryValue = RegistryValue;
            if (registryValue == null)
            {
                return PolicyDefaultValue;
            }

            return _policyDetail.DataType switch
            {
                "Integer" => registryValue,
                "Boolean" => (int)registryValue == 1,
                "String" => registryValue,
                _ => throw new Exception($"Unsupported data type {_policyDetail.DataType}.")
            };
        }
        set
        {
            Console.WriteLine($"Set policy value to {value}.");
            switch (_policyDetail.DataType)
            {
                case "Integer":
                case "Boolean":
                case "String":
                    switch (PolicyLevel)
                    {
                        case 1 when _policyDetail.Registry.CanRecommended:
                            RegistryUtil.SetRegistryValue(
                                _policyDetail.Registry.RecommendedPath,
                                _policyDetail.Registry.Name,
                                RegistryValueKind,
                                value);
                            if (_policyDetail.Registry.CanMandatory)
                            {
                                RegistryUtil.DeleteRegistryValue(
                                    _policyDetail.Registry.MandatoryPath,
                                    _policyDetail.Registry.Name);
                            }

                            break;
                        case 2 when _policyDetail.Registry.CanMandatory:
                            RegistryUtil.SetRegistryValue(
                                _policyDetail.Registry.MandatoryPath,
                                _policyDetail.Registry.Name,
                                RegistryValueKind,
                                value);
                            if (_policyDetail.Registry.CanRecommended)
                            {
                                RegistryUtil.DeleteRegistryValue(
                                    _policyDetail.Registry.RecommendedPath,
                                    _policyDetail.Registry.Name);
                            }

                            break;
                    }

                    break;
                default:
                    throw new Exception($"Unsupported data type {_policyDetail.DataType}.");
            }
        }
    }

    public bool PolicyDataOptionsExists => _policyDetail.DataOptions != null;

    public List<PolicyDataOption> PolicyDataOptions => _policyDetail.DataOptions;

    public string PolicyShowValue
    {
        get
        {
            var policyValue = PolicyValue;

            var showValue = !PolicyDefaultValueExists && policyValue == null
                ? ResourceUtil.GetString("RegistryUtil/ShowValue/UnconfiguredText")
                : _policyDetail.DataType switch
                {
                    "Boolean" => (bool)policyValue
                        ? ResourceUtil.GetString("RegistryUtil/ShowValue/EnabledText")
                        : ResourceUtil.GetString("RegistryUtil/ShowValue/DisabledText"),
                    "Integer" => policyValue.ToString(),
                    "String" => policyValue.ToString(),
                    _ => ResourceUtil.GetString("RegistryUtil/ShowValue/UnsupportedText")
                };

            if (UsingPolicyDefaultValue)
            {
                showValue += ResourceUtil.GetString("RegistryUtil/ShowValue/DefaultText");
            }

            return showValue;
        }
    }

    public int PolicyLevel
    {
        get
        {
            if (_policyDetail.Registry.CanMandatory && RegistryUtil.GetRegistryValueExists(_policyDetail.Registry.MandatoryPath, _policyDetail.Registry.Name))
            {
                return 2;
            }

            if (_policyDetail.Registry.CanRecommended && RegistryUtil.GetRegistryValueExists(_policyDetail.Registry.RecommendedPath, _policyDetail.Registry.Name))
            {
                return 1;
            }

            return 0;
        }
        set
        {
            var nowLevel = PolicyLevel;
            if (nowLevel == value)
            {
                return;
            }

            // 当前策略值 > 策略默认值 > 类型默认值
            var policyValue = (PolicyValue ?? PolicyDefaultValue) ?? (PolicyDataOptionsExists
                ? PolicyDataOptions[0].Value
                : _policyDetail.DataType switch
                {
                    "String" => "",
                    "Integer" => 0,
                    "Boolean" => false,
                    _ => throw new Exception($"Unsupported data type {_policyDetail.DataType}.")
                });
            
            Console.WriteLine($"Set policy level to {value}, value is {policyValue}.");
            switch (value)
            {
                case 0:
                    RevertPolicyDefaultValue();
                    break;
                case 1 when _policyDetail.Registry.CanRecommended:
                    switch (_policyDetail.DataType)
                    {
                        case "Integer":
                        case "Boolean":
                        case "String":
                            RegistryUtil.SetRegistryValue(
                                _policyDetail.Registry.RecommendedPath,
                                _policyDetail.Registry.Name,
                                RegistryValueKind,
                                policyValue);
                            break;
                        default:
                            throw new Exception($"Unsupported data type {_policyDetail.DataType}.");
                    }

                    if (_policyDetail.Registry.CanMandatory)
                    {
                        RegistryUtil.DeleteRegistryValue(
                            _policyDetail.Registry.MandatoryPath,
                            _policyDetail.Registry.Name);
                    }

                    break;
                case 2 when _policyDetail.Registry.CanMandatory:
                    switch (_policyDetail.DataType)
                    {
                        case "Boolean":
                        case "Integer":
                        case "String":
                            RegistryUtil.SetRegistryValue(
                                _policyDetail.Registry.MandatoryPath,
                                _policyDetail.Registry.Name,
                                RegistryValueKind,
                                policyValue);
                            break;
                        default:
                            throw new Exception($"Unsupported data type {_policyDetail.DataType}.");
                    }

                    if (_policyDetail.Registry.CanRecommended)
                    {
                        RegistryUtil.DeleteRegistryValue(
                            _policyDetail.Registry.RecommendedPath,
                            _policyDetail.Registry.Name);
                    }

                    break;
            }
        }
    }

    public bool PolicyValueExists => PolicyValue != null;

    public void RevertPolicyDefaultValue()
    {
        if (_policyDetail.Registry.CanMandatory)
        {
            RegistryUtil.DeleteRegistryValue(_policyDetail.Registry.MandatoryPath, _policyDetail.Registry.Name);
        }

        if (_policyDetail.Registry.CanRecommended)
        {
            RegistryUtil.DeleteRegistryValue(_policyDetail.Registry.RecommendedPath, _policyDetail.Registry.Name);
        }
    }

    public object PolicyDefaultValue => _policyDetail.Registry.DefaultValue;

    public bool PolicyDefaultValueExists => PolicyDefaultValue != null;

    public bool UsingPolicyDefaultValue => PolicyLevel == 0;

    public bool UsingPolicyCustomValue => PolicyLevel != 0;
}

public sealed class NotifyPolicyManager : INotifyPropertyChanged, IPolicyManager
{
    private readonly PolicyManager _policyManager;

    public NotifyPolicyManager(PolicyManager policyManager)
    {
        _policyManager = policyManager;
    }

    public RegistryValueKind RegistryValueKind => _policyManager.RegistryValueKind;

    public object RegistryValue => _policyManager.RegistryValue;

    public object PolicyValue
    {
        get => _policyManager.PolicyValue;

        set
        {
            _policyManager.PolicyValue = value;

            OnPropertyChanged(nameof(PolicyValue));
            OnPropertyChanged(nameof(RegistryValue));
            OnPropertyChanged(nameof(PolicyShowValue));
        }
    }

    public bool PolicyDataOptionsExists => _policyManager.PolicyDataOptionsExists;

    public List<PolicyDataOption> PolicyDataOptions => _policyManager.PolicyDataOptions;

    public object PolicyDefaultValue => _policyManager.PolicyDefaultValue;

    public string PolicyShowValue => _policyManager.PolicyShowValue;

    public int PolicyLevel
    {
        get => _policyManager.PolicyLevel;
        set
        {
            _policyManager.PolicyLevel = value;
            OnPropertyChanged(nameof(PolicyLevel));
            OnPropertyChanged(nameof(PolicyValue));
            OnPropertyChanged(nameof(PolicyShowValue));
            OnPropertyChanged(nameof(RegistryValue));
            OnPropertyChanged(nameof(PolicyValueExists));
            OnPropertyChanged(nameof(UsingPolicyDefaultValue));
            OnPropertyChanged(nameof(UsingPolicyCustomValue));
        }
    }

    public bool PolicyDefaultValueExists => _policyManager.PolicyDefaultValueExists;

    public bool PolicyValueExists => _policyManager.PolicyValueExists;

    public void RevertPolicyDefaultValue()
    {
        _policyManager.RevertPolicyDefaultValue();
        OnPropertyChanged(nameof(PolicyLevel));
        OnPropertyChanged(nameof(PolicyShowValue));
        OnPropertyChanged(nameof(UsingPolicyDefaultValue));
        OnPropertyChanged(nameof(UsingPolicyCustomValue));
    }

    public bool UsingPolicyDefaultValue => _policyManager.UsingPolicyDefaultValue;

    public bool UsingPolicyCustomValue => _policyManager.UsingPolicyCustomValue;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}