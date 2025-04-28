using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RewardBannerManager : MonoBehaviour
{
    public RectTransform bannerRoot;
    public GameObject bannerPrefab;

    public float verticalSpacing = 110f;

    private readonly List<RectTransform> activeBanners = new();

    public static RewardBannerManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    
    public void ShowReward(DropedObjEntry drop,int count)
    {
        GameObject ins = Instantiate(bannerPrefab, bannerRoot);
        RectTransform rt = ins.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -verticalSpacing * activeBanners.Count);

        activeBanners.Add(rt);
        RewardBanner banner = ins.GetComponent<RewardBanner>();
        banner.Init(drop, count);

        StartCoroutine(CleanupWhenDone(ins, rt));
    }

    IEnumerator CleanupWhenDone(GameObject obj, RectTransform rt)
    {
        yield return new WaitForSeconds(2.5f);
        activeBanners.Remove(rt);
        Reflow();
    }

    void Reflow()
    {
        for (int i = 0; i < activeBanners.Count; i++)
        {
            activeBanners[i].DOAnchorPosY(-verticalSpacing * i, 0.25f).SetEase(Ease.OutCubic);
        }
    }
}

