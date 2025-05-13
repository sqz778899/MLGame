using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BulletSpawner:ItemBase,
    IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    public BulletData Data { get; private set; }

    [Header("UI")]
    public Image Icon;
    public TextMeshProUGUI txtCount;
    public Transform StarGroup;
    internal RectTransform rectTransform;
    
    float lastClickTime = -10f;
    const float doubleClickThreshold = 0.3f; // 同样的双击间隔
    
    public virtual void OnPointerDown(PointerEventData eventData) =>
        Spawner(eventData, BulletCreateFlag.Spawner);

    //双击自动装备子弹功能
    public bool IsDoubleClick()
    {
        float time = Time.time;
        bool isDouble = time - lastClickTime < doubleClickThreshold;
        lastClickTime = time;

        if (isDouble)
        {
            Debug.Log("双击！");
            ISlotController empty = SlotManager.GetEmptySlotController(SlotType.CurBulletSlot);
            if (empty == null) return false; //没有空槽位,不走自动装备子弹的逻辑
            
            // 直接装备到当前槽位
            // 克隆一份新的数据
            BulletData newData = new BulletData(Data.ID, null);
            BulletSlotController bulletController = empty as BulletSlotController;
            if (bulletController == null || !bulletController.CanAccept(newData)) return false;
            // Spawner 减去 1
            Data.SpawnerCount--;
            // 创建新子弹
            Bullet bulletSC = BulletFactory.CreateBullet(newData, BulletInsMode.EditA) as Bullet;
            bulletController.Assign(newData,bulletSC.gameObject);
            return true;
        }
        return false;
    }


    internal void Spawner(PointerEventData eventData,BulletCreateFlag createFlag)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (Data.SpawnerCount <= 0) return;
        if(IsDoubleClick()) return; //执行双击逻辑

        // 克隆一份新的数据
        BulletData newData = new BulletData(Data.ID, null);
        // 减去 1
        Data.SpawnerCount--;
        // 创建新子弹
        Bullet bulletSC = BulletFactory.CreateBullet(newData, BulletInsMode.EditA) as Bullet;
        bulletSC.transform.SetParent(DragManager.Instance.dragRoot, false);
        bulletSC.transform.position = UTools.GetWPosByMouse(rectTransform);
        bulletSC.CreateFlag = createFlag;
        //用DragManager管理后续拖拽
        DragManager.Instance.ForceDrag(bulletSC.gameObject,eventData);
    }

    #region 不关心的部分
    void Awake() => rectTransform = GetComponent<RectTransform>();
    
    public override void BindData(ItemDataBase data)
    {
        Data = data as BulletData;
        Data.OnDataChanged -= RefreshUI;
        Data.OnDataChanged += RefreshUI;
        RefreshUI();
    }
    
    internal void RefreshUI()
    {
        // 设置图标
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(Data.ID, BulletInsMode.Spawner));
        // 设置数量
        txtCount.text = $"x{Data.SpawnerCount}";
        // 星级显示
        for (int i = 0; i < StarGroup.childCount; i++)
            StarGroup.GetChild(i).gameObject.SetActive(i < Data.Level);
    }
    
    public void OnPointerEnter(PointerEventData eventData) => ShowTooltips();
    
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 pos = UTools.GetWPosByMouse(rectTransform);
        if (Data.CurSlotController != null)
            pos += Data.CurSlotController.TooltipOffset;
        TooltipsManager.Instance.UpdatePosition(pos);
    }

    public void OnPointerExit(PointerEventData eventData) => TooltipsManager.Instance.Hide();
    
    void ShowTooltips()
    {
        if (Data is ITooltipBuilder builder)
        {
            Vector3 pos = UTools.GetWPosByMouse(rectTransform);
            if (Data.CurSlotController != null)
                pos += Data.CurSlotController.TooltipOffset;
            TooltipsManager.Instance.Show(builder.BuildTooltip(), pos);
        }
        TooltipsManager.Instance.Show(Data.BuildTooltip(), UTools.GetWPosByMouse(rectTransform));
    }

    void OnDestroy() =>
        Data.OnDataChanged -= RefreshUI;
    #endregion
}
