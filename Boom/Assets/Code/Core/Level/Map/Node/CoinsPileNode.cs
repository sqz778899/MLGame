using System.Collections;
using UnityEngine;

public class CoinsPileNode: MapNodeBase
{
    public void ClickIt()
    {
        GameObject coin = ResManager.instance.IntanceAsset(PathConfig.RewardCoinAsset);
    }
    
    public GameObject prefab;

    // 设定每次创建 GameObject 的时间间隔（以秒为单位）
    public float spawnInterval = 1f;

    void Start()
    {
        // 开始 Coroutine
        //StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        // 永远循环
        while (true)
        {
            // 创建一个新的 GameObject
            Instantiate(prefab, transform);

            // 等待一定时间
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}