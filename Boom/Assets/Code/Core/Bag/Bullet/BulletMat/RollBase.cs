using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollBase:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public RollBulletMatType CurType;
    public Color OrignalColor;
    public TextMeshProUGUI _rollCost;
    
    internal virtual void Update()
    {
        if (_rollCost != null)   
            _rollCost.text = MainRoleManager.Instance.ShopCost.ToString();
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopUtility.SelOne(eventData.pointerClick);
        DestroyImmediate(gameObject);
    }
}