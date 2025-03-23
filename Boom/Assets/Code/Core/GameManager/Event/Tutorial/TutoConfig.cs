using UnityEngine;

public static class TutoConfig
{
    //新手引导的箭头的偏移的值
    public static readonly Vector3 arrowOffset = new Vector3(-0.2f,0.8f,0);

    public static void SetArrow(ParticleSystem fxArrow,Vector3 pos)
    {
        RectTransform arrowRTrans = fxArrow.GetComponent<RectTransform>();
        arrowRTrans.transform.position = pos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        fxArrow.Play();
    }
}