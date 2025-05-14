using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltips : MonoBehaviour
{
    public float YOffset = 45f;
    [Header("标题")]
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtLV;
    public TextMeshProUGUI txtItemTitle;
    
    [Header("Group")]
    public GameObject ItemGroup;
    public GameObject OtherGroup;
    [Header("属性")] 
    public GameObject AttriRoot;
    public GameObject OriginAttriGO;
    [Header("描述")]
    public TextMeshProUGUI txtDescription;
    
    [Header("背景")] 
    public GameObject BGSmall;
    public GameObject BGMedium;
    public GameObject BGLarge;
    public GameObject BGItem;
  
    [Header("分割线")]
    public Image IconDrividerLine;
    
    [Header("Badge")]
    public GameObject Badge;

    void Awake()
    {
        txtTitle.color = ColorPalette.NormalTextColor;
        txtLV.color = ColorPalette.NormalTextColor;
    }
    
    public void SetInfo(ToolTipsInfo toolTipsInfo)
    {
        ClearInfo();
        if (toolTipsInfo.CurToolTipsType == ToolTipsType.Gem || 
            toolTipsInfo.CurToolTipsType == ToolTipsType.Bullet)
            SetGemBulletInfo(toolTipsInfo);
        else
            SetItemInfo(toolTipsInfo);
    }
    
    void SetItemInfo(ToolTipsInfo toolTipsInfo)
    {
        ItemGroup.SetActive(true);
        OtherGroup.SetActive(false);
        //加载标题
        txtItemTitle.text = toolTipsInfo.Name;
        //设置背景
        SetBGItem();
        //加载描述
        txtDescription.text = TextProcessor.Parse(toolTipsInfo.Description);
        //设置稀有度颜色
        IconDrividerLine.color = ColorPalette.Rarity(toolTipsInfo.Rarity);
        //
        Badge.SetActive(true);
    }
    
    void SetGemBulletInfo(ToolTipsInfo toolTipsInfo)
    { 
        ItemGroup.SetActive(false);
        OtherGroup.SetActive(true);
        //加载标题
        txtTitle.text = toolTipsInfo.Name;
        if(toolTipsInfo.Level != 0)
            txtLV.text = $"Lv.{toolTipsInfo.Level}";
        else
            txtLV.text = "";
        int attriCount = toolTipsInfo.AttriInfos.Count;
        //设置背景大小
        if (attriCount <= 3) SetBGSmall();
        else if (attriCount <= 4) SetBGMedium();
        else SetBGLarge();
        //逐条加载属性
        for (int i = 0; i < attriCount; i++)
        {
            ToolTipsAttriSingleInfo attriInfo = toolTipsInfo.AttriInfos[i];
            //实例化属性词条UI
            GameObject attriGO = Instantiate(OriginAttriGO, AttriRoot.transform);
            attriGO.SetActive(true);
            Vector2 curPos = attriGO.GetComponent<RectTransform>().anchoredPosition;
            Vector2 newPos = new Vector2(curPos.x, curPos.y - YOffset * i);
            attriGO.GetComponent<RectTransform>().anchoredPosition = newPos;
            //装填属性
            TextMeshProUGUI[] attriSingleTexts = attriGO.GetComponentsInChildren<TextMeshProUGUI>();
            string desStr = "";
            Color valueColor = Color.white;
            switch (attriInfo.Type)
            {
                case ToolTipsAttriType.Damage:
                    desStr = $"{Loc.Get("battle.damage")}:"; valueColor = ColorPalette.DamageValueColor; break;
                case ToolTipsAttriType.Piercing:
                    desStr = $"{Loc.Get("battle.piercing")}:"; valueColor = ColorPalette.PiercingValueColor; break;
                case ToolTipsAttriType.Resonance:
                    desStr = $"{Loc.Get("battle.resonance")}:"; valueColor = ColorPalette.ResonanceValueColor; break;
                case ToolTipsAttriType.Critical:
                    desStr = $"{Loc.Get("battle.critical")}:"; valueColor = ColorPalette.DamageValueColor; break;
                case ToolTipsAttriType.ElementalValue:
                    desStr = $"{Loc.Get("battle.evalue")}:"; valueColor = ColorPalette.ElementalInfusionValue; break;
                case ToolTipsAttriType.Element:
                    desStr = $"{Loc.Get("battle.element")}:"; valueColor = ColorPalette.NormalTextColor; break;
            }
            
            attriSingleTexts[0].text = desStr;
            attriSingleTexts[0].color = ColorPalette.NormalTextColor;
            if (attriInfo.Type != ToolTipsAttriType.Element)
            {
                if (attriInfo.Type == ToolTipsAttriType.Critical)//暴击率用百分比呈现
                    attriSingleTexts[1].text = $"{attriInfo.OriginValue}%";
                else
                    attriSingleTexts[1].text = attriInfo.OriginValue.ToString();
                
                attriSingleTexts[1].color = valueColor;
                if (attriInfo.AddedValue != 0)
                {
                    if (attriInfo.AddedValue < 0)
                        attriSingleTexts[2].text = $"({attriInfo.AddedValue})";
                    else
                        attriSingleTexts[2].text = $"(+{attriInfo.AddedValue})";
                    attriSingleTexts[2].color = valueColor;
                }
                else
                    attriSingleTexts[2].text = "";
            }
            else
            {
                string curEleStr = attriInfo.ElementType switch
                {
                    ElementalTypes.Fire => Loc.Get("battle.fire"),
                    ElementalTypes.Ice => Loc.Get("battle.ice"),
                    ElementalTypes.Thunder => Loc.Get("battle.thunder"),
                    _ => Loc.Get("battle.none")
                };
                attriSingleTexts[1].text = curEleStr;
                attriSingleTexts[1].fontSize -= 5;
                attriSingleTexts[1].color = valueColor;
                attriSingleTexts[2].text = "";
            }
        }
    }

    public void ClearInfo()
    {
        for (int i = AttriRoot.transform.childCount - 1; i >= 0; i--)
            Destroy(AttriRoot.transform.GetChild(i).gameObject);
    }
    
    void SetBGSmall() { BGSmall.SetActive(true); BGMedium.SetActive(false); BGLarge.SetActive(false);BGItem.SetActive(false); }
    void SetBGMedium() { BGSmall.SetActive(false); BGMedium.SetActive(true); BGLarge.SetActive(false);BGItem.SetActive(false); }
    void SetBGLarge() { BGSmall.SetActive(false); BGMedium.SetActive(false); BGLarge.SetActive(true); BGItem.SetActive(false);}
    void SetBGItem() { BGSmall.SetActive(false); BGMedium.SetActive(false); BGLarge.SetActive(false); BGItem.SetActive(true);}
}