using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetStr : MonoBehaviour
{
    TextMeshProUGUI _curText;
    MStr _mStr;
    void Start()
    {
        _curText = GetComponent<TextMeshProUGUI>();

        _mStr = new MStr(_curText.text,_curText.fontSize,_curText.font);
        SyncTextData();
    }
    
    void Update()
    {
        SyncTextData();
    }
    
    void SyncTextData()
    {
        MultiLa.Instance.GetMLStr(ref _mStr);
        _curText.text = _mStr.Str;
        _curText.font = _mStr.FondAsset;
        _curText.fontSize = _mStr.FondSize;
    }
}
