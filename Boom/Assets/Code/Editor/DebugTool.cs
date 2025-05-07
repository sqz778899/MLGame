using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public GameObject Root;
    public GameObject Root2;
    public Material _defaultMaterial;

    [Button("解决脚本Miss",ButtonSizes.Large)]
    void DealMissingScript()
    {
        Transform[] all = Root.GetComponentsInChildren<Transform>(true);
        foreach (var each in all)
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(each.gameObject);
    }
    
    [Button("替换材质球",ButtonSizes.Large)]
    void DealMat()
    {
        SpriteRenderer[] allTrans = Root.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var each in allTrans)
        {
            each.material = _defaultMaterial;
        }
    }
    
    [Button("战场模拟",ButtonSizes.Large)] 
    void SetGemData()
    {
        Debug.Log(BattleSimulator.SimulateBattle());
    }
    
    [Button("添加奇迹物件",ButtonSizes.Large)] 
    void SetMO()
    {
        GM.Root.InventoryMgr._InventoryData.EquipMiracleOddity(1);
    }
}
