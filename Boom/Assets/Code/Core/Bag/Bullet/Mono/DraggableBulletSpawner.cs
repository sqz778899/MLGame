using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class DraggableBulletSpawner :DragBase
{
    // 1) 数据引用
    public BulletData _data;
    // 2) UI 资产
    [Header("表现资产")]
    public Image IconSpawner;
    public GameObject GroupStar;
    public TextMeshProUGUI txtCount;
    // 3) 拖拽中生成的子弹
    public GameObject childBulletIns;
    
    internal override void Start()
    {
        base.Start();
        childBulletIns = null;
    }
    
    public void BindData(BulletData data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangedSpawner; // 先退订旧Data的事件
        
        _data = data;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangedSpawner;
            OnDataChangedSpawner(); // 立即刷新一遍
        }
    }
    
    void OnDataChangedSpawner()
    {
        // 显示数量同步
        txtCount.text = "X" + _data.SpawnerCount;
        // 显示星级
        for (int i = 0; i < GroupStar.transform.childCount; i++)
            GroupStar.transform.GetChild(i).
                gameObject.SetActive(i < _data.Level);
        // 显示Icon
        IconSpawner.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(_data.ID, BulletInsMode.Spawner));
    } 
    
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        HideTooltips();
        if (childBulletIns == null && _data.SpawnerCount > 0)
        {
            childBulletIns = BulletFactory.CreateBullet(_data,BulletInsMode.EditA).gameObject;
            childBulletIns.transform.SetParent(UIManager.Instance.DragObjRoot.transform,false);
           
            DraggableBullet DraBuSC = childBulletIns.GetComponentInChildren<DraggableBullet>();
            DraBuSC.originalPosition = transform.position;
            DraBuSC.IsSpawnerCreate = true;
            _data.SpawnerCount--;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        HideTooltips();
        if (_data.SpawnerCount >= 0 && childBulletIns != null)
        {
            childBulletIns.GetComponentInChildren<DraggableBullet>().DropOneBullet(eventData);
            childBulletIns = null;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HideTooltips();
        if (_data.SpawnerCount >= 0 && childBulletIns != null)
        {
            // 在拖动时，我们把子弹位置设置为鼠标位置
            RectTransform rectTransform = childBulletIns.transform.GetComponent<RectTransform>();
            Vector3 worldPoint;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                    eventData.position, eventData.pressEventCamera, out worldPoint))
                rectTransform.position = worldPoint;
        }
    }
    public override void OnPointerMove(PointerEventData eventData)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            DisplayTooltips(eventData);
        }
    }
    
    void OnDestroy()
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangedSpawner;
    }
}
