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
    
    [Title("图书馆")]
    [Button("创建天赋树",ButtonSizes.Large),PropertyOrder(1)]
    void CreateLibrary()
    {
        TalentRoot sc =  GameObject.Find("TalentRoot").GetComponent<TalentRoot>();
        for (int i = sc.LineRoot.transform.childCount-1; i >=0; i--)
            GameObject.DestroyImmediate(sc.LineRoot.transform.GetChild(i).gameObject);
        sc.InitTalentRoot();
    } 
}