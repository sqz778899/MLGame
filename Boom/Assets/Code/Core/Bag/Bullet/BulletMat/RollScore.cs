using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RollScore:RollBase
{
    public TextMeshProUGUI _rollScore;
    public int Score;

    internal override void Update()
    {
        base.Update();
        _rollScore.text = Score.ToString();
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        _rollScore.color = Color.yellow;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _rollScore.color = OrignalColor;
    }
}