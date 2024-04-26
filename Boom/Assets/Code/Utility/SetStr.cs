using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetStr : MonoBehaviour
{
    public string _orginStr;
    public float _orginFondSize;
    TextMeshProUGUI _curText;
    MStr _mStr;
    MultiLaEN _curLanguage;
    
    void Awake()
    {
        _curText = GetComponent<TextMeshProUGUI>();
        _orginStr = _curText.text;
        _orginFondSize = _curText.fontSize;
        _curLanguage = MultiLa.Instance.CurLanguage;
        _mStr = new MStr(_curText.text,_curText.fontSize,_curText.font);
        SyncTextData();
    }
    
    void Update()
    {
        if (_curLanguage != MultiLa.Instance.CurLanguage)
        {
            SyncTextData();
            _curLanguage = MultiLa.Instance.CurLanguage;
        }
    }
    
    public void SyncTextData()
    {
        MultiLa.Instance.GetMLStr(_orginStr,_orginFondSize,ref _mStr);
        _curText.text = _mStr.Str;
        _curText.font = _mStr.FondAsset;
        _curText.fontSize = _mStr.FondSize;
    }
}
