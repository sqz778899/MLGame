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
    
    [Header("属性")] 
    public GameObject AttriRoot;
    public GameObject OriginAttriGO;

    [Header("背景")] 
    public GameObject BGSmall;
    public GameObject BGMedium;
    public GameObject BGLarge;
    
    [Header("字体颜色")]
    public Color TitleColor;
    public Color DescriptionColor;
    public Color DamageValueColor;
    public Color PiercingValueColor;
    public Color ResonanceValueColor;

    void Awake()
    {
        txtTitle.color = TitleColor;
        txtLV.color = TitleColor;
    }
    
    public void SetInfo(ToolTipsInfo toolTipsInfo)
    {
        int attriCount = toolTipsInfo.AttriInfos.Count;
        //设置背景大小
        if (attriCount <= 3)
            SetBGSmall();
        else if (attriCount <= 5)
            SetBGMedium();
        else
            SetBGLarge();
        
        //加载标题
        txtTitle.text = toolTipsInfo.Name;
        if(toolTipsInfo.Level != 0)
            txtLV.text = $"Lv.{toolTipsInfo.Level}";
        else
            txtLV.text = "";
        
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
                    desStr = "伤害:";
                    valueColor = DamageValueColor;
                    break;
                case ToolTipsAttriType.Piercing:
                    desStr = "穿透:";
                    valueColor = PiercingValueColor;
                    break;
                case ToolTipsAttriType.Resonance:
                    desStr = "共振:";
                    valueColor = ResonanceValueColor;
                    break;
                case ToolTipsAttriType.Element:
                    desStr = "元素:";
                    valueColor = DescriptionColor;
                    break;
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
                string curEleStr = "";
                switch (attriInfo.ElementType)
                {
                    case ElementalTypes.Non:
                        curEleStr = "无";
                        break;
                    case ElementalTypes.Ice:
                        curEleStr = "冰";
                        break;
                    case ElementalTypes.Fire:
                        curEleStr = "火";
                        break;
                    case ElementalTypes.Electric:
                        curEleStr = "电";
                        break;
                }
                attriSingleTexts[1].text = curEleStr;
                attriSingleTexts[1].fontSize -= 5;
                attriSingleTexts[1].color = valueColor;
                attriSingleTexts[2].text = "";
            }
        }
    }

    void SetBGSmall()
    {
        BGSmall.SetActive(true);
        BGMedium.SetActive(false);
        BGLarge.SetActive(false);
    }

    void SetBGMedium()
    {
        BGMedium.SetActive(true);
        BGSmall.SetActive(false);
        BGLarge.SetActive(false);
    }

    void SetBGLarge()
    {
        BGLarge.SetActive(true);
        BGSmall.SetActive(false);
        BGLarge.SetActive(false);
    }

    public void ClearInfo()
    {
        for (int i = AttriRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(AttriRoot.transform.GetChild(i).gameObject);
    }
}