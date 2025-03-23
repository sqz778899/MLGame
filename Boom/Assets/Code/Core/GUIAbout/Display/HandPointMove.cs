using DG.Tweening;
using UnityEngine;

public class HandPointMove : MonoBehaviour
{
    public Transform startTrans; // 起始位置
    public Transform endTrans;   // 目标位置
    public float SlowtDuration = 2f;   // 动画时长
    public float FastDuration = 2f;   // 动画时长
    public AnimationCurve moveCurve; // 动画曲线
    private Sequence sequence;

    public void ResetPos()
    {
        sequence.Kill();
        transform.position = startTrans.position;
        MoveObject();
    }

    private void MoveObject()
    {
        // 先移动到目标位置（缓慢）
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endTrans.position, SlowtDuration)
            .SetEase(moveCurve));  // 使用自定义的动画曲线
        // 然后迅速返回到起始位置
        sequence.Append(transform.DOMove(startTrans.position, FastDuration)
            .SetEase(Ease.Linear));  // 使用线性（快速）插值
        // 循环执行上述动作
        sequence.SetLoops(-1, LoopType.Restart);
    }
}
