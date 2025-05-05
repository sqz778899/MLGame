using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HPSync : MonoBehaviour
{
    List<GameObject> Herts;
    PlayerData _playerData => GM.Root.PlayerMgr._PlayerData;
    void Start()
    {
        _playerData.OnHPChanged += HPChanged;
        Herts = new List<GameObject>();
        Herts.AddRange(transform.Cast<Transform>().Select(child => child.gameObject));
        HPChanged();
    }

    void HPChanged()
    {
        int maxHP = _playerData.MaxHP;
        int curHP = _playerData.HP;
        for (int i = 0; i < maxHP; i++)
        {
            Herts[i].SetActive(i < curHP);
        }
    }

    void OnDestroy() => _playerData.OnHPChanged -= HPChanged;
}
