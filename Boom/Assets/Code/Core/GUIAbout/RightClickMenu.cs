using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickMenu : MonoBehaviour,IPointerExitHandler
{
    public GameObject CurIns;

    public void DeleteIns()
    {
        if (CurIns==null)
            return;
        BagItemManager<ItemBase>.DeleteObject(CurIns);
        Destroy(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
