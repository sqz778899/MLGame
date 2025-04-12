using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("血条资源选择")] 
    public GameObject Bar01;
    public GameObject Bar02;
    public GameObject Bar03;
    public GameObject Bar04;
    public GameObject Bar05;
    
    [Header("血条类型")]
    public HealthBarType CurBarType;
    public Enemy CurEnemy;
    public ShieldMono CurShield;
    
    [Header("其他属性")]
    public float Speed = 1f;
    public Transform BloodBar_Mid;
    public Transform BloodBar_Blood;
    public TextMeshPro Text;
    
    float Threshold;
    Func<int> GetCurHP;
    Func<int> GetMaxHP;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Threshold = Speed * 0.001f;
        Text.text = string.Format("{0} / {1}", GetCurHP(), GetMaxHP());
        SelBar();
    }

    public void InitHealthBar(Enemy _enemy)
    {
        CurBarType = HealthBarType.Enemy;
        CurEnemy = _enemy;
        SelBar();
    }
    
    public void InitHealthBar(Func<int> getCurHP, Func<int> getMaxHP)
    {
        GetCurHP = getCurHP;
        GetMaxHP = getMaxHP;

        SelectBarByHP();

        Refresh(); // 初始化时立即刷新一次
    }
    
    public void InitHealthBar(ShieldMono _shield)
    {
        CurBarType = HealthBarType.Shield;
        CurShield = _shield;
        SelBar();
    }

    void Update()
    {   
        if (GetCurHP == null || GetMaxHP == null) return;
        
        float ratio = Mathf.Max(0f, (float)GetCurHP() / GetMaxHP());
        BloodBar_Blood.localScale = new Vector3(ratio, 1f, 1f);
        Text.text = $"{GetCurHP()} / {GetMaxHP()}";
        
        if (BloodBar_Mid.localScale.x > BloodBar_Blood.localScale.x)
        {
            float tmp = BloodBar_Mid.localScale.x;
            tmp -= Threshold;
            BloodBar_Mid.localScale = new Vector3(tmp, 1, 1);
        }
        else
            BloodBar_Mid.localScale = BloodBar_Blood.localScale;
    }

    #region 一些私有方法
    void SelectBarByHP()
    {
        int maxHP = GetMaxHP?.Invoke() ?? 1;

        Dictionary<int, GameObject> barMapping = new()
        {
            { 30, Bar01 },
            { 20, Bar02 },
            { 15, Bar03 },
            { 9, Bar04 },
            { 0, Bar05 }
        };

        GameObject barRoot = null;
        foreach (var kv in barMapping.OrderByDescending(kv => kv.Key))
        {
            if (maxHP >= kv.Key)
            {
                barRoot = kv.Value;
                break;
            }
        }

        if (barRoot != null)
        {
            barRoot.SetActive(true);
            BloodBar_Mid = barRoot.transform.GetChild(0);
            BloodBar_Blood = barRoot.transform.GetChild(1);
            Text = barRoot.GetComponentInChildren<TextMeshPro>();
        }
    }
    
    public void Refresh()
    {
        if (GetCurHP == null || GetMaxHP == null) return;

        float ratio = Mathf.Max(0f, (float)GetCurHP() / GetMaxHP());
        BloodBar_Blood.localScale = new Vector3(ratio, 1f, 1f);
        Text.text = $"{GetCurHP()} / {GetMaxHP()}";
    }
    void SelBar()
    {
        int maxHP = GetMaxHP();
        // 使用 Dictionary 映射 maxHP 区间到血条预设
        Dictionary<int, GameObject> barMapping = new Dictionary<int, GameObject>()
        {
            { 30, Bar01 },
            { 20, Bar02 },
            { 15, Bar03 },
            { 9, Bar04 },
            { 0, Bar05 }
        };
    
        // 查找适合的血条类型
        GameObject barRoot = null;
        foreach (KeyValuePair<int,GameObject> entry in 
                 barMapping.OrderByDescending(kv => kv.Key))
        {
            if (maxHP >= entry.Key)
            {
                barRoot = entry.Value;
                break;
            }
        }
        
        barRoot.SetActive(true);
        BloodBar_Mid = barRoot.transform.GetChild(0);
        BloodBar_Blood = barRoot.transform.GetChild(1);
        Text = barRoot.GetComponentInChildren<TextMeshPro>();
    }
    #endregion
}
