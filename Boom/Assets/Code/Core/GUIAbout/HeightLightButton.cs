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
        _myMark = GetComponentInChildren<ParticleSystem>(true);
        _myMark.Stop();
        _myMark.gameObject.SetActive(false);
        _myText = GetComponentInChildren<TextMeshProUGUI>(true);
        _textOrginalColor = _myText.color;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!_myMark.gameObject.activeSelf)
        {
            _myMark.gameObject.SetActive(true);
            _myMark.Play();
        }
        _myText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_myMark.gameObject.activeSelf)
        {
            _myMark.Stop();
            _myMark.gameObject.SetActive(false);
        }
        _myText.color = _textOrginalColor;
    }
}
