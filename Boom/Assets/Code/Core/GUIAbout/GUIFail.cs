using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GUIFail : MonoBehaviour
{
    [Header("窗口")]
    public GameObject FailGUI;
    public GameObject GameOverGUI;
    public GameObject ContinueButton;
    [Header("小心心")]
    public GameObject HertRoot;
    List<GameObject> Herts;
    
    public void SetHertAni()
    {
        Herts = new List<GameObject>();
        Herts.AddRange(HertRoot.transform.Cast<Transform>().Select(child => child.gameObject));
        
        if (PlayerManager.Instance._PlayerData.HP == 0)//如果血量为0,不显示继续了。播完动画直接显示gameover
            ContinueButton.SetActive(false);
        
        //Should HP
        int shouldHP = PlayerManager.Instance._PlayerData.HP + 1;
        int maxHP = PlayerManager.Instance._PlayerData.MaxHP;
        
        for (int i = maxHP - 1; i >= 0; i--)
        {
            if (i == shouldHP - 1) break;
            Herts[i].SetActive(false);
        }
        
        int needSubIndex = shouldHP - 1;
        CuteDisappear(Herts[needSubIndex].GetComponent<Image>());
        
        if(PlayerManager.Instance._PlayerData.HP == 0)
            StartCoroutine(ShowGameOver());
    }
    
    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        FailGUI.SetActive(false);
        GameOverGUI.SetActive(true);
    }
    
    void CuteDisappear(Image heart)
    {
        Sequence seq = DOTween.Sequence();
        // 先抖一下
        seq.Append(heart.transform.DOShakePosition(1f, new Vector3(80,30,0),30,20).SetEase(Ease.OutElastic));
        seq.Join(heart.transform.DOShakeScale(1f, new Vector3(0.15f,0.05f,0),10,50).SetEase(Ease.OutElastic));
        // 再变小同时往上飘
        seq.Append(heart.transform.DOScale(0.3f, 0.5f));
        seq.Join(heart.transform.DOMoveY(heart.transform.position.y + 2, 0.5f).SetRelative());
        seq.Join(heart.DOFade(0, 0.5f));// 渐隐
        seq.OnComplete(() => { heart.gameObject.SetActive(false); }); // 动画结束后隐藏
    }
}
