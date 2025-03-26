using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class LineConnector : MonoBehaviour
{
    public RectTransform fromUI;
    public RectTransform toUI;
    private LineRenderer lr;
    private Camera uiCamera;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        uiCamera = Camera.main; // or assign your UI camera
        lr.sortingLayerName = "UI";
        lr.sortingOrder = 5;
    }

    void Update()
    {
        if (fromUI == null || toUI == null) return;

        Vector3 worldPosA = fromUI.position;
        Vector3 worldPosB = toUI.position;

        
        lr.positionCount = 2;
        lr.SetPosition(0, worldPosA);
        lr.SetPosition(1, worldPosB);
    }
}