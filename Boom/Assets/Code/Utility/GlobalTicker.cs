using UnityEngine;
using System;

public class GlobalTicker:MonoBehaviour
{
    public event Action OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
