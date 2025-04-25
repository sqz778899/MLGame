using UnityEngine;

public static class TutoConfig
{
    //新手引导的箭头的偏移的值
    public static readonly Vector3 arrowOffset = new Vector3(-0.2f,0.8f,0);
    public static readonly Vector3 arrowOffsetPortal = new Vector3(-0.2f,2f,0);

    //在教学中设置箭头
    public static void SetArrow(ParticleSystem fxArrow,Vector3 pos)
    {
        RectTransform arrowRTrans = fxArrow.GetComponent<RectTransform>();
        arrowRTrans.transform.position = pos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        fxArrow.Play();
    }

    //在教学中让物体高亮
    public static void SetTutoHigh(GameObject go,float _radius) 
        =>go.AddComponent<ShaderHoleController>().radius = _radius;
    
    //在教学中让物体取消高亮
    public static void RemoveTutoHigh(GameObject go) 
        =>Object.Destroy(go.GetComponent<ShaderHoleController>());
}