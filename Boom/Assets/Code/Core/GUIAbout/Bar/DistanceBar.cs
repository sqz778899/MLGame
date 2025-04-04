using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DistanceBar : MonoBehaviour
{
    public Image NarmalBar;
    public TextMeshProUGUI Text;
    
    public float MaxDistance;
    public float CurDistance;

    BattleLogic _battleLogic;
    float t;
    public virtual void Start()
    {
        MaxDistance = 100f;
        if (_battleLogic == null)
            _battleLogic = BattleManager.Instance.battleLogic;
        
        CurDistance = BattleManager.Instance.battleData.Distance;
        Text.text = string.Format("{0} : {1}", GetRange(CurDistance), CurDistance);
    }

    public virtual void Update()
    {
        CurDistance = BattleManager.Instance.battleData.Distance;
        t = CurDistance / MaxDistance;
        NarmalBar.fillAmount = t;
        Text.text = string.Format("{0} : {1}", GetRange(CurDistance), CurDistance);
        Text.color = Color.Lerp(Color.red,Color.gray, t);
        NarmalBar.color =  Color.Lerp(Color.red,Color.gray, t);
    }

    string GetRange(float curDis)
    {
        float value01 = 70;
        float value02 = 40;
        
        string result = "";
        if (curDis >= value01)
        {
            result = "Far";
        }

        if (curDis < value01 && curDis >= value02)
        {
            result = "Mid";
        }

        if (curDis < value02)
        {
            result = "Near";
        }
        return result;
    }
}
