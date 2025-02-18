using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeBase : SpriteClickHandler
{
    [Header("飞行特效参数")] 
    public EParameter EPara;
    
    MapLogic _mapLogic;
    internal MapLogic MMapLogic
    {
        get
        {
            if (_mapLogic == null)
                _mapLogic = UIManager.Instance.MapLogicGO.GetComponent<MapLogic>();
            return _mapLogic;
        }   
    }
    
    EffectManager _effectManager;
    internal EffectManager MEffectManager
    {
        get
        {
            if (_effectManager==null)
                _effectManager = UIManager.Instance.EffectRoot.GetComponent<EffectManager>();
            return _effectManager;
        }
    }
    
    internal virtual void FloatingGetItemText(string Content,int Count = 1)
    {
        GameObject textIns = ResManager.instance.CreatInstance(PathConfig.TxtGetItemPB);
        Transform textNode = MainRoleManager.Instance.MainRoleIns.GetComponent<RoleInner>().TextNode;
        textIns.transform.SetParent(textNode.transform,false);
        FloatingDamageText textSc = textIns.GetComponent<FloatingDamageText>();
        textSc.AnimateText($"{Content} + {Count}",new Color(218f/255f,218f/255f,218f/255f,1f));
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
