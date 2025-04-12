using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [Header("Enemy自身资产")]
    public Sprite Portrait; //对话头像
    public SkeletonAnimation Ani;
    public HealthBar HealthBar;
    public Transform HitTextPos;
    public Color HitColor;
    
    [Header("Shields资产")]
    public GameObject ShieldsNode;
    public List<ShieldMono> Shields = new List<ShieldMono>();

    public void InitSpine(EnemyData data)
    {
        Portrait = ResManager.instance.
            GetAssetCache<Sprite>(PathConfig.GetEnemyPortrait(data.ID));
        var asset = ResManager.instance.GetAssetCache<SkeletonDataAsset>(
            PathConfig.GetEnemySkelentonDataPath(data.ID));
        Ani.skeletonDataAsset = asset;
        Ani.Initialize(true);
        InitShields(data.ShieldsHPs);
    }

    public void PlayIdle(bool isFullHP)
    {
        if (isFullHP)
            AniUtility.PlayIdle(Ani);
        else
            AniUtility.PlayIdle(Ani);
    }

    public void PlayDead() => AniUtility.PlayDead01(Ani);

    public void ShowHitText(int damage)
    {
        GameObject txt = ResManager.instance.CreatInstance(PathConfig.TxtHitPB);
        txt.transform.SetParent(HitTextPos,false);
        txt.GetComponent<FloatingDamageText>().AnimateText($"-{damage}", HitColor, 18f);
    }
    
    public void InitShields(List<int> shieldsHPs)
    {
        // 清理旧盾
        foreach (Transform child in ShieldsNode.transform)
            Destroy(child.gameObject);
        Shields.Clear();

        // 创建新盾
        for (int i = 0; i < shieldsHPs.Count; i++)
        {
            GameObject shieldIns = ResManager.instance.CreatInstance(PathConfig.ShieldPB);
            ShieldMono mono = shieldIns.GetComponent<ShieldMono>();
            mono.ShieldIndex = i;
            mono.InitShield(shieldsHPs[i]);
            mono.HitColor = HitColor;
            shieldIns.transform.SetParent(ShieldsNode.transform,false);
            float curStep = i * mono.InsStep;
            shieldIns.transform.localPosition = new Vector3(curStep,0,0);
            Shields.Add(mono);
        }
    }
}
