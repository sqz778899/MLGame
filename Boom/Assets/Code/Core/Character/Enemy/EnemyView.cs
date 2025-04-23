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
    public List<Shield> Shields = new List<Shield>();

    public void InitSpine(EnemyData data)
    {
        Portrait = ResManager.instance.
            GetAssetCache<Sprite>(PathConfig.GetEnemyPortrait(data.ID));
        var asset = ResManager.instance.GetAssetCache<SkeletonDataAsset>(
            PathConfig.GetEnemySkelentonDataPath(data.ID));
        Ani.skeletonDataAsset = asset;
        Ani.Initialize(true);
        InitShields(data.Shields);
    }

    public void PlayIdle(bool isFullHP)
    {
        if (isFullHP)
            AniUtility.PlayIdle(Ani);
        else
            AniUtility.PlayIdle(Ani);
    }

    public void PlayDead() => AniUtility.PlayDead01(Ani);

    public void ShowHitText(int damage) => FloatingTextFactory.CreateWorldText(
        $"-{damage}",HitTextPos.position+Vector3.up*0.5f,FloatingTextType.Damage,HitColor,15f);
    
    public void InitShields(List<ShieldData> shields)
    {
        // 清理旧盾
        foreach (Transform child in ShieldsNode.transform)
            Destroy(child.gameObject);
        Shields.Clear();

        // 创建新盾
        for (int i = 0; i < shields.Count; i++)
        {
            Shield shieldSC = ShieldFactory.CreateShield(shields[i],ShieldsNode.transform);
            shieldSC.View.HitColor = HitColor;
            float curStep = i * shieldSC.View.InsStep;
            shieldSC.transform.localPosition = new Vector3(curStep,0,0);
            Shields.Add(shieldSC);
        }
    }
}
