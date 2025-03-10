using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class EXDOBezier
{
    public static Tweener DOBezier(this Transform transform, Vector3 startPos,Vector3 controlPos,
        Vector3 endPos, float time,Action callback = null)
    {
        if(transform == null)
            return null;
        DOTween.Kill(transform);
        Vector3[] path = BezierPath(startPos,controlPos,endPos);
        return transform.DOPath(path, time).OnComplete
        (()=> { if (callback != null)
                callback(); }).SetEase(Ease.InQuad);
    }
    
    static Vector3[] BezierPath(Vector3 startPos,Vector3 controlPos,Vector3 endPos)
    {
        Vector3[] path = new Vector3[10];
        for (int i = 1; i <= 10; i++)
        {
            float t = i / 10f;
            path[i - 1] = Bezier(startPos,controlPos,endPos,t);
        }
        return path;
    }
    
    static Vector3 Bezier(Vector3 startPos,Vector3 controlPos,Vector3 endPos,float t)
    {
        return (1 - t) * (1 - t) * startPos + 2 * (1 - t) * t * controlPos + t * t * endPos;
    }
}
