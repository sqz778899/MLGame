using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffMono : MonoBehaviour
{
    public int ID;
    public Image imgBuff;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;

    public void InitBuffData()
    {
        BuffDataJson curBuffData = BuffMannager.Instance.GetBuffDataByID(ID);
        imgBuff.sprite = BuffMannager.Instance.GetBuffImageByID(ID);
        txtTitle.text = curBuffData.name;
        txtDescription.text = GetDescriptionStr(curBuffData);
    }

    public string GetDescriptionStr(BuffDataJson buffData)
    {
        string result = "";
        CommonAttribute comAttri = buffData.comAttributes;
        SpeAttribute speAttri = buffData.speAttributes;
        //CommonAttribute
        if (comAttri.damage != 0)
        {
            if (comAttri.damage > 0)
                result = string.Format("Damage: +{0}\n", comAttri.damage);
            else
                result = string.Format("Damage: -{0}\n", comAttri.damage);
        }

        if (comAttri.elementalType != 1)
        {
            string curEle = ((ElementalTypes)comAttri.elementalType).ToString();
            int curValue = comAttri.elementalValue;
            if (curValue > 0)
                result = result + string.Format("{0} : +{1}\n",curEle, curValue);
            else
                result = result + string.Format("{0} : -{1}\n",curEle, curValue);
        }

        if (comAttri.Penetration != 0)
        {
            if (comAttri.Penetration > 0)
                result = result + string.Format("Penetration : +{0}\n", comAttri.Penetration);
            else
                result = result + string.Format("Penetration : -{0}\n", comAttri.Penetration);
        }
        
        //SpeAttribute
        if (speAttri.interest != 0)
        {
            if (speAttri.interest > 0)
                result = result + string.Format("Interest : +{0}\n", speAttri.interest);
            else
                result = result + string.Format("Interest : -{0}\n", speAttri.interest);
        }
        
        if (speAttri.standbyAdd != 0)
        {
            if (speAttri.standbyAdd > 0)
                result = result + string.Format("standby : +{0}\n", speAttri.standbyAdd);
            else
                result = result + string.Format("standby : -{0}\n", speAttri.standbyAdd);
        }
        
        return result;
    }
}
