
using System;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    [Header("依赖资产")] 
    [SerializeField] GameObject ArrowLeft;
    [SerializeField] GameObject ArrowRight;
    [SerializeField] GameObject MainSceneRoot;
    [SerializeField] GameObject TowerSeneRoot;

    void Start() => SwichMainScene();
    public void SwichTowerScene()
    {
        ArrowLeft.SetActive(false);
        ArrowRight.SetActive(true);
        MainSceneRoot.SetActive(false);
        TowerSeneRoot.SetActive(true);
    }

    public void SwichMainScene()
    {
        ArrowLeft.SetActive(true);
        ArrowRight.SetActive(false);
        MainSceneRoot.SetActive(true);
        TowerSeneRoot.SetActive(false);
    }
}
