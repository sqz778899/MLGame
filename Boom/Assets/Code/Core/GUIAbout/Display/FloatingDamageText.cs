using UnityEngine;
using TMPro;
using DG.Tweening; // 需要导入 DOTween 库

public class FloatingDamageText : MonoBehaviour
{
    TextMeshPro damageText; // 伤害文本
    TextMeshProUGUI damageTextUI; // 伤害文本
    public float moveUpDistance = 50f; // 上升距离
    public float duration = 1f; // 动画时间
    
    public void AnimateText(string text, Color color,float _frontSize = 4f)
    {
        damageText ??= GetComponent<TextMeshPro>();
        
        damageText.text = text;
        damageText.color = color;
        damageText.fontSize = _frontSize;
        PlayTxtAni();
    }

    public void AnimateTextUI(string text, Color color,float _frontSize = 4f)
    {
        damageTextUI ??= GetComponent<TextMeshProUGUI>();
        
        damageTextUI.text = text;
        damageTextUI.color = color;
        damageTextUI.fontSize = _frontSize;
        PlayTxtAni();
    }
    
    void PlayTxtAni()
    {
        // 记录初始位置
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, moveUpDistance, 0);

        // 文字上浮 + 透明度渐变
        transform.DOMoveY(endPos.y, duration).SetEase(Ease.OutCubic);
        damageText.DOFade(0, duration).SetEase(Ease.OutCubic);

        // 一定时间后销毁对象
        Destroy(gameObject, duration);
    }
}

