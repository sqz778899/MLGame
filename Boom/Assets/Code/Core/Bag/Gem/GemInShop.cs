using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GemInShop : ToolTipsBase,IPointerUpHandler,IPointerDownHandler
{
    public GemData _data;
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangeGem; // 先退订旧Data的事件
        
        _data = data as GemData;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangeGem;
            OnDataChangeGem(); // 立即刷新一遍
        }
    }
    
    EffectManager _effectManager;
    internal EffectManager MEffectManager
    {
        get
        {
            if (_effectManager==null)
                _effectManager = UIManager.Instance.CommonUI.EffectRoot.GetComponent<EffectManager>();
            return _effectManager;
        }
    }

    [Header("商店属性")]
    public int Price;
    public TextMeshProUGUI PriceText;
    public ShopNode CurShopNode;
    
    [Header("基础属性")]
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
        OnDataChangeGem();
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
    public void OnDataChangeGem()
    {
        _data.InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = TrunkManager.Instance.GetGemJson(_data.ID);
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
        
        Price = gemDesignData.Price;
        PriceText.text = Price.ToString();
    }
    #endregion
    
    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        TooltipsInfo curTooltipsInfo = new TooltipsInfo(_data.Name,_data.Level);
        if (_data.Damage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage,_data.Damage);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.Piercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, _data.Piercing);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.Resonance != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, _data.Resonance);
            curTooltipsInfo.AttriInfos.Add(curInfo);
        }
        CurTooltipsSC.SetInfo(curTooltipsInfo);
    }
    #endregion
}
