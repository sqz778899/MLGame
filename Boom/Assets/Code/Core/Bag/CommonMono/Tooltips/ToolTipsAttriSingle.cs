using TMPro;
using UnityEngine;

public class ToolTipsAttriSingle:MonoBehaviour
{
    [Header("依赖资产")]
    [SerializeField]TextMeshProUGUI txtAttriDes;
    [SerializeField]TextMeshProUGUI txtAttriValue;
    [SerializeField]TextMeshProUGUI txtAttriValueAdd;
    [SerializeField]TextMeshProUGUI txtElementNone;
    [SerializeField]GameObject ice;
    [SerializeField]GameObject fire;
    [SerializeField]GameObject thunder;

    public void InitData(ToolTipsAttriSingleInfo attriInfo)
    {
        //Step1 设置通用属性
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
        txtAttriDes.text = desStr;
        txtAttriDes.color = ColorPalette.NormalTextColor;
        txtAttriValue.color = valueColor;
        txtAttriValueAdd.color = valueColor;
        txtElementNone.color = valueColor;
        
        //Step2 如果是属性值，则按照属性值的方式去Set数据
        if (attriInfo.Type == ToolTipsAttriType.Element)
        {
            txtAttriValue.gameObject.SetActive(false);
            txtAttriValueAdd.gameObject.SetActive(false);
            switch (attriInfo.ElementType)
            {
                case ElementalTypes.Fire:fire.SetActive(true); break;
                case ElementalTypes.Ice:ice.SetActive(true); break;
                case ElementalTypes.Thunder:thunder.SetActive(true); break;
                case ElementalTypes.Non:
                    txtElementNone.gameObject.SetActive(true);
                    txtElementNone.text = Loc.Get("battle.none");
                    break;
            }
            return;
        }
        
        //Step3 如果不是属性值，则按照一般属性值的方式去Set数据
        if (attriInfo.Type == ToolTipsAttriType.Critical)//暴击率用百分比呈现
            txtAttriValue.text = $"{attriInfo.OriginValue}%";
        else
            txtAttriValue.text = attriInfo.OriginValue.ToString();
        if (attriInfo.AddedValue != 0)
        {
            if (attriInfo.AddedValue < 0)
                txtAttriValueAdd.text = $"({attriInfo.AddedValue})";
            else
                txtAttriValueAdd.text = $"(+{attriInfo.AddedValue})";
        }
        else
            txtAttriValueAdd.text = "";
    }
}