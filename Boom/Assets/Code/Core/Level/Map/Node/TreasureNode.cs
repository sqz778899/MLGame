using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    [Header("重要功能")]
    public int GemID;
    public Sprite TreasureOpened;
    public bool isOpened = false;
    
    public void OpenTreasureBox()
    {
        if(isOpened) return;
        //抽宝石
        isOpened = true;
        spriteRenderer.sprite = TreasureOpened;
        
        //创建一个临时的宝石，只用来表现，用完即销毁
        GemSlotController emptyGemSlotController = SlotManager.GetEmptySlotController(SlotType.GemBagSlot);
        GemData rollGemData = new GemData(GemID, emptyGemSlotController);
        string gemName = rollGemData.Name;
        GameObject GemIns = BagItemTools<GemNew>.CreateTempObjectGO(rollGemData,CreateItemType.TempGem);
        
        GemIns.transform.SetParent(transform,false);
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GemIns.transform.position = startPos;
        GemIns.transform.localScale = Vector3.one*0.7f;
        EPara.StartPos = startPos;
        MEffectManager.CreatEffect(EPara,GemIns,false,()=>FloatingGetItemText($"获得{gemName}！"));
       
        InventoryManager.Instance.AddGemToBag(GemID);
    }
}