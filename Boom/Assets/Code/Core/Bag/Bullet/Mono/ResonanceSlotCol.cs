using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceSlotCol : MonoBehaviour
{
    public List<int> ResonanceSlots;
    ParticleSystem _fx;

    void Start()
    {
        _fx = GetComponent<ParticleSystem>();
        _fx.Stop();
    }

    public void OpenEffect()
    {
        _fx.Play();
    }
    
    public void CloseEffect()
    {
        _fx.Clear();
        _fx.Stop();
    }
}
