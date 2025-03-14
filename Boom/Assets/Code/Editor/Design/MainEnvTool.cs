using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MainEnvTool
{
    [Title("Menu")]
    [Button("传送门开启",ButtonSizes.Large),PropertyOrder(0)]
    void ResetAll()
    {
        GameObject portal = GameObject.Find("MenuRoot").transform.GetChild(0).gameObject;
        portal.SetActive(!portal.activeSelf);
    }   
}