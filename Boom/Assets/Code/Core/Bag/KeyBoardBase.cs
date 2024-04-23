using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardBase : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UIManager.Instance.GroupSetting.GetComponent<SettingMono>().CloseSetting();
        }
    }
}
