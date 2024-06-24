using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DistanceBar : MonoBehaviour
{
    public Image NarmalBar;
    public TextMeshProUGUI Text;
    
    public float MaxDistance;
    public float CurDistance;

    float t;
    public virtual void Start()
    {
        MaxDistance = 100f;
        CurDistance = UIManager.Instance.LevelLogic.Distance;
        Text.text = string.Format("{0} : {1}", GetRange(CurDistance), CurDistance);
    }

    public virtual void Update()
    {
        CurDistance = UIManager.Instance.LevelLogic.Distance;
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
