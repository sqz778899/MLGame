using UnityEngine;
using DG.Tweening;

public class FloatingIcon : MonoBehaviour
{
    public enum FloatDirection { Vertical, Horizontal, Custom }

    [Header("浮动设置")]
    public FloatDirection floatDirection = FloatDirection.Vertical;
    public Vector2 customDirection = Vector2.up; // 仅在 Custom 模式下有效
    public float floatDistance = 10f;
    public float duration = 1f;
    public Ease easeType = Ease.InOutSine;

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

        Vector2 dir = floatDirection switch
        {
            FloatDirection.Vertical => Vector2.up,
            FloatDirection.Horizontal => Vector2.right,
            FloatDirection.Custom => customDirection.normalized,
            _ => Vector2.up
        };

        Vector3 targetPos = startPos + (Vector3)(dir * floatDistance);

        transform.DOLocalMove(targetPos, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
