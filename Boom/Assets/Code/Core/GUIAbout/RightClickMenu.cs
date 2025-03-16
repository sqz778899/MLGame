using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickMenu : MonoBehaviour,IPointerExitHandler
{
    public GameObject CurIns;
    public ToolTipsBase CurToolTipsBase;
    public void DeleteIns()
    {
        if (CurIns==null) return;
        BagItemTools<ItemBase>.DeleteObject(CurIns);
        CurToolTipsBase.CurToolTipsMenuState = ToolTipsMenuState.Normal;
        gameObject.SetActive(false);
    }
    
    public void EquipIns()
    {
        if (CurIns==null) return;
        ItemBase curBaseSC = CurIns.GetComponent<ItemBase>();
        if (curBaseSC is Gem curGem)
        {
            SlotBase curEmptySlot = SlotManager.GetEmptySlot(SlotType.GemInlaySlot);
            if (!curEmptySlot) return;
            
            SlotManager.ClearSlot(curGem._data.CurSlot);//清除旧的Slot信息
            curEmptySlot.SOnDrop(CurIns);
        }
        CurToolTipsBase.CurToolTipsMenuState = ToolTipsMenuState.Normal;
        gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurToolTipsBase.CurToolTipsMenuState = ToolTipsMenuState.Normal;
        gameObject.SetActive(false);
    }
}
