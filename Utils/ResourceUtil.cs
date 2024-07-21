using System;
using System.IO;
using System.Reflection;
using Microsoft.Windows.ApplicationModel.Resources;
using Newtonsoft.Json;

namespace PolicyManager.Utils;

public static class ResourceUtil
{
    private const string Namespace = "PolicyManager";
    
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    
    private static readonly ResourceManager ResourceManager = new ResourceManager();
    
    private static readonly ResourceMap ResourceMap = ResourceManager.MainResourceMap.GetSubtree("Resources");

    public static string GetEmbeddedPlainText(string resourceName)
    {
        // 尝试获取资源的流
        using var stream = Assembly.GetManifestResourceStream($"{Namespace}.{resourceName.Replace("{LangCode}", GetString("Language"))}");
        
        // 资源不存在，返回 null
        if (stream == null) return null;

        // 在这里读取和处理资源
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static T GetEmbeddedJson<T>(string resourceName)
    {
        return JsonConvert.DeserializeObject<T>(GetEmbeddedPlainText(resourceName)) ?? throw new InvalidOperationException("Failed to deserialize JSON data.");
    }

    public static string GetString(string key)
    {
        try
        {
            return ResourceMap.GetValue(key).ValueAsString;
        }
        catch
        {
            return key;
        }
    }
}