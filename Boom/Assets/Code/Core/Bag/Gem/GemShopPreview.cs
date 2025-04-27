using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemShopPreview : ItemBase, IItemInteractionBehaviour,IHighlightableUI,IPressEffect
{
    public GemData Data { get; private set; }

    [Header("表现组件")]
    public Image ItemSprite;
    public TextMeshProUGUI PriceText;
    public EParameter EPara;

    [Header("高亮表现")]
    [ColorUsage(true, true)]
    public Color OutlineColor = Color.white;

    Material _outlineMat;
    RectTransform _rectTransform;
    EffectManager _effectManager;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _effectManager = EternalCavans.Instance._EffectManager;
        _outlineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatUIOutLine);
    }

    public override void BindData(ItemDataBase data)
    {
        Data = data as GemData;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (Data == null) return;

        GemJson design = TrunkManager.Instance.GetGemJson(Data.ID);
        ItemSprite.sprite = ResManager.instance.GetGemIcon(Data.ID);
        PriceText.text = design.Price.ToString();
    }

    #region 交互逻辑（仅启用点击 + Tooltips）
    public void OnDoubleClick() {}
    public void OnRightClick() {}
    public void OnBeginDrag() {}
    public void OnEndDrag() {}
    
    public void OnPressDown() => _rectTransform.localScale = new Vector3(0.8f,0.8f,0.8f);

    public void OnPressUp()=> _rectTransform.localScale = Vector3.one;
    
    public void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            _outlineMat.SetColor("_OutlineColor", OutlineColor);
            ItemSprite.material = _outlineMat;
        }
        else
            ItemSprite.material = null;
    }

    public void OnClick()
    {
        if (ShopUtility.SelOne(this))
        {
            EPara.StartPos = transform.position;
            _effectManager.CreatEffect(EPara, gameObject);
        }
    }
    
    public bool CanDrag => false;
    #endregion
}