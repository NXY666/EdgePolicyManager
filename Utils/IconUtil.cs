using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.UI.Xaml.Controls;
using PolicyManager.Models.Icon;

namespace PolicyManager.Utils;

public static class IconUtil
{
    private static readonly IconMap IconMap =
        ResourceUtil.GetEmbeddedJson<IconMap>("StaticModels.Icon.IconMap.json");

    private static FontIcon DefaultIcon => new() { Glyph = "\uE74C" };

    public static FontIcon GetIconByName(string iconName)
    {
        try
        {
            return new FontIcon { Glyph = IconMap[iconName] };
        }
        catch (Exception)
        {
            return DefaultIcon;
        }
    }

    public static string GetGlyphByName(string iconName)
    {
        try
        {
            return IconMap[iconName];
        }
        catch (Exception)
        {
            return DefaultIcon.Glyph;
        }
    }

    public static string GetNameByGlyph(string iconGlyph)
    {
        try
        {
            return IconMap.FirstOrDefault(x => x.Value == iconGlyph).Key;
        }
        catch (Exception)
        {
            return DefaultIcon.Glyph;
        }
    }

    public static FontIcon GetIconByGlyph(string iconGlyph)
    {
        try
        {
            return new FontIcon { Glyph = iconGlyph };
        }
        catch (Exception)
        {
            return DefaultIcon;
        }
    }

    public static FontIcon GetIconByTextGlyph(string iconTextGlyph)
    {
        try
        {
            return new FontIcon { Glyph = HttpUtility.HtmlDecode(iconTextGlyph) };
        }
        catch (Exception)
        {
            return DefaultIcon;
        }
    }

    public static FontIcon GetIconByCodeGlyph(string iconCodeGlyph)
    {
        try
        {
            return new FontIcon { Glyph = Regex.Unescape(iconCodeGlyph) };
        }
        catch (Exception)
        {
            return DefaultIcon;
        }
    }
}