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
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Root);
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

    public SlotView slot;
    [Button(ButtonSizes.Large)] 
    [ButtonGroup("打印POS")]
    void SetGemData()
    {
       GameObject s = Selection.activeGameObject;
       GemNew gem = s.GetComponent<GemNew>();
       GemData dt = new GemData(1, null);
       gem.BindData(dt);
       s.GetComponent<ItemInteractionHandler>().BindData(dt);
    }
    
    
    [Button(ButtonSizes.Large)] 
    void SetSlotView()
    {
        Transform[] trans = Root.GetComponentsInChildren<Transform>(true);
        int count = 1;
        foreach (var each in trans)
        {
            if (each.name.StartsWith("GemSlot") && !each.name.StartsWith("GemSlotRoot"))
            {
                GemSlotInnerView slotView = each.GetComponent<GemSlotInnerView>();
                if (slotView == null)
                {
                    slotView = each.gameObject.AddComponent<GemSlotInnerView>();
                }
                GameObject.DestroyImmediate(each.GetComponent<SlotBase>());
                slotView.ViewSlotType = SlotType.GemBagSlot;
                slotView.ViewSlotID = count;
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
}
