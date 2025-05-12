using System;
using System.Text.RegularExpressions;

public static class RichTextFormatter
{
    static readonly Regex RichColorRegex = new(@"#(\w+)\((.*?)\)#");

    public static string Format(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return raw;
        return RichColorRegex.Replace(raw, match =>
        {
            string colorKey = match.Groups[1].Value;
            string content = match.Groups[2].Value;
            
            if (Enum.TryParse(colorKey, out RichTextColorType type))
            {
                string hex = RichTextColorConfig.GetColorHex(type);
                return $"<color={hex}><nobr>{content}</nobr></color>";
            }

            // fallback
            return content;
        });
    }
}
