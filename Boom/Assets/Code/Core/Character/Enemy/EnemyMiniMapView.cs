using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class EnemyMiniMapView : MonoBehaviour
{
    [Header("资产")]
    public SkeletonGraphic SkeletonGraphic; // Spine 图
    public TextMeshProUGUI EnemyHPText;
    public GameObject ShieldsNode;
    public GameObject ShieldPrefab;
    
    Dictionary<ShieldData, GameObject> shieldTextMap = new();
    EnemyData _data;

    public void Bind(EnemyData data)
    {
        _data = data;
        // 初始化 Spine 资源
        SkeletonGraphic.skeletonDataAsset = ResManager.instance.
            GetAssetCache<SkeletonDataAsset>(PathConfig.GetEnemySkelentonDataPath(data.ID));
        SkeletonGraphic.Initialize(true);
        AniUtility.PlayIdle(SkeletonGraphic);
        // 注册监听
        _data.OnTakeDamage += RefreshView;
        InitView(); // 初始刷新
    }

    void InitView()
    {
        CreateShieldsUI();//创建护盾的UI
        RefreshView();//刷新数据
    }
    void RefreshView()
    {
        EnemyHPText.text = _data.CurHP.ToString();
        if (_data.IsDead)
            AniUtility.PlayDead01(SkeletonGraphic);

        // 刷新护盾
        foreach (var each in shieldTextMap)
        {
            GameObject shieldObj = each.Value;
            TextMeshProUGUI txt = shieldObj.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = each.Key.CurHP.ToString();
            if (each.Key.CurHP == 0)
                shieldObj.SetActive(false);
        }
    }
    //创建护盾的UI
    void CreateShieldsUI()
    {
        foreach (Transform child in ShieldsNode.transform)
            Destroy(child.gameObject);
        shieldTextMap.Clear();

        for (int i = 0; i < _data.Shields.Count; i++)
        {
            ShieldData shield = _data.Shields[i];
            GameObject ins = Instantiate(ShieldPrefab, ShieldsNode.transform);
            ins.SetActive(true);
            float offsetX = i * -220f;
            ins.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX, 0);
            TextMeshProUGUI txt = ins.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = shield.CurHP.ToString();
            shield.OnTakeDamage += RefreshView;
            shieldTextMap[shield] = ins;
        }
    }
    void OnDestroy()
    {
        if (_data != null)
            _data.OnTakeDamage -= RefreshView;
        foreach (var pair in shieldTextMap)
            pair.Key.OnTakeDamage -= RefreshView;
    }
}
