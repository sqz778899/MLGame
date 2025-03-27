using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHeighLight : MonoBehaviour,IPointerMoveHandler,IPointerExitHandler
{
    [Header("显示相关")]
    [ColorUsage(true, true)]
    public Color OutlineColor = Color.white;
    public Image _image;
    [Header("参数")] 
    public bool IsLocked = false;
    
    internal Material defaultMat;
    internal Material outLineMat
    {
        get
        {
            if (_outLineMat == null)
                _outLineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatUIOutLine);
            return _outLineMat;
        }
    }
    Material _outLineMat;
    Material _realOutLineMat;
    
    void Start() => _realOutLineMat = Instantiate(outLineMat);

    public void SetLocked()
    {
        IsLocked = true;
        _image.material = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if(IsLocked) return;
        _realOutLineMat.SetColor("_OutlineColor",OutlineColor);
        _image.material = _realOutLineMat;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(IsLocked) return;
        _image.material = null;
    }
}
