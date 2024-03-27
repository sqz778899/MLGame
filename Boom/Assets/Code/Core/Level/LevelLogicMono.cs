using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogicMono : MonoBehaviour
{
    public float delay = 1.0f;
    List<BulletDataJson> BulletDesignJsons;
    GameObject GroupBullet;
    
    public void CheckForKeyPress(Vector3 pos)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Fire(pos);
    }
    public void Fire(Vector3 pos)
    {
        StartCoroutine(FireWithDelay(pos, delay));
    }
    
    public IEnumerator FireWithDelay(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);  // 等待delay秒，然后开始发射子弹

        Debug.Log("fire");
    
        List<BulletData> bulletDatas = CharacterManager.Instance.Bullets;
        BulletDesignJsons = CharacterManager.Instance.BulletDesignJsons;
    
        if (GroupBullet == null)
            GroupBullet = GameObject.Find("GroupBullet");
    
        foreach (BulletData eBuDT in bulletDatas)
        {
            GameObject curBullet = eBuDT.InstanceBullet(BulletDesignJsons,pos);
            if (curBullet != null && GroupBullet != null)
                curBullet.transform.SetParent(GroupBullet.transform);
        
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
    }
}
