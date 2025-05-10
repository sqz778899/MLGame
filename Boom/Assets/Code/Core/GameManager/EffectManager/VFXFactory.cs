using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VFXFactory
{
    static Dictionary<string, Queue<GameObject>> fxPool = new();
    static Transform _poolRoot;
    static Transform PoolRoot
    {
        get
        {
            if (_poolRoot == null)
            {
                GameObject go = new GameObject("PooledFxRoot");
                GameObject.DontDestroyOnLoad(go);
                _poolRoot = go.transform;
            }
            return _poolRoot;
        }
    }
    
    public static void PlayFx(string fxPath, Vector3 pos, Transform parent = null, float? customLifetime = null)
    {
        GameObject fx = GetFxInstance(fxPath);
        fx.transform.SetParent(parent ?? PoolRoot, false);
        fx.transform.position = pos;
        fx.SetActive(true);

        float lifetime = customLifetime ?? CalcLifetime(fx);
        GM.Root.StartCoroutine(AutoRecycle(fx, fxPath, lifetime));
    }
    
    
    #region 不关心的私有方法 && 对象池管理相关
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
    
    //从对象池中拿取特效
    static GameObject GetFxInstance(string fxPath)
    {
        if (!fxPool.TryGetValue(fxPath, out var queue) || queue.Count == 0)
        {
            GameObject prefab = ResManager.instance.CreatInstance(fxPath);
            if (prefab == null) return null;
            prefab.SetActive(false);
            return prefab;
        }
        GameObject fx = queue.Dequeue();
        return fx;
    }
    
    //回收特效到对象池
    static IEnumerator AutoRecycle(GameObject fx, string fxPath, float delay)
    {
        yield return new WaitForSeconds(delay);
        RecycleFx(fxPath, fx);
    }
    
    static void RecycleFx(string fxPath, GameObject fx)
    {
        fx.SetActive(false);
        fx.transform.SetParent(PoolRoot);

        if (!fxPool.TryGetValue(fxPath, out var queue))
            fxPool[fxPath] = queue = new Queue<GameObject>();

        queue.Enqueue(fx);
    }
    #endregion
}