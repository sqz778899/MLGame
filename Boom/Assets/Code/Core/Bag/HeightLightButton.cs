using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeightLightButton : MonoBehaviour,IPointerExitHandler,IPointerMoveHandler
{
    ParticleSystem _myMark;
    TextMeshProUGUI _myText;
    Color _textOrginalColor;
    void Start()
    {
        _myMark = GetComponentInChildren<ParticleSystem>();
        _myText = GetComponentInChildren<TextMeshProUGUI>();
        _textOrginalColor = _myText.color;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _myText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _myText.color = _textOrginalColor;
    }
}
