using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Spine.Unity;

public class Bullet : DragBase
{
    [Header("重要属性")]
    public int _level;
    public int Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
                OnIDChanged();
            }
        }
    }
    
    public int Damage;     //基础伤
    public int Piercing;
    public int Resonance;
    
    public int FinalDamage; //最终伤害
    public int FinalPiercing;
    public int FinalResonance;
    public ElementalTypes ElementalType;
    public bool IsResonance = false;

    [Header("重要资产")]
    public GameObject Ins;
    public GameObject Edit_a;
    public GameObject Edit_b;
    public BulletInsMode _bulletInsMode;
    public BulletInsMode BulletInsMode
    {
        get => _bulletInsMode;
        set
        {
            if (_bulletInsMode != value)
            {
                _bulletInsMode = value;
                OnIDChanged();
            }
        }
    }
    
    public GameObject HitEffect; // 击中效果预制体
    public SkeletonDataAsset BulletSpineAsset; // 子弹Spine资产
    public SkeletonDataAsset HitSpfxAsset; // 子弹击中效果子弹Spine资产资产
    //宝石镶嵌
    List<Gem> InLayGems;

    //辅助功能
    internal Vector3 forward = new Vector3(1, 0, 0);
    GameObject GroupStar;

    void Awake()
    {
        InLayGems = new List<Gem>();
    }
    
    #region 同步数据
    protected override void OnIDChanged()
    {
        SyncData();
    }
    void InitBulletData()
    {
        switch (BulletInsMode)
        {
            case BulletInsMode.Inner:
                SetInfo(gameObject);
                break;
            case BulletInsMode.Spawner:
                SetInfo(gameObject);
                break;
            case BulletInsMode.EditA:
                Edit_a.SetActive(true);
                Edit_b.SetActive(false);
                SetInfo(Edit_a);
                break;
            case BulletInsMode.EditB:
                Edit_a.SetActive(false);
                Edit_b.SetActive(true);
                SetInfo(Edit_b);
                break;
            default:
                GroupStar = null;
                break;
        }
    }
    
    void SetStart(int Level)
    {
        if (GroupStar == null)return;
        switch (Level)
        {
            case 1:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(false);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }
    
    void SetInfo(GameObject curRoot)
    {
        //...........GroupStar...................
        GroupStar = GetComponentsInChildren<Transform>().
            FirstOrDefault(trans => trans.gameObject.name == "GroupStar")?.gameObject;
        //...........Set Image&&SpineAsset...................
        if (BulletInsMode == BulletInsMode.Inner)
        {
            SkeletonAnimation SkeleSC = curRoot.GetComponentInChildren<SkeletonAnimation>();
            if (SkeleSC == null) return;
            SkeleSC.skeletonDataAsset = BulletSpineAsset;
        }
        else
        {
            Image target = GetComponentsInChildren<Image>()
                .FirstOrDefault(img => img.gameObject.name == "imgBullet");
            if (target == null) return;
            target.sprite = ResManager.instance.GetAssetCache<Sprite>(
                PathConfig.GetBulletImageOrSpinePath(ID, BulletInsMode));
        }
    }
    
    //同步镶嵌槽的宝石数据
    public void AddGem(Gem gem)
    {
        if (gem == null) return;
        InLayGems.Add(gem);
        SyncGemAttributes();
    }

    public void ClearGem()
    {
        InLayGems.Clear();
        SyncGemAttributes();
    }
    
    void SyncGemAttributes()
    {  
        FinalDamage = Damage;
        FinalPiercing = Piercing;
        FinalResonance = Resonance;

        foreach (var gem in InLayGems)  
        {  
            if (gem == null) continue;  

            FinalDamage += gem.Attribute.Damage;
            FinalPiercing += gem.Attribute.Piercing;
            FinalResonance += gem.Attribute.Resonance;
        }
    }
    public override void SyncData()
    {
        InstanceID = gameObject.GetInstanceID();
        BulletJson bulletJson = ToJosn();
        Level = bulletJson.Level;
        Name = bulletJson.Name;
        Damage = bulletJson.Damage;
        Piercing = bulletJson.Piercing;
        Resonance = bulletJson.Resonance;
        ElementalType = (ElementalTypes)bulletJson.ElementalType;
        /*ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>
            (PathConfig.GetBulletImageOrSpinePath(ID, BulletInsMode));*/
        BulletSpineAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletImageOrSpinePath(ID,BulletInsMode));
        //实例化Prefab
        HitEffect = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletSpfxTemplate);
        HitSpfxAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletSpfxPath(ID));
        InitBulletData();
        SetStart(Level);
        //SyncGemAttributes();
    }
    
    public BulletJson ToJosn()
    {
        BulletJson designJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(each => each.ID == ID) ?? new BulletJson();
        
        //同步在游戏内的变量
        designJson.InstanceID = InstanceID;
        designJson.SlotID = SlotID;
        designJson.SlotType = (int)SlotType;
        designJson.FinalDamage = FinalDamage;
        designJson.FinalPiercing = FinalPiercing;
        designJson.FinalResonance = FinalResonance;
        return designJson;
    }
    #endregion

    #region ToolTips
    internal void DisplayTooltips(Vector3 pos)
    {
        if (TooltipsGO == null)
        {
            TooltipsGO = ResManager.instance.CreatInstance(PathConfig.TooltipAsset);
            CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
            curTip.SyncInfo(ID,TipTypes.Bullet);
            TooltipsGO.transform.SetParent(UIManager.Instance.TooltipsRoot.transform);
            TooltipsGO.transform.localScale = Vector3.one;
        }
        TooltipsGO.transform.position = pos;
    }

    internal void DestroyTooltips()
    {
        for (int i = UIManager.Instance.TooltipsRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(UIManager.Instance.TooltipsRoot
                .transform.GetChild(i).gameObject);
        }
    }
    #endregion
}