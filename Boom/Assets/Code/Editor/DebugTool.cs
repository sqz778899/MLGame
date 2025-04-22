using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public GameObject Root;
    public GameObject Root2;
    public Material _defaultMaterial;

    [Button("解决脚本Miss",ButtonSizes.Large)]
    [ButtonGroup("S")]
    void DealMissingScript()
    {
        Transform[] all = Root.GetComponentsInChildren<Transform>(true);
        foreach (var each in all)
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(each.gameObject);
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("替换材质球")]
    void DealMat()
    {
        SpriteRenderer[] allTrans = Root.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var each in allTrans)
        {
            each.material = _defaultMaterial;
        }
    }
    
    [Button(ButtonSizes.Large)] 
    [ButtonGroup("打印POS")]
    void SetGemData()
    {
        Vector3 OnePos = new Vector3(5.75f, 0.37f, 0);
        Vector3 Pos1 = new Vector3(6.12974977f, 12.2808266f, 11.033061f);
        Vector3 Pos2 = new Vector3(8.1297493f, 8.78082657f, 11.033061f);
        Debug.Log($"Pos1 {Vector3.Distance(OnePos,Pos1)}");
        Debug.Log($"Pos2 {Vector3.Distance(OnePos,Pos2)}");
    }
    
    
    [Button(ButtonSizes.Large)] 
    void SetSlotView()
    {
        Transform[] trans = Root.GetComponentsInChildren<Transform>(true);
        int count = 1;
        foreach (var each in trans)
        {
            if (each.name.StartsWith("BagSlot") && !each.name.StartsWith("BagSlotRoot"))
            {
                ItemSlotView slotView = each.GetComponent<ItemSlotView>();
                if (slotView == null)
                    slotView = each.gameObject.AddComponent<ItemSlotView>();
                
                slotView.ViewSlotType = SlotType.ItemBagSlot;
                slotView.ViewSlotID = count;
                each.name = "ItemSlot" + count.ToString("D2");
                count++;
            }
        }
    }
    
    [Button(ButtonSizes.Large)] 
    void SetSlotViewLinke()
    {
        GemSlotView[] mainTrans = Root.GetComponentsInChildren<GemSlotView>(true);
        GemSlotInnerView[] miniTrans = Root2.GetComponentsInChildren<GemSlotInnerView>(true);
        foreach (var eachM in mainTrans)
        {
            foreach (var eMini in miniTrans)
            {
                if (eachM.ViewSlotID == eMini.ViewSlotID)
                {
                    eachM.InnerSlot = eMini;
                }
            }
        }
    }

    public Sprite sprite;
    public DropedRarity rarity;
    [Button(ButtonSizes.Large)]
    void TestBanner()
    {
        GameObject.Find("RewardBannerManager").GetComponent<RewardBannerManager>().ShowReward(sprite, 3,rarity);
    }
}
