﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedMember.Global

namespace PolicyManager.Utils;

public class RegistryResult(string path, string name, RegistryValueKind valueKind = RegistryValueKind.None, object value = null)
{
    public string Path { get; } = path;
    public string Name { get; } = name;
    public RegistryValueKind ValueKind { get; } = valueKind;
    public object Value { get; } = value;
}

[method: JsonConstructor]
public class RegistryJsonItem(string name, RegistryValueKind valueKind, object value)
{
    public RegistryJsonItem(RegistryResult registryResult) : this(registryResult.Name, registryResult.ValueKind, registryResult.Value)
    {
    }

    public string Name { get; } = name;
    public RegistryValueKind ValueKind { get; } = valueKind;
    public object Value { get; } = value;
}

public static class RegistryUtil
{
    private static bool CheckPathSafe(string path)
    {
        path = path.ToUpperInvariant();
        if (
            path.StartsWith(@"SOFTWARE\POLICIES\MICROSOFT\EDGE") ||
            path.StartsWith(@"SOFTWARE\POLICIES\MICROSOFT\EDGEUPDATE")
        )
        {
            return true;
        }

        Console.WriteLine("Path is not safe.");
        return false;
    }

    public static RegistryResult GetRegistryValue(RegistryKey baseKey, string path, string name)
    {
        var registryKey = baseKey.OpenSubKey(path);

        if (registryKey == null) return new RegistryResult(path, name);

        RegistryValueKind valueKind;
        try
        {
            valueKind = registryKey.GetValueKind(name);
        }
        catch (Exception)
        {
            valueKind = RegistryValueKind.None;
        }

        var value = registryKey.GetValue(name);

        registryKey.Close();

        return value == null
            ? new RegistryResult(path, name)
            : new RegistryResult(path, name, valueKind, value);
    }

    public static RegistryResult GetRegistryValue(string path, string name)
    {
        return GetRegistryValue(Registry.LocalMachine, path, name);
    }

    public static bool GetRegistryValueExists(string path, string name)
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(path);

        if (registryKey == null) return false;

        var exists = registryKey.GetValue(name) != null;

        registryKey.Close();

        return exists;
    }

    public static bool SetRegistryValue(string path, string name, RegistryValueKind valueKind, object value)
    {
        Console.WriteLine($@"Editing registry {path}\{name} {valueKind} {value}");

        if (!CheckPathSafe(path)) return false;

        var registryKey = Registry.LocalMachine.OpenSubKey(path, true);

        registryKey ??= Registry.LocalMachine.CreateSubKey(path);

        if (registryKey == null) return false;

        try
        {
            registryKey.SetValue(name, value, valueKind);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            registryKey.Close();
        }
    }

    public static bool DeleteRegistryValue(string path, string name)
    {
        Console.WriteLine($@"Deleting registry {path}\{name}");

        if (!CheckPathSafe(path)) return false;

        var registryKey = Registry.LocalMachine.OpenSubKey(path, true);
        if (registryKey == null) return false;

        try
        {
            registryKey.DeleteValue(name);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            registryKey.Close();
        }
    }

    public static List<RegistryResult> GetRegistryValues(string path)
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(path);

        if (registryKey == null) return [];

        var result = new List<RegistryResult>();

        foreach (var name in registryKey.GetValueNames())
        {
            RegistryValueKind valueKind;
            try
            {
                valueKind = registryKey.GetValueKind(name);
            }
            catch (Exception)
            {
                valueKind = RegistryValueKind.None;
            }

            var value = registryKey.GetValue(name);

            result.Add(value == null
                ? new RegistryResult(path, name)
                : new RegistryResult(path, name, valueKind, value));
        }

        registryKey.Close();

        return result;
    }

    public static string ExportRegistryValues(string path)
    {
        var registryResults = GetRegistryValues(path);

        // 创建一个新的json对象
        var json = new JObject
        {
            ["path"] = path
        };

        // 创建一个新的json数组
        var values = new JArray();

        // 遍历所有的结果
        foreach (var value in registryResults.Select(registryResult => new RegistryJsonItem(registryResult)))
        {
            // 将结果添加到json数组中
            values.Add(JObject.FromObject(value));
        }

        json["values"] = values;

        // 转为json文本（压缩）
        return json.ToString(Formatting.None);
    }

    public static bool ImportRegistryValues(string json, string verifyPath = null)
    {
        try
        {
            // 将json文本转为json对象
            var jObject = JObject.Parse(json);

            // 获取path属性
            var path = jObject["path"]?.ToString();

            if (path == null || (verifyPath != null && path != verifyPath)) return false;

            // 获取values属性
            var values = jObject["values"]?.ToObject<List<RegistryJsonItem>>();

            // 如果values属性不存在，返回false
            if (values == null) return false;

            // 遍历所有的值
            foreach (var value in values)
            {
                // 设置注册表值
                SetRegistryValue(path, value.Name, value.ValueKind, value.Value);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("Import registry values failed.");
            return false;
        }
    }
}