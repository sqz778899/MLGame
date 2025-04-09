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
    
    internal void OnDataChangedSpawner()
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
            childBulletIns = BulletFactory.CreateBullet(new BulletData(_data.ID,null),BulletInsMode.EditA).gameObject;
            childBulletIns.transform.SetParent(DragManager.Instance.dragRoot.transform,false);
           
            DraggableBullet DraBuSC = childBulletIns.GetComponentInChildren<DraggableBullet>();
            DraBuSC.originalPosition = transform.position;
            DraBuSC.IsSpawnerCreate = true;
            _data.SpawnerCount--;
            
            RectTransform rectTransform = childBulletIns.transform.GetComponent<RectTransform>();
            Vector3 worldPoint;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                    eventData.position, eventData.pressEventCamera, out worldPoint))
                rectTransform.position = worldPoint;
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
    
    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        TooltipsInfo curTooltipsInfo = new TooltipsInfo(_data.Name,_data.Level);

        if (_data.FinalDamage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage, _data.FinalDamage,_data.FinalDamage-_data.Damage);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.FinalPiercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, _data.FinalPiercing,_data.FinalPiercing-_data.Piercing);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.FinalResonance != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, _data.FinalResonance,_data.FinalResonance-_data.Resonance);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        //把元素最后加上
        curTooltipsInfo.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Element, elementType: _data.ElementalType));
        CurTooltipsSC.SetInfo(curTooltipsInfo);
    }
    #endregion

    #region 数据绑定相关
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
    void OnDestroy()
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangedSpawner;
    }
    #endregion
}
