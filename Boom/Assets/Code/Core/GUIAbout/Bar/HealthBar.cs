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
    
    [Header("其他属性")]
    public float Speed = 1f;
    public Transform BloodBar_Mid;
    public Transform BloodBar_Blood;
    public TextMeshPro Text;
    
    float Threshold;
    Func<int> GetCurHP;
    Func<int> GetMaxHP;
    
    public void InitHealthBar(Func<int> getCurHP, Func<int> getMaxHP)
    {
        GetCurHP = getCurHP;
        GetMaxHP = getMaxHP;

        SelectBarByHP();

        Refresh(); // 初始化时立即刷新一次
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
            kv.Value.SetActive(false); //先把血条全关了
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
    #endregion
}
