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
    
    public virtual void OnPointerDown(PointerEventData eventData) =>
        Spawner(eventData, BulletCreateFlag.Spawner);

    internal void Spawner(PointerEventData eventData,BulletCreateFlag createFlag)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        if (Data.SpawnerCount <= 0) return;

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
