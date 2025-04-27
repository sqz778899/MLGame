using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectManager : MonoBehaviour
{
    public GameObject Root;
    [Header("目标GO")] 
    public GameObject CoinsTarget;
    public GameObject RoomKeysTarget;
    public GameObject BagTarget;
    [Header("Curves")]
    public AnimationCurve GamblingCurve;
    [Header("公共参数")] 
    public EParameter eParamKey;

    #region 总接口
    public void CreatEffect(EParameter eParam,GameObject Ins = null,Action onFinish = null)
    {
        string prefabPath = "";
        Vector3 targetpos;
        switch (eParam.CurEffectType)
        {
            case EffectType.CoinsPile:
                prefabPath = PathConfig.AwardCoin;
                targetpos = CoinsTarget.transform.position;
                break;
            case EffectType.RoomKeys:
                eParamKey.StartPos = eParam.StartPos;
                eParam = eParamKey;
                prefabPath = PathConfig.AwardRoomKey;
                targetpos = RoomKeysTarget.transform.position;
                break;
            case EffectType.Shop:
                prefabPath = PathConfig.AwardCoin;
                targetpos = BagTarget.transform.position;
                break;
            case EffectType.FlipReward:  //新加翻找掉落走这条
                eParam.CustomFlyCurve = GamblingCurve;
                prefabPath = eParam.SpecialEffectPath;
                targetpos = BagTarget.transform.position;
                break;
            default:
                prefabPath = PathConfig.AwardCoin;
                targetpos = CoinsTarget.transform.position;
                break;
        }
        
        int total = eParam.InsNum;
        int finished = 0;
        
        for (int i = 0; i < eParam.InsNum; i++)
        {
            GameObject instance = null;
            if (Ins != null)
                instance = Ins;
            else
                instance = ResManager.instance.CreatInstance(prefabPath);

          
            PlaySingleAward(instance, eParam, targetpos, () =>
            {
                finished++;
                if (finished >= total)
                {
                    onFinish?.Invoke(); //所有光球飞完之后，再统一弹 Banner
                }
            });
        }
    }
    
    void PlaySingleAward(GameObject obj, EParameter eParam, Vector3 targetPos, Action onFinish = null)
    {
        if (obj.GetComponent<RectTransform>())
            obj.transform.SetParent(Root.transform, false);
        else
            obj.transform.SetParent(Root.transform, true);
        eParam.StartPos.z = targetPos.z; // 保持z轴一致
        obj.transform.position = eParam.StartPos;

        Vector3 randomExpandOffset = Random.insideUnitSphere * eParam.Radius;
        Vector3 expandPos = eParam.StartPos + randomExpandOffset;

        float spawnDuration = Random.Range(eParam.SpawntimeRange.x, eParam.SpawntimeRange.y);

        float distance = Vector3.Distance(expandPos, targetPos);
        float flyTime = eParam.FlyTimeBase + distance * eParam.FlyTimePerUnitDistance;
        flyTime = Mathf.Clamp(flyTime, eParam.FlyTimeBase, eParam.FlyTimeClampMax);

        Vector3 controlPoint = Vector3.Lerp(expandPos, targetPos, 0.5f)
                               + Random.insideUnitSphere * Random.Range(eParam.FlyRangeOffset.x, eParam.FlyRangeOffset.y);

        Sequence seq = DOTween.Sequence();
        seq.Append(obj.transform.DOMove(expandPos, spawnDuration).SetEase(Ease.OutSine));
        seq.Append(obj.transform.DOBezier(expandPos, controlPoint, targetPos, flyTime, () =>
        {
            FadeOutAndDestroy(obj);

            if (eParam.CurEffectType == EffectType.FlipReward && BagTarget != null)
            {
                BagTarget.GetComponent<BagIconAnimator>()?.PlayHeartbeatBump();
            }
            onFinish?.Invoke();
        }).SetEase(eParam.CustomFlyCurve));
        
        seq.SetUpdate(true);
    }
    #endregion
    
    void FadeOutAndDestroy(GameObject obj)
    {
        bool handled = false;

        // 1. 处理粒子系统
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            float delay = ps.main.startLifetime.constantMax;
            DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(() => Destroy(obj));
            handled = true;
        }

        // 2. 处理SpriteRenderer（渐隐）
        var sr = obj.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            DOTween.To(() => sr.color, c => sr.color = c, new Color(sr.color.r, sr.color.g, sr.color.b, 0), 0.4f)
                .OnComplete(() => Destroy(obj));
            handled = true;
        }

        // 3. 处理TrailRenderer（自然消散）
        var trail = obj.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false; // 关闭发射新轨迹
            float trailDuration = trail.time;
            DOTween.Sequence()
                .AppendInterval(trailDuration)
                .AppendCallback(() => Destroy(obj));
            handled = true;
        }

        // 4. 如果啥都没找到，直接销毁
        if (!handled)
        {
            Destroy(obj);
        }
    }
}

public enum EffectType
{
    CoinsPile = 0,
    RoomKeys = 1,
    Shop = 2,
    FlipReward = 3
}
[Serializable]
public class EParameter
{
    public EffectType CurEffectType;
    public Vector3 StartPos;        //开始位置
    public int InsNum;              //实例的数量
    public Vector2 SpawntimeRange;  //诞生动画时长
    public float Radius;            // 诞生球半径
    public Vector2 FlyRangeOffset;  // 贝塞尔控制点偏移
    
    public float FlyTimeBase = 0.5f;
    public float FlyTimePerUnitDistance = 0.05f;
    public float FlyTimeClampMax = 1.2f;
    public string SpecialEffectPath = "";
    
    public AnimationCurve CustomFlyCurve; //加一个飞行动画曲线

    public EParameter()
    {
        CurEffectType = EffectType.CoinsPile;
        StartPos = Vector3.zero;
        InsNum = 1;
        SpawntimeRange = new Vector2(0.3f, 0.4f);
        Radius = 1;
        FlyRangeOffset = new Vector2(-1, 1);
        CustomFlyCurve = AnimationCurve.EaseInOut(0,0,1,1); // 默认曲线
    }
}