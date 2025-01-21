using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;

public class GoldTextSync : MonoBehaviour
{
    public float duration = 1f;
    TextMeshProUGUI _txtCoins;
    int _curCoins;
    int _targetCoins;
    bool isAddingCoins = false;
    
    void Start()
    {
        _txtCoins = GetComponent<TextMeshProUGUI>();
        _txtCoins.text = MainRoleManager.Instance.Coins.ToString();
        _curCoins = _targetCoins = MainRoleManager.Instance.Coins;
    }

    void Update()
    {
        _targetCoins = MainRoleManager.Instance.Coins;
        if (_curCoins != _targetCoins && !isAddingCoins)
        {
            StartCoroutine(AddCoins());
        }
    }
    
    IEnumerator AddCoins()
    {
        isAddingCoins = true;
        yield return new WaitForSeconds(duration);
        int tempCoins = _curCoins;
        DOVirtual.Int(tempCoins, _targetCoins, 0.6f, value =>
        {
            _curCoins = Mathf.RoundToInt(value);
            _txtCoins.text = _curCoins.ToString();
        })
            .OnComplete(() =>
            {
                isAddingCoins = false;
                _txtCoins.transform.DOKill();
            });
    }
}