using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Newtonsoft.Json;

namespace PolicyManager.Utils;

public static class ResourceUtil
{
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    private static ResourceLoader _resourceLoader;

    private const string Namespace = "PolicyManager";

    private const string FallbackLanguage = "en-US";

    private static string GetEmbeddedPlainText(string resourceName)
    {
        // 尝试获取资源的流
        using var stream = Assembly.GetManifestResourceStream($"{Namespace}.{resourceName.Replace("{LangCode}", GetString("Language"))}");


        // 资源不存在，返回 null
        if (stream == null)
        {
            return null;
        }

        // 在这里读取和处理资源
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static T GetEmbeddedJson<T>(string resourceName)
    {
        return JsonConvert.DeserializeObject<T>(GetEmbeddedPlainText(resourceName)) ?? throw new InvalidOperationException("Failed to deserialize JSON data.");
    }

    public static void SetLanguage(string language)
    {
        ResourceContext resourceContext;
        try
        {
            resourceContext = ResourceContext.GetForCurrentView();
        }
        catch (Exception)
        {
            resourceContext = ResourceContext.GetForViewIndependentUse();
        }

        resourceContext.Languages = new[] { language, FallbackLanguage };
        CultureInfo.CurrentCulture = new CultureInfo(language);
    }

    public static string GetString(string key)
    {
        if (_resourceLoader == null)
        {
            try
            {
                _resourceLoader ??= ResourceLoader.GetForCurrentView();
            }
            catch (Exception)
            {
                _resourceLoader ??= ResourceLoader.GetForViewIndependentUse();
            }
        }

        return _resourceLoader.GetString(key);
    }
}