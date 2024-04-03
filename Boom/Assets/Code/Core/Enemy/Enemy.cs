using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public int score = 100;
    public EnemyState state;
    public GameObject txtHitNode;
    
    LevelLogicMono _LevelLogic;

    void Start()
    {
        InitState(null);
    }

    public void InitState(EnemyState curState)
    {
        if (curState!=null)
            state = curState;
        else
            state = new EnemyState();

        _LevelLogic = GameObject.Find("LevelLogic").GetComponent<LevelLogicMono>();
    }

    public void TakeDamage(int damage)
    {
        GameObject txtHitIns = Instantiate(ResManager.instance
            .GetAssetCache<GameObject>(PathConfig.TxtHitPB),txtHitNode.transform);
        txtHitIns.GetComponent<TextMeshPro>().text = "-" + damage;
        Animation curAni = txtHitIns.GetComponent<Animation>();
        float curAniTime = curAni.clip.length;
        curAni.Play();
        // 如果血量为0或更少，销毁这个敌人
        if (health <= 0)
        {
            StartCoroutine(Wait(curAniTime));
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        DestroySelf();
    }
    void DestroySelf()
    {
        CharacterManager.Instance.Score += score;
        CharacterManager.Instance.WinOrFailState = WinOrFail.Win;
        _LevelLogic.isBeginCalculation = true; //通知进行结算
        Destroy(gameObject);
    }
}
