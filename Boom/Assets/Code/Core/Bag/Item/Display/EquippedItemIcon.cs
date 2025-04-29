using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Image Icon;
    public ItemData Data;

    public Vector3 TooltipOffset;

    public void BindingData(ItemData data)
    {
        Data = data;
        Icon.sprite = ResManager.instance.GetItemIcon(data.ID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Data == null) return;
        Vector3 worldPos = UTools.GetWPosByMouse(transform as RectTransform);
        worldPos += TooltipOffset;
        TooltipsManager.Instance.Show(Data.BuildTooltip(), worldPos);
    }

    public void OnPointerExit(PointerEventData eventData)
        => TooltipsManager.Instance.Hide();

    public void OnPointerMove(PointerEventData eventData) 
        => TooltipsManager.Instance.UpdatePosition(TooltipOffset);
}