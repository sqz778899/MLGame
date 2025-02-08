using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("血条类型")]
    public HealthBarType CurBarType;
    public Enemy CurEnemy;
    public ShieldMono CurShield;
    
    [Header("其他属性")]
    public float Speed = 1f;
    public Transform BloodBar_Blood;
    public Transform BloodBar_Mid;
    public TextMeshPro Text;
    
    int MaxHP;
    float Threshold;
    
    void Start()
    {
        Threshold = Speed * 0.001f;
        Text.text = string.Format("{0} / {1}", GetCurHP(), GetMaxHP());
    }

    public void InitHealthBar(Enemy _enemy)
    {
        CurBarType = HealthBarType.Enemy;
        CurEnemy = _enemy;
    }
    
    public void InitHealthBar(ShieldMono _shield)
    {
        CurBarType = HealthBarType.Shield;
        CurShield = _shield;
    }

    int GetCurHP()
    {
        int CurHP = 0;
        switch (CurBarType)
        {
            case HealthBarType.Enemy:
                CurHP = CurEnemy.CurHP;
                break;
            case HealthBarType.Shield:
                CurHP = CurShield.CurHP;
                break;
        }
        return CurHP;
    }
    
    int GetMaxHP()
    {
        int CurHP = 0;
        switch (CurBarType)
        {
            case HealthBarType.Enemy:
                CurHP = CurEnemy.MaxHP;
                break;
            case HealthBarType.Shield:
                CurHP = CurShield.MaxHP;
                break;
        }
        return CurHP;
    }
    
    void Update()
    {
        //BloodBar_Blood.localScale;
        float CurRatio = Mathf.Max(0,(float)GetCurHP() / GetMaxHP());
        BloodBar_Blood.localScale = new Vector3(CurRatio, 1, 1);
        Text.text = string.Format("{0} / {1}", GetCurHP(), GetMaxHP());
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
