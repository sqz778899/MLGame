using System.Collections;
using UnityEngine;

public class CoinsPileNode: MapNodeBase
{
    EffectManager effectManager;
    EffectManager _effectManager
    {
        get
        {
            if (effectManager==null)
                effectManager = UIManager.Instance.EffectRoot.GetComponent<EffectManager>();
            return effectManager;
        }
    }
    
    public int CoinsNum = 20;
    
    public void GetCoins()
    {
        _effectManager.CreatEffect("CoinsPile",transform.position);
        DestroyImmediate(gameObject);
        MainRoleManager.Instance.Coins += CoinsNum;
    }
}