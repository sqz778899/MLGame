public static class TextProcessor
{
    public static string Parse(string keyOrRaw, bool isKey = false)
    {
        string raw = isKey ? LocalizationManager.Get(keyOrRaw) : keyOrRaw;
        return RichTextFormatter.Format(raw);
    }
}