using UnityEngine;
using DG.Tweening;

public class FloatingIcon : MonoBehaviour
{
    public float floatDistance = 10f;  // 上下浮动的距离
    public float duration = 1f;        // 浮动所需时间
    public Ease easeType = Ease.InOutSine;  // 过渡效果

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
        StartFloating();
    }

    public void ResetPos(Vector3 pos)
    {
        startPos = pos;
        StartFloating();
    }

    void StartFloating()
    {
        transform.DOKill();
        transform.DOLocalMoveY(startPos.y + floatDistance, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
