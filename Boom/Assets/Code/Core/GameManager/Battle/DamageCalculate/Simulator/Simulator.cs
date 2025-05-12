using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    [Header("依赖资产")]
    public TextMeshProUGUI winCountText;
    GameObject PreERIconGO => GM.Root.BattleMgr._MapManager.PreERIconGO;

    GameObject _bulletInFightRoot => GM.Root.BattleMgr._MapManager.MapBuleltRoot;
    List<GameObject> currentIcons = new();
    List<List<BulletInner>> currentBulletGroups = new();

    public void InitData()
    {
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged -= Simulate;
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged += Simulate;
        GM.Root.InventoryMgr.OnBulleInnerChanged -= Simulate;
        GM.Root.InventoryMgr.OnBulleInnerChanged += Simulate;
        Simulate();
    }
    void OnDestroy() => StopSimulate();

    public void StopSimulate()
    {
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged -= Simulate;
        GM.Root.InventoryMgr.OnBulleInnerChanged -= Simulate;
        ClearIcons(); // 清理旧图标
    }

    public void Simulate()
    {
        float s = BattleSimulator.SimulateBattle();
        winCountText.text = $"胜率：{s * 100}%";
        
        List<ElementReactionInfo> reactions = GM.Root.BattleMgr.elementZoneMgr.PredictReactions();
        ClearIcons();
        foreach (var info in reactions)
        {
            if (info.Reaction == ElementReactionType.Non || info.ReactionBullets.Count == 0)
                continue;

            Vector3 center = ComputeCenter(info.ReactionBullets);
            Vector3 worldPos = center + Vector3.up * 1.5f;

            GameObject icon = Instantiate(PreERIconGO, worldPos, Quaternion.identity, _bulletInFightRoot.transform);
            icon.SetActive(true);
            icon.GetComponent<ERIconView>().InitData(info.Reaction); // 如有图标绑定逻辑

            currentIcons.Add(icon);
            currentBulletGroups.Add(info.ReactionBullets);
        }  
    }

    public void Update()
    {
        // 每帧同步 icon 位置
        for (int i = 0; i < currentIcons.Count; i++)
        {
            if (currentIcons[i] == null || currentBulletGroups[i] == null) continue;

            Vector3 center = ComputeCenter(currentBulletGroups[i]);
            currentIcons[i].transform.position = center + Vector3.up * 1.8f;
        }
    }

 
    void ClearIcons()
    {
        foreach (var icon in currentIcons)
        {
            if (icon != null) Destroy(icon);
        }
        currentIcons.Clear();
        currentBulletGroups.Clear();
    }
    
    Vector3 ComputeCenter(List<BulletInner> group)
    {
        Vector3 sum = Vector3.zero;
        foreach (var b in group)
        {
            if (b != null)
                sum += b.transform.position;
        }
        return sum / group.Count;
    }
}


