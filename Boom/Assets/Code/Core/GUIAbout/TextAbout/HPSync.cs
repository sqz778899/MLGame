using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HPSync : MonoBehaviour
{
    List<GameObject> Herts;
    void Start()
    {
        PlayerManager.Instance._PlayerData.OnHPChanged += HPChanged;
        Herts = new List<GameObject>();
        Herts.AddRange(transform.Cast<Transform>().Select(child => child.gameObject));
        HPChanged();
    }

    void HPChanged()
    {
        int maxHP = PlayerManager.Instance._PlayerData.MaxHP;
        int curHP = PlayerManager.Instance._PlayerData.HP;
        for (int i = maxHP - 1; i >= 0; i--)
        {
            if (i == curHP - 1) break;
            Herts[i].SetActive(false);
        }
    }

    void OnDestroy()
    {
        PlayerManager.Instance._PlayerData.OnHPChanged -= HPChanged;
    }
}
