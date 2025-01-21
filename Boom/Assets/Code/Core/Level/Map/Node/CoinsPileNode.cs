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
    
    internal override void OnMouseEnter()
    {
        outLineMat.color = OutlineColor;
        spriteRenderer.material = outLineMat;// 高亮勾边
        if (Input.GetMouseButtonDown(0))
            transform.localScale = defaultScale * 0.8f;
        if (Input.GetMouseButtonUp(0))
            transform.localScale = defaultScale;
    }

    internal override void OnMouseExit()
    {
        spriteRenderer.material = defaultMat;// 还原
    }
    
    public void GetCoins()
    {
        _effectManager.CreatEffect("CoinsPile",transform.position);
        DestroyImmediate(gameObject);
        MainRoleManager.Instance.Coins += CoinsNum;
    }
}