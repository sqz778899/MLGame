using System;
using UnityEngine;

public class HotkeyManager : MonoBehaviour
{
    public event Action OnEscapePressed;
    public event Action OnOpenInventory;
    public event Action OnOpenMap;
    public event Action OnFirePressed;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEscapePressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.B))
            OnOpenInventory?.Invoke();

        if (Input.GetKeyDown(KeyCode.M))
            OnOpenMap?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space))
            OnFirePressed?.Invoke(); // 可选：战斗状态下才监听
    }
}