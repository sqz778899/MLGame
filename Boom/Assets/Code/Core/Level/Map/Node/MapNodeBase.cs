using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class MapNodeBase : SpriteClickHandler
{
    
    public Transform NodeTextNode;
    [Header("飞行特效参数")] 
    public EParameter EPara;
    EffectManager _effectManager;
    [Header("渲染层级相关")]
    public string SortingLayerName = "";
    internal EffectManager MEffectManager
    {
        get
        {
            if (_effectManager==null)
                _effectManager = UIManager.Instance.CommonUI.EffectRoot.GetComponent<EffectManager>();
            return _effectManager;
        }
    }

    #region 浮动消息滑块
    void SetFloatingIns(Transform textNode,out FloatingDamageText textSc)
    {
        GameObject textIns = ResManager.instance.CreatInstance(PathConfig.TxtFloatingPB);
        //1)设置渲染层级
        int targetLayerID = SortingLayer.NameToID(SortingLayerName);
        Renderer[] renderers = textIns.GetComponentsInChildren<Renderer>();
        renderers.ForEach(r => r.sortingLayerID = targetLayerID);
        //2)设置脚本
        textSc = textIns.GetComponent<FloatingDamageText>();
        textIns.transform.SetParent(textNode.transform,false);
    }
    
    internal virtual void FloatingGetItemText(string Content)
    {
        Transform textNode = PlayerManager.Instance.RoleInMapGO.GetComponent<RoleInMap>().TextNode;
        SetFloatingIns(textNode,out FloatingDamageText textSc);
        textSc.AnimateText($"{Content}",new Color(218f/255f,218f/255f,218f/255f,1f));
    }
    
    internal virtual void FloatingText(string Content,Color col = default)
    {
        //NodeTextNode
        if (col == default)
        {
            col = new Color(218f / 255f, 218f / 255f, 218f / 255f, 1f);
        }
        SetFloatingIns(NodeTextNode,out FloatingDamageText textSc);
        textSc.AnimateText($"{Content}",col);
    }
    #endregion
}
