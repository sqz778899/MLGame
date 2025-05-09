using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementZoneRoot:MonoBehaviour
{
    public GameObject ElementZoneIconGO;
    public Vector3 StartPos;
    public float IconSpacing = 100f;
    
    List<ElementZoneIcon> activeIcons = new();
    List<ElementZoneData> activeZones => GM.Root.BattleMgr.elementZoneMgr.ActiveZones;
    ElementZoneManager mgr => GM.Root.BattleMgr.elementZoneMgr;

    public void UpdateZoneIcon()
    {
        // 保证图标数量足够
        while (activeIcons.Count < activeZones.Count)
        {
            GameObject go = Instantiate(ElementZoneIconGO, transform);
            go.SetActive(false);
            ElementZoneIcon iconComp = go.GetComponent<ElementZoneIcon>();
            activeIcons.Add(iconComp);
        }
        
        // 更新数据 & 显示
        for (int i = 0; i < activeZones.Count; i++)
        {
            ElementZoneData zone = activeZones[i];
            ElementZoneIcon icon = activeIcons[i];

            icon.gameObject.SetActive(true);
            icon.transform.localPosition = StartPos - new Vector3(IconSpacing * i, 0, 0);
            icon.InitData(zone.ElementalType);
            icon.AddValue(zone.ElementalInfusionValue);
            icon.AnimateEnter(); // 添加动画
        }

        // 多余的隐藏
        for (int i = activeZones.Count; i < activeIcons.Count; i++)
            activeIcons[i].gameObject.SetActive(false);
    }
    
    void HandleReactionVisual(ElementReactionType reaction, int consumeCount)
    {
        // 1. 播放图标淡出动画
        StartCoroutine(DoAnimateReactionIcons(consumeCount));

        // 2. 显示反应名称飘字
        string reactionName = reaction.ToString(); // 或做个本地化字典
        Vector3 pos = activeIcons.Count > 0 ? activeIcons[0].transform.position : transform.position;
        FloatingTextFactory.CreateWorldText(reactionName,pos,
            FloatingTextType.MapHint, Color.yellow);
    }

    IEnumerator DoAnimateReactionIcons(int count)
    {
        count = Mathf.Min(count, activeIcons.Count);
        for (int i = 0; i < count; i++)
        {
            yield return activeIcons[i].AnimateReactAndFade();
        }
    }
    
    #region 数据初始化，事件的注册与注销
    public void InitData()
    {
        mgr.OnZoneChanged -= UpdateZoneIcon;
        mgr.OnZoneChanged += UpdateZoneIcon;

        mgr.OnReaction -= HandleReactionVisual;
        mgr.OnReaction += HandleReactionVisual;
    }
    void OnDestroy()
    {
        mgr.OnZoneChanged -= UpdateZoneIcon;
        mgr.OnReaction -= HandleReactionVisual;
    }
    #endregion
}