using System;
using UnityEngine;


//按ESC键关闭当前窗口
public class HotkeyManager : MonoBehaviour
{
    public event Action OnEscapePressed;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEscapePressed?.Invoke();
    }
}