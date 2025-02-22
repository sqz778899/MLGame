using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GemInShop : ToolTipsBase,IPointerUpHandler,IPointerDownHandler
{
    EffectManager _effectManager;
    internal EffectManager MEffectManager
    {
        get
        {
            if (_effectManager==null)
                _effectManager = UIManager.Instance.EffectRoot.GetComponent<EffectManager>();
            return _effectManager;
        }
    }

    [Header("商店属性")]
    public int Price;
    public TextMeshProUGUI PriceText;
    public ShopNode CurShopNode;
    public int ShopSlotIndex;
    
    [Header("基础属性")]
    public GemAttribute Attribute;
    public Image ItemSprite;

    [Header("显示相关")]
    [ColorUsage(true, true)]
    public Color OutlineColor = Color.white;
    [Header("飞行特效参数")] 
    public EParameter EPara;
    
    internal Material defaultMat;
    internal Material outLineMat
    {
        get
        {
            if (_outLineMat == null)
                _outLineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatUIOutLine);
            return _outLineMat;
        }
    }
    Material _outLineMat;
    
    internal virtual void Start()
    {
        base.Start();
        SyncData();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.localScale = new Vector3(0.8f,0.8f,0.8f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.localScale = Vector3.one;
        if (ShopUtility.SelOne(this))
        {
            CurShopNode.ShopIndexToGemId.Remove(ShopSlotIndex);
            EPara.StartPos = transform.position;
            MEffectManager.CreatEffect(EPara,gameObject,true);
        }
    }
    
    public override void OnPointerMove(PointerEventData eventData)
    {
        base.OnPointerMove(eventData);
        outLineMat.SetColor("_OutlineColor",OutlineColor);
        ItemSprite.material = outLineMat;
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ItemSprite.material = null;
    }

    #region 数据同步相关
    protected override void OnIDChanged() =>  SyncData();
    
    public override void SyncData()
    {
        InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = ToJosn();
        Name = gemDesignData.Name;
        Attribute = gemDesignData.Attribute;
        Price = gemDesignData.Price;
        PriceText.text = Price.ToString();
        if (ItemSprite == null) ItemSprite = GetComponent<Image>();
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    
    internal override void SetTooltipInfo()
    {
        CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
        string des = GetItemAttriInfo();
        curTip.txtTitle.text = Name;
        curTip.txtDescription.text = des;
        curTip.ImgThumbnail.sprite = ItemSprite.sprite;
    }
    
    string GetItemAttriInfo()
    {
        List<string> attributes = new List<string>();
        if (Attribute.Damage != 0) attributes.Add($"伤害 + {Attribute.Damage}");
        if (Attribute.Resonance != 0) attributes.Add($"共振 + {Attribute.Resonance}");
        if (Attribute.Piercing != 0) attributes.Add($"穿透 + {Attribute.Piercing}");

        return string.Join("\n", attributes);
    }
    
    public GemJson ToJosn()
    {
        GemJson gemJson = new GemJson();
        GemJson gemDesignJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == ID) ?? new GemJson();

        gemJson.CopyFrom(gemDesignJson);
        //同步在游戏内的变量
        gemJson.InstanceID = InstanceID;
        return gemJson;
    }
    #endregion
}
