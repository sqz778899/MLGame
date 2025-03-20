using UnityEngine;
using System;

public class GlobalTicker:MonoBehaviour
{
    public static GlobalTicker Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public event Action OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
