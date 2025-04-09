using UnityEngine;
using System;

public class GlobalTicker:MonoBehaviour
{
    public static GlobalTicker Instance { get; private set; }
    
    void Awake() => Instance = this;

    public event Action OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
