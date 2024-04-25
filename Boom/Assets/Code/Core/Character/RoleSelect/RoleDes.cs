using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoleDes : MonoBehaviour
{
    public Role CurRole;
    public Image _imgRole;
    public TextMeshProUGUI _bloodGroup;
    public TextMeshProUGUI _zodiacSign;
    public TextMeshProUGUI _MBTI;
    public TextMeshProUGUI _description;
    public TextMeshProUGUI _attri;

    public void Start()
    {
        CurRole = new Role();
        CurRole.ID = 1;
        CurRole.InitRoleData();
        SyncRoleData();
    }

    private void Update()
    {
        SyncRoleData();
    }

    public void SyncRoleData()
    {
        _imgRole.sprite = CurRole._spRole;
        MultiLa.Instance.SyncText(_bloodGroup, CurRole.BloodGroup);
        MultiLa.Instance.SyncText(_zodiacSign, CurRole.ZodiacSign);
        MultiLa.Instance.SyncText(_MBTI, CurRole.MBTI);
        MultiLa.Instance.SyncText(_description, CurRole.Description);
        MultiLa.Instance.SyncText(_attri, GetAttriStr(CurRole.Attri));
    }

    string GetAttriStr(RoleAttri attri)
    {
        string result = "";
        if (attri.StandbyAdd != 0)
        {
            if (attri.StandbyAdd > 0)
                result += "Standby +" + attri.StandbyAdd;
            else
                result += "Standby -" + attri.StandbyAdd;
        }
        return result;
    }
}
