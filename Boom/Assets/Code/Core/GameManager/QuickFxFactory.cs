using UnityEngine;

public static class QuickFxFactory
{
    /// <summary>
    /// 快速创建一个特效并自动销毁
    /// </summary>
    public static void PlayFx(string fxPath, Vector3 pos, Transform parant = null,float? customLifetime = null)
    {
        GameObject fx = ResManager.instance.CreatInstance(fxPath);
        if (parant != null)
            fx.transform.SetParent(parant, false);
        fx.transform.position = pos;

        float lifetime = customLifetime ?? CalcLifetime(fx);

        GameObject.Destroy(fx, lifetime);
    }

    #region 不关心的私有方法
    //自动计算特效时长
    static float CalcLifetime(GameObject fx)
    {
        float maxLife = 0f;

        // 粒子系统
        var psArray = fx.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in psArray)
        {
            var main = ps.main;
            maxLife = Mathf.Max(maxLife, main.startLifetime.constantMax);
        }

        // TrailRenderer
        var trailArray = fx.GetComponentsInChildren<TrailRenderer>(true);
        foreach (var trail in trailArray)
        {
            maxLife = Mathf.Max(maxLife, trail.time);
        }

        // 加一点保险时间
        return Mathf.Max(0.1f, maxLife + 0.5f);
    }
    #endregion
}