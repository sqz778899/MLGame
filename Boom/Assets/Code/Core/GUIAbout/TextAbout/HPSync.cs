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
        for (int i = 0; i < maxHP; i++)
        {
            Herts[i].SetActive(i < curHP);
        }
    }

    void OnDestroy() => PlayerManager.Instance._PlayerData.OnHPChanged -= HPChanged;
}
