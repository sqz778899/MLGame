using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFlashEffect : MonoBehaviour
{
    [SerializeField] Image _image;

    public void Flash(float intensity = 0.8f, float duration = 0.2f,Color baseColor = default)
    {
        if (baseColor == default)
            baseColor = Color.white;
        _image.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a);
        _image.DOKill(); // 取消残余动画
        _image.DOFade(intensity, 0.05f).OnComplete(() =>
        {
            _image.DOFade(0, duration);
        });
    }
}