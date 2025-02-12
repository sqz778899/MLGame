using System.Collections;
using UnityEngine;

public class CoinsPileNode: MapNodeBase
{
    public int CoinsNum = 20;
    
    public void GetCoins()
    {
        EPara.InsNum = CoinsNum;
        EPara.StartPos = transform.position;
        MEffectManager.CreatEffect(EPara);
        DestroyImmediate(gameObject);
        MainRoleManager.Instance.Coins += CoinsNum;
    }
}