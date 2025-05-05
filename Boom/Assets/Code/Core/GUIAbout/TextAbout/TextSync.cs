using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;

public class TextSync : MonoBehaviour
{
    public float duration = 1f;
    TextMeshProUGUI _txt;
    int _curValue;
    int _targetValue;
    bool isAdding = false;
    
    // 自定义的值和更新类型 (Key 或 Coins)
    public enum ValueType
    {
        RoomKeys = 0,
        Coins = 1,
        Score = 2,
        MagicDust =3,
    }
    public ValueType valueType;

    public void InitData()
    {
        _txt = GetComponent<TextMeshProUGUI>();
        // 根据 `valueType` 初始化
        switch (valueType)
        {
            case ValueType.Coins:
                GM.Root.PlayerMgr._PlayerData.OnCoinsAdd -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnCoinsAdd += CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnCoinsSub -= CommonSub;
                GM.Root.PlayerMgr._PlayerData.OnCoinsSub += CommonSub;
                _curValue = _targetValue = GM.Root.PlayerMgr._PlayerData.Coins;
                break;
            case ValueType.RoomKeys:
                GM.Root.PlayerMgr._PlayerData.OnRoomKeysChanged -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnRoomKeysChanged += CommonChange;
                _curValue = _targetValue = GM.Root.PlayerMgr._PlayerData.RoomKeys;
                break;
            case ValueType.Score:
                GM.Root.PlayerMgr._PlayerData.OnScoreChanged -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnScoreChanged += CommonChange;
                _curValue = _targetValue =  GM.Root.PlayerMgr._PlayerData.Score;
                break;
            case ValueType.MagicDust:
                GM.Root.PlayerMgr._PlayerData.OnMagicDustAdd -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnMagicDustAdd += CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnMagicDustSub -= CommonSub;
                GM.Root.PlayerMgr._PlayerData.OnMagicDustSub += CommonSub;
                _curValue = _targetValue = GM.Root.PlayerMgr._PlayerData.MagicDust;
                break;
        }
        _txt.text = _curValue.ToString();
    }

    #region 不关心的私有方法
    void CommonChange()
    {
        // 更新目标值
        switch (valueType)
        {
            case ValueType.Coins:
                _targetValue = GM.Root.PlayerMgr._PlayerData.Coins;
                break;
            case ValueType.RoomKeys:
                _targetValue = GM.Root.PlayerMgr._PlayerData.RoomKeys;
                break;
            case ValueType.Score:
                _targetValue = GM.Root.PlayerMgr._PlayerData.Score;
                break;
            case ValueType.MagicDust:
                _targetValue = GM.Root.PlayerMgr._PlayerData.MagicDust;
                break;
        }
        
        // 如果当前值和目标值不同，且没有正在进行的动画，则开始新的动画
        if (_curValue != _targetValue && !isAdding)
        {
            bool isActive = gameObject.activeInHierarchy;
            if (isActive)
                StartCoroutine(AddValue());
            else
                CommonSub();
        }
    }
    
    void CommonSub()
    {
        switch (valueType)
        {
            case ValueType.Coins:
                _targetValue = GM.Root.PlayerMgr._PlayerData.Coins;
                break;
            case ValueType.RoomKeys:
                _targetValue = GM.Root.PlayerMgr._PlayerData.RoomKeys;
                break;
            case ValueType.Score:
                _targetValue = GM.Root.PlayerMgr._PlayerData.Score;
                break;
            case ValueType.MagicDust:
                _targetValue = GM.Root.PlayerMgr._PlayerData.MagicDust;
                break;
        }
        _curValue = _targetValue;
        _txt.text = _curValue.ToString();
    }
    
    IEnumerator AddValue()
    {
        isAdding = true;
        yield return new WaitForSeconds(duration);
        int tempValue = _curValue;
        
        // 使用 DOVirtual 进行平滑过渡
        DOVirtual.Int(tempValue, _targetValue, 0.6f, value =>
            {
                _curValue = Mathf.RoundToInt(value);
                _txt.text = _curValue.ToString();
            })
            .OnComplete(() =>
            {
                isAdding = false;
                _txt.transform.DOKill();
            });
    }

    void OnDestroy()
    {
        switch (valueType)
        {
            case ValueType.Coins:
                GM.Root.PlayerMgr._PlayerData.OnCoinsAdd -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnCoinsSub -= CommonSub;
                break;
            case ValueType.RoomKeys:
                GM.Root.PlayerMgr._PlayerData.OnRoomKeysChanged -= CommonChange;
                break;
            case ValueType.Score:
                GM.Root.PlayerMgr._PlayerData.OnScoreChanged -= CommonChange;
                break;
            case ValueType.MagicDust:
                GM.Root.PlayerMgr._PlayerData.OnMagicDustAdd -= CommonChange;
                GM.Root.PlayerMgr._PlayerData.OnMagicDustSub -= CommonSub;
                break;
        }
    }
    #endregion
}
