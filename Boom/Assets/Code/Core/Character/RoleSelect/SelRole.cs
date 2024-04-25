using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelRole : MonoBehaviour,IPointerClickHandler
{
    public int roleID;
    public GameObject fxSelBox;
    RoleDes _roleDes;
    SelRoleLogic _curSceneLogic;
    

    void InitData()
    {
        if (_roleDes == null)
            _roleDes = UIManager.Instance.GroupRoleDes.GetComponent<RoleDes>();
        if (_curSceneLogic == null)
            _curSceneLogic = UIManager.Instance.SelRoleLogic.GetComponent<SelRoleLogic>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        InitData();
        fxSelBox.transform.SetParent(eventData.pointerEnter.transform,false);
        fxSelBox.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        _roleDes.CurRole.ID = roleID;
        _roleDes.CurRole.InitRoleData();
        _roleDes.SyncRoleData();
        _curSceneLogic.SyncMultiLa();
    }
}
