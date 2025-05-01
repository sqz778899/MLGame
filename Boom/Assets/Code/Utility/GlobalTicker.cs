using UnityEngine;
using System;
using System.Collections.Generic;

public class GlobalTicker:MonoBehaviour
{
    public event Action OnUpdate;

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
