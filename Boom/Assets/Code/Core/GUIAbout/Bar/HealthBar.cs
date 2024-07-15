using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float Speed = 1f;
    public Enemy CurEnemy;
    public Transform BloodBar_Blood;
    public Transform BloodBar_Mid;
    public TextMeshPro Text;

    float Threshold;
    
    void Start()
    {
        Threshold = Speed * 0.001f;
        Text.text = string.Format("{0} / {1}", CurEnemy.CurHP, CurEnemy.MaxHP);
    }
    
    void Update()
    {
        //BloodBar_Blood.localScale;
        float CurRatio = Mathf.Max(0,(float)CurEnemy.CurHP / CurEnemy.MaxHP);
        BloodBar_Blood.localScale = new Vector3(CurRatio, 1, 1);
        Text.text = string.Format("{0} / {1}", CurEnemy.CurHP, CurEnemy.MaxHP);
        if (BloodBar_Mid.localScale.x > BloodBar_Blood.localScale.x)
        {
            float tmp = BloodBar_Mid.localScale.x;
            tmp -= Threshold;
            BloodBar_Mid.localScale = new Vector3(tmp, 1, 1);
        }
        else
        {
            BloodBar_Mid.localScale = BloodBar_Blood.localScale;
        }
    }
}
