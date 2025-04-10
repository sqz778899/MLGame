using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BulletSpawnerNew:ItemBase,
    IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    public BulletData Data { get; private set; }

    [Header("UI")]
    public Image Icon;
    public TextMeshProUGUI txtCount;
    public Transform StarGroup;

    RectTransform rectTransform;

    void Awake() => rectTransform = GetComponent<RectTransform>();

    void Update()
    {
        Debug.Log($"SpawnerCount {Data.SpawnerCount}");
        Debug.Log($"[Spawner] Origin Data Hash: {Data.GetHashCode()}");
    }

    public override void BindData(ItemDataBase data)
    {
        Data = data as BulletData;
        RefreshUI();
    }

    void RefreshUI()
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
    
    public void OnDragCanceled()
    {
        Data.SpawnerCount++;
        RefreshUI();
    }

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
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Data.SpawnerCount <= 0) return;

        // 克隆一份新的数据
        BulletData newData = new BulletData(Data.ID, null);
        // 减去 1
        Data.SpawnerCount--;
        RefreshUI();
        // 创建新子弹
        GameObject bulletGO = BulletFactory.CreateBullet(newData, BulletInsMode.EditA).gameObject;
        bulletGO.transform.SetParent(DragManager.Instance.dragRoot, false);
        bulletGO.transform.position = UTools.GetWPosByMouse(rectTransform);
        var bulletSC = bulletGO.GetComponent<BulletNew>();
        bulletSC.Spawner = this;//让拖出的子弹持有“父 Spawner”引用
        //用DragManager管理后续拖拽
        DragManager.Instance.ForceDrag(bulletGO,eventData);
    }
}
