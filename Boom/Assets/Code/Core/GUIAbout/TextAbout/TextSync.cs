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
        RoomKeys,
        Coins,
        Score
    }
    public ValueType valueType;

    void Start()
    {
        _txt = GetComponent<TextMeshProUGUI>();
        // 根据 `valueType` 初始化
        switch (valueType)
        {
            case ValueType.Coins:
                _curValue = _targetValue = MainRoleManager.Instance.Coins;
                break;
            case ValueType.RoomKeys:
                _curValue = _targetValue = MainRoleManager.Instance.RoomKeys;
                break;
            case ValueType.Score:
                _curValue = _targetValue = MainRoleManager.Instance.Score;
                break;
        }
        _txt.text = _curValue.ToString();
    }

    void Update()
    {
        // 更新目标值
        switch (valueType)
        {
            case ValueType.Coins:
                _targetValue = MainRoleManager.Instance.Coins;
                break;
            case ValueType.RoomKeys:
                _targetValue = MainRoleManager.Instance.RoomKeys;
                break;
            case ValueType.Score:
                _targetValue = MainRoleManager.Instance.Score;
                break;
        }

        // 如果当前值和目标值不同，且没有正在进行的动画，则开始新的动画
        if (_curValue != _targetValue && !isAdding)
            StartCoroutine(AddValue());
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
}
