using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SelRole : MonoBehaviour,IPointerClickHandler
{
    [Header("重要资产")]
    public int roleID;
    public GameObject FXSelBox;
    [Header("结点信息")] 
    public Transform textFlotingNode;
    public bool IsLocked = false;
    
    RoleDes _roleDes;
    SelRoleLogic _curSceneLogic;
    
    void InitData()
    {
        _roleDes ??= UIManager.Instance.GroupRoleDes.GetComponent<RoleDes>();
        _curSceneLogic ??= _curSceneLogic = UIManager.Instance.SelRoleLogic.GetComponent<SelRoleLogic>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsLocked)
        {
            FloatingText("此角色还未解锁");
            return;
        }
        InitData();
        FXSelBox.transform.SetParent(eventData.pointerEnter.transform,false);
        FXSelBox.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        _roleDes.CurRole.ID = roleID;
        _roleDes.CurRole.InitRoleData();
        _roleDes.SyncRoleData();
        _curSceneLogic.SyncMultiLa();
    }
    
    internal virtual void FloatingText(string Content,Color col = default)
    {
        //NodeTextNode
        if (col == default)
        {
            col = new Color(218f / 255f, 218f / 255f, 218f / 255f, 1f);
        }
        SetFloatingIns(textFlotingNode,out FloatingDamageText textSc);
        textSc.AnimateTextUI($"{Content}",col);
    }
    
    void SetFloatingIns(Transform textNode,out FloatingDamageText textSc)
    {
        GameObject textIns = ResManager.instance.CreatInstance(PathConfig.TxtFloatingUIPB);
        textSc = textIns.GetComponent<FloatingDamageText>();
        textIns.transform.SetParent(textNode.transform,false);
        textIns.transform.position = transform.position;
    }
}
