using System;
using DG.Tweening;
using UnityEngine;

public class Connon : MonoBehaviour
{
    public float AppearanceTime = 0.5f;
    public void Appear(Vector3 targetPos)
    {
        transform.DOMove(targetPos, AppearanceTime);
    }
}