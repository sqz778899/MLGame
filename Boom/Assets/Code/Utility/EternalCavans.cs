using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EternalCavans : MonoBehaviour
{
    [Header("永不凋零的GUI资产")]
    [Header("Bag")]
    public GameObject BagRoot;
    public GameObject BagRootMini;
    [Header("GUIMap")]
    public GameObject GUIMap;
    
    #region 单例的加载卸载
    public Canvas MCanvas;
    public static EternalCavans Instance { get; private set; }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCavans;
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoadedCavans;
        }
        else
            Destroy(gameObject);
    }

    void OnSceneLoadedCavans(Scene scene, LoadSceneMode mode)
    {
        MCanvas.worldCamera = Camera.main;
    }
    #endregion
}
