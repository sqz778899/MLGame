using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public EnemyState EState;
    public DamageState DState;
    //表现相关
    public Animation DeadAni;
    public GameObject txtHitNode;
    
    //Award...........
    public Award award;
    
    LevelLogicMono _LevelLogic;

    void Start()
    {
        InitState(null);
    }

    public void InitState(DamageState curState)
    {
        if (curState!=null)
            DState = curState;
        else
            DState = new DamageState();

        _LevelLogic = GameObject.Find("LevelLogic").GetComponent<LevelLogicMono>();
    }

    public void TakeDamage(int damage)
    {
        //伤害跳字
        HitText(damage);
        // 如果血量为0或更少，销毁这个敌人
        if (health <= 0)
        {
            float waitTime = PlayDeadAni() + 1;
            StartCoroutine(Wait(waitTime));
        }
    }

    #region Animation
    void HitText(int damage)
    {
        GameObject txtHitIns = Instantiate(ResManager.instance
            .GetAssetCache<GameObject>(PathConfig.TxtHitPB),txtHitNode.transform);
        txtHitIns.GetComponent<TextMeshPro>().text = "-" + damage;
        Animation curAni = txtHitIns.GetComponent<Animation>();
        curAni.Play();
    }
    
    //DeadAnimation
    float PlayDeadAni()
    {
        float time = DeadAni.clip.length;
        DeadAni.Play();
        return time;
    }
    #endregion
    

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        DestroySelf();
    }
    void DestroySelf()
    {
        MainRoleManager.Instance.Score += award.score;
        EState = EnemyState.dead;
        _LevelLogic.isBeginCalculation = true; //通知进行结算
        Destroy(gameObject);
    }
}
