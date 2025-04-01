using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonanceSlotCol : MonoBehaviour
{
    public List<int> ResonanceSlots;

    private ParticleSystem fx;
    ParticleSystem _fx
    {
        get
        {
            if (fx == null)
                fx = GetComponent<ParticleSystem>();
            return fx;
        }
    }

    //void Start() =>  CloseEffect();

    public void OpenEffect()
    {
        _fx.Play();
        /*StartCoroutine(LoadAndPlayEffect());
        _fx.Play();*/
    }

    /*
    IEnumerator LoadAndPlayEffect() {
        yield return new WaitForFixedUpdate();
        _fx.Play();
    }
    */
    
    public void CloseEffect()
    {
        _fx.Clear();
        _fx.Stop();
    }
}
