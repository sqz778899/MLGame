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

    public void SyncRoleData()
    {
        _imgRole.sprite = CurRole._spRole;
        _bloodGroup.text = CurRole.BloodGroup;
        _zodiacSign.text = CurRole.ZodiacSign;
        _MBTI.text = CurRole.MBTI;
        _description.text = CurRole.Description;
        _attri.text = GetAttriStr(CurRole.Attri);
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
