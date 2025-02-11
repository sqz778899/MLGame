using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeBase : SpriteClickHandler
{
    [Header("飞行特效参数")] 
    public EParameter EPara;
    
    internal EffectManager effectManager;
    internal EffectManager _effectManager
    {
        get
        {
            if (effectManager==null)
                effectManager = UIManager.Instance.EffectRoot.GetComponent<EffectManager>();
            return effectManager;
        }
    }
    internal override void OnMouseEnter()
    {
        outLineMat.SetColor("_BaseColor",defaultColor);
        outLineMat.SetColor("_Color",OutlineColor);
        spriteRenderer.material = outLineMat;// 高亮勾边
        if (Input.GetMouseButtonDown(0))
            transform.localScale = defaultScale * 0.8f;
        if (Input.GetMouseButtonUp(0))
            transform.localScale = defaultScale;
    }

    internal override void OnMouseExit()
    {
        spriteRenderer.material = defaultMat;// 还原
    }

}
