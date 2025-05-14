using System;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    public string addStr;
    
    TextMeshPro text;
    TextMeshProUGUI textUGUI;
    
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        textUGUI = GetComponent<TextMeshProUGUI>();
        Loc.OnLanguageChanged -= Refresh;
        Loc.OnLanguageChanged += Refresh;
        Refresh();
    }
    
    void OnDestroy() => Loc.OnLanguageChanged -= Refresh;
   
    public void Refresh()
    {
        string val = $"{Loc.Get(key)}{addStr}";
        if (text != null)
        {
            text.font = FontRegistry.GetCurrentFont();
            if (!string.IsNullOrEmpty(key)) text.text = val;
        }

        if (textUGUI != null)
        {
            textUGUI.font = FontRegistry.GetCurrentFont();
            if (!string.IsNullOrEmpty(key)) textUGUI.text = val;
        }
    }
}