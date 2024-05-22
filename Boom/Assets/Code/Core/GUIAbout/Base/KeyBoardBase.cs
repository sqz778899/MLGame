using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyBoardBase : MonoBehaviour
{
    internal List<SetStr> Alltxt;

    internal virtual void Start()
    {
        GetAllTextInScene();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UIManager.Instance.G_Setting.GetComponent<SettingMono>().CloseWindow();
            UIManager.Instance.G_Help.GetComponent<HelpMono>().CloseWindow();
        }
    }

    internal void GetAllTextInScene()
    {
        if (Alltxt == null)
            Alltxt = new List<SetStr>();
        
        // 获取当前活跃的场景
        Scene activeScene = SceneManager.GetActiveScene();
        List<GameObject> rootObjects = new List<GameObject>();
        activeScene.GetRootGameObjects(rootObjects);

        // 遍历rootObjects并打印每个根对象的名称
        foreach (GameObject go in rootObjects)
        {
            SetStr[] curSC = go.GetComponentsInChildren<SetStr>();
            foreach (var each in curSC)
            {
                if (!Alltxt.Contains(each))
                    Alltxt.Add(each);
            }
        }
    }

    public void SyncMultiLa()
    {
        foreach (var each in Alltxt)
            each.SyncTextData();
    }
}
