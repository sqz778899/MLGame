using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bullet:DragBase
{
    public BulletData _data; //绝对核心数据

    [Header("表现资产")]
    public GameObject EditA;
    public GameObject EditB;
    public Image IconEditA;
    public Image IconEditB;
    public GameObject EditA_GroupStar;
    public GameObject EditB_GroupStar;
    
    BulletInsMode _bulletInsMode;
    public BulletInsMode BulletInsMode
    {
        get => _bulletInsMode;
        set
        {
            if (_bulletInsMode != value)
            {
                _bulletInsMode = value;
                OnModeChanged();
            }
        }
    }
    
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChanged; // 先退订旧Data的事件
        
        _data = data as BulletData;
        _data.InstanceID= GetInstanceID();
        name = _data.InstanceID.ToString();
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChanged;
            OnDataChanged(); // 立即刷新一遍
        }
    }

    #region 数据同步
    void OnModeChanged()
    {
        if (BulletInsMode == BulletInsMode.EditA)
        {
            EditA.SetActive(true);
            EditB.SetActive(false);
        }
        else
        {
            EditA.SetActive(false);
            EditB.SetActive(true);
        }
    }

    void OnDataChanged()
    {
        LevelStarDisplay();
        IconEditA.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(_data.ID, BulletInsMode.EditA));
        IconEditB.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(_data.ID, BulletInsMode.EditB));
        OnModeChanged();
    }
    #endregion
    
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

    #region 不需要额外关注
    void LevelStarDisplay()
    {
        for (int i = 0; i < EditA_GroupStar.transform.childCount; i++)
            EditA_GroupStar.transform.GetChild(i).gameObject.SetActive(i < _data.Level);
        
        for (int i = 0; i < EditB_GroupStar.transform.childCount; i++)
            EditB_GroupStar.transform.GetChild(i).gameObject.SetActive(i < _data.Level);
    }
    
    void OnDestroy()
    {
        // 确保退订，避免内存泄漏
        if (_data != null)
            _data.OnDataChanged -= OnDataChanged;
    }
    #endregion
}