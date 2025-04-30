using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GemInner : ItemBase, IBeginDragHandler, IEndDragHandler, 
    IDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler,IPointerClickHandler
{
    [Header("UI元素")]
    public Image Icon;

    [HideInInspector] public GemData Data;
    [HideInInspector] public Gem SourceGem; // 指向主 GemNew，用于拖拽反馈

    RectTransform rectTransform;
    BagRootMini _bagRootMini;
    Transform originalParent;
    IItemInteractionBehaviour behaviour;
    float lastClickTime;
    const float doubleClickThreshold = 0.3f;
    public Action OnGemDragged;
    void Awake() => rectTransform = GetComponent<RectTransform>();

    void Start()
    {
        behaviour = SourceGem.GetComponent<IItemInteractionBehaviour>();
        //战场镜头移动消息注册
        _bagRootMini = EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>();
        OnGemDragged += _bagRootMini.BulletDragged;
    }

    public override void BindData(ItemDataBase data)
    {
        Data = data as GemData;
        Data.OnDataChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy() => Data.OnDataChanged -= RefreshUI;

    void RefreshUI() =>
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetGemPath(Data.ImageName));

    public void OnPointerEnter(PointerEventData eventData) => ShowTooltips();

    public void OnPointerExit(PointerEventData eventData) => TooltipsManager.Instance.Hide();

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = eventData.pointerDrag.transform.parent;
        
        transform.SetParent(DragManager.Instance.dragRoot.transform);
        
        TooltipsManager.Instance.Hide();
        TooltipsManager.Instance.Disable();
        OnGemDragged?.Invoke();//开始拖拽，战场镜头移动
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) return;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var worldPoint);
        transform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 尝试放到一个 GemSlot
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        bool dropped = false;
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotView slotView))
            {
                if (!slotView.Controller.CanAccept(Data)) continue;//先判断是否合法
               
                //如果槽位已满，且是可交换的
                GemSlotController s = slotView.Controller as GemSlotController;
                if (s == null) continue;
               
                if (!s.IsEmpty && slotView.Controller.CurData != Data)
                    SlotManager.Swap(s.CurData, Data);
                else
                    slotView.Controller.Assign(Data, SourceGem.gameObject); // 回收主 Gem
                dropped = true;
            }
        }
        
        if (!dropped)
        {
            transform.SetParent(originalParent,true);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
            Destroy(gameObject);//销毁自己,因为无论如何，主Gem都会投影过来
        
        TooltipsManager.Instance.Enable();
        ShowTooltips();
    }
    
    
    public void OnPointerMove(PointerEventData eventData)
    {
        if (Data.CurSlotController == null) return;
       
        GemSlotController s = Data.CurSlotController as GemSlotController;
        Vector3 pos = UTools.GetWPosByMouse(rectTransform) + s.TooltipOffset;
        TooltipsManager.Instance.UpdatePosition(pos);
    }

    void ShowTooltips()
    {
        if (Data is ITooltipBuilder builder)
        {
            Vector3 pos = UTools.GetWPosByMouse(rectTransform);
            if (Data.CurSlotController != null)
            {
                GemSlotController s = Data.CurSlotController as GemSlotController;
                pos += s.TooltipOffset;
            }
            TooltipsManager.Instance.Show(builder.BuildTooltip(), pos);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            behaviour?.OnDoubleClick();
            TooltipsManager.Instance.Hide();
        }
        lastClickTime = Time.time;
    }
}
