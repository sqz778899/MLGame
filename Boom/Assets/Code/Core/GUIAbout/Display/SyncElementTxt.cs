using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SyncElementTxt : MonoBehaviour
{
    public TextMeshProUGUI Water;
    public TextMeshProUGUI Fire;
    public TextMeshProUGUI Thunder;
    public TextMeshProUGUI Light;
    public TextMeshProUGUI Dark;

    void Update()
    {
        Water.text =$"水：{MainRoleManager.Instance.WaterElement}";
        Fire.text = $"火：{MainRoleManager.Instance.FireElement}";
        Thunder.text = $"雷：{MainRoleManager.Instance.ThunderElement}";
    }
}
