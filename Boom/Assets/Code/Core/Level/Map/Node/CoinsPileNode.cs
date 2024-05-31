using System.Collections;
using UnityEngine;

public class CoinsPileNode: MapNodeBase
{
    public int Count = 5;
    // 设定每次创建 GameObject 的时间间隔（以秒为单位）
    public float spawnInterval = 0.1f;
    
    public void ClickIt()
    {
        // 开始 Coroutine
        StartCoroutine(SpawnCoroutine());
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    
    IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < Count; i++)
        {
            float curOffset = Random.Range(-5,5);
            GameObject coin = ResManager.instance.IntanceAsset(PathConfig.RewardCoinAsset);
            coin.transform.SetParent(UIManager.Instance.RewardRoot.transform,false);
            Vector3 curPos = transform.position;
            curPos.x += curOffset;
            curPos.y += curOffset;
            coin.transform.position = curPos;
            yield return new WaitForSeconds(spawnInterval);
        }
        DestroyImmediate(gameObject);
    }
}