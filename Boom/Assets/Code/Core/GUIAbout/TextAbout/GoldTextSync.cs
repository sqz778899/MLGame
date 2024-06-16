using TMPro;
using UnityEngine;

public class GoldTextSync : MonoBehaviour
{
    TextMeshProUGUI _txtGold;
    void Start()
    {
        _txtGold = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _txtGold.text = MainRoleManager.Instance.Gold.ToString();
    }
}
