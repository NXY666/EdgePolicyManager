using System;
using Microsoft.Win32;

namespace PolicyManager.Utils;

public class RegistryResult
{
    public string Path { get; }
    public string Name { get; }
    public RegistryValueKind ValueKind { get; }
    public object Value { get; }

    public RegistryResult(string path, string name)
    {
        Path = path;
        Name = name;
        ValueKind = RegistryValueKind.None;
    }

    public RegistryResult(string path, string name, RegistryValueKind valueKind, object value)
    {
        Path = path;
        Name = name;

        ValueKind = valueKind;
        Value = value;
    }
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

    public static RegistryResult GetRegistryValue(string path, string name)
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(path);

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

    public static bool GetRegistryValueExists(string path, string name)
    {
        var registryKey = Registry.LocalMachine.OpenSubKey(path);

        if (registryKey == null)
        {
            return false;
        }

        var exists = registryKey.GetValue(name) != null;

        registryKey.Close();

        return exists;
    }

    public static bool SetRegistryValue(string path, string name, RegistryValueKind valueKind, object value)
    {
        Console.WriteLine($@"Editing registry {path}\{name} {valueKind} {value}");

        if (!CheckPathSafe(path))
        {
            return false;
        }

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

        if (!CheckPathSafe(path))
        {
            return false;
        }

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
}