using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeightLightEnv : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [ColorUsage(true, true)] 
    public Color OutlineColor;

    Material _OutLineMat;
    Material OutLineMat
    {
        get
        {
            if (_OutLineMat == null)
                _OutLineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatOutLine);
            return _OutLineMat;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
