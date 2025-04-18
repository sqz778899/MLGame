using System.Collections.Generic;
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
    
    [Header("字体颜色")]
    public Color TitleColor;
    public Color DescriptionColor;
    public Color DamageValueColor;
    public Color PiercingValueColor;
    public Color ResonanceValueColor;

    [Header("道具稀有度颜色")] 
    public Color Common;
    public Color Rare;
    public Color Epic;
    public Color Legendary;
    public Image IconDrividerLine;
    
    [Header("Badge")]
    public GameObject Badge;

    void Awake()
    {
        txtTitle.color = TitleColor;
        txtLV.color = TitleColor;
    }
    
    public void SetInfo(ToolTipsInfo toolTipsInfo)
    {
        ClearInfo();
        if (toolTipsInfo.CurToolTipsType == ToolTipsType.Item)
            SetItemInfo(toolTipsInfo);
        else
            SetGemBulletInfo(toolTipsInfo);
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
        IconDrividerLine.color = toolTipsInfo.Rarity switch
        {
            DropedRarity.Common => Common,
            DropedRarity.Rare => Rare,
            DropedRarity.Epic => Epic,
            DropedRarity.Legendary => Legendary,
            _ => Common
        };
        //
        Badge.SetActive(toolTipsInfo.Category == ItemCategory.Persistent);
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
                    desStr = "伤害:"; valueColor = DamageValueColor; break;
                case ToolTipsAttriType.Piercing:
                    desStr = "穿透:"; valueColor = PiercingValueColor; break;
                case ToolTipsAttriType.Resonance:
                    desStr = "共振:"; valueColor = ResonanceValueColor; break;
                case ToolTipsAttriType.Element:
                    desStr = "元素:"; valueColor = DescriptionColor; break;
            }
            
            attriSingleTexts[0].text = desStr;
            attriSingleTexts[0].color = DescriptionColor;
            if (attriInfo.Type != ToolTipsAttriType.Element)
            {
                attriSingleTexts[1].text = attriInfo.OriginValue.ToString();
                attriSingleTexts[1].color = valueColor;
                if (attriInfo.AddedValue != 0)
                {
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
                    ElementalTypes.Fire => "火",
                    ElementalTypes.Ice => "冰",
                    ElementalTypes.Electric => "电",
                    _ => "无"
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