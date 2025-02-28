using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickMenu : MonoBehaviour,IPointerExitHandler
{
    public GameObject CurIns;
    public ToolTipsBase CurToolTipsBase;
    public void DeleteIns()
    {
        if (CurIns==null) return;
        BagItemManager<ItemBase>.DeleteObject(CurIns);
        CurToolTipsBase.CurToolTipsMenuState = ToolTipsMenuState.Normal;
        gameObject.SetActive(false);
    }
    
    public void EquipIns()
    {
        if (CurIns==null) return;
        ItemBase curBaseSC = CurIns.GetComponent<ItemBase>();
        if (curBaseSC is Gem)
        {
            Gem curGem = curBaseSC as Gem;
            GemSlot curEmptySlot = null;
            curEmptySlot = MainRoleManager.Instance.GetEmptyGemSlot();//在角色栏找到一个空的GemSlot
            if (!curEmptySlot) return;
            
            SlotManager.ClearBagSlotByID(curGem.SlotID,curGem.CurSlot.SlotType);//清除旧的Slot信息
            curGem.CurSlot = curEmptySlot;//再换Slot信息
            curGem.OnDropEmptySlot();
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
