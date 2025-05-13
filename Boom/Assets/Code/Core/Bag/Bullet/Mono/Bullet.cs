using UnityEngine;
using UnityEngine.UI;

public class Bullet : ItemBase, IItemInteractionBehaviour
{
    public BulletData Data { get; private set; }
    
    [Header("表现资产")]
    public Image IconEditA;
    public Image IconEditB;
    public GameObject GroupStarEditA;
    public GameObject GroupStarEditB;
    public GameObject EditA;
    public GameObject EditB;
   
    public BulletCreateFlag CreateFlag;
    public BulletInsMode Mode { get; private set; } = BulletInsMode.EditA;
    RectTransform rectTransform;
    
    void RefreshUI()
    {
        IconEditA.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(Data.ID, BulletInsMode.EditA));

        IconEditB.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(Data.ID, BulletInsMode.EditB));

        SetStarCount(GroupStarEditA, Data.Level);
        SetStarCount(GroupStarEditB, Data.Level);
        SwitchMode(Mode);
    }

    void SetStarCount(GameObject group, int count)
    {
        for (int i = 0; i < group.transform.childCount; i++)
            group.transform.GetChild(i).gameObject.SetActive(i < count);
    }

    public void SwitchMode(BulletInsMode mode)
    {
        Mode = mode;
        EditA.SetActive(mode == BulletInsMode.EditA);
        EditB.SetActive(mode == BulletInsMode.EditB);
    }
    
    /// <summary>
    /// 拖拽失败时手动调用
    /// </summary>
    public void OnDragCanceled()=>GM.Root.InventoryMgr.ReturnToSpawner(gameObject,Data);

    #region IItemInteractionBehaviour 实现
    public void OnBeginDrag() {}
    public void OnEndDrag() {}
    public void OnClick(){}
    public bool CanDrag => true;
    public void OnDoubleClick()
    {
        var from = Data.CurSlotController;
        var toSlot = (from.SlotType == SlotType.BulletSlot)
            ? SlotManager.GetEmptySlotController(SlotType.CurBulletSlot)
            : SlotManager.GetEmptySlotController(SlotType.BulletSlot);

        toSlot?.Assign(Data, gameObject);
    }

    public void OnRightClick()
    {
        var mousePos = UTools.GetWPosByMouse(rectTransform);
        RightClickMenuManager.Instance.Show(gameObject, mousePos);
    }
    #endregion

    #region 数据初始化绑定卸载等
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        SwitchMode(Mode);
    }

    public override void BindData(ItemDataBase data)
    {
        if (Data != null)
            Data.OnDataChanged -= RefreshUI;

        Data = data as BulletData;

        if (Data != null)
        {
            Data.OnDataChanged += RefreshUI;
            RefreshUI();
        }
    }

    void OnDestroy()
    {
        if (Data != null)
            Data.OnDataChanged -= RefreshUI;
    }
    #endregion
}

public enum BulletCreateFlag
{
    None = 0,
    Spawner = 1,
    SpawnerInner = 2,
    Spawnered = 3
}