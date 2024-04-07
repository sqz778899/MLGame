using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RollBullet:BulletBase,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public int Cost;
    Image _rollImage;
    TextMeshProUGUI _rollScore;
    void Start()
    {
        bulletInsMode = BulletInsMode.Roll;
        InitImage();
    }

    void InitImage()
    {
        Transform[] allTrans = GetComponentsInChildren<Transform>();
        foreach (var each in allTrans)
        {
            if (each.gameObject.name == "imgBullet")
            {
                _rollImage = each.GetComponent<Image>();
                break;
            }

            if (each.gameObject.name == "txtScore")
            {
                _rollScore = each.GetComponent<TextMeshProUGUI>();
                break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_rollImage != null)
            _rollImage.color = Color.yellow;
        if (_rollScore != null)
            _rollScore.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_rollImage != null)
            _rollImage.color = Color.white;
        if (_rollScore != null)
            _rollScore.color = new Color(0.66f, 0.66f, 0.66f, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RollManager.Instance.SelOne(eventData.pointerClick);
    }
}