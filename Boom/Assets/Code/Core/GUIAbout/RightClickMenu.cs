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

    public void OnPointerExit(PointerEventData eventData)
    {
        CurToolTipsBase.CurToolTipsMenuState = ToolTipsMenuState.Normal;
        gameObject.SetActive(false);
    }
}
