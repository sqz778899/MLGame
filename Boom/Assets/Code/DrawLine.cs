using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject MapNodeGroup;
    List<Transform> MapNodes;
    List<LineRenderer> allLines;
    
    void Awake()
    {
        MapNodes = new List<Transform>();
        for (int i = 0; i < MapNodeGroup.transform.childCount; i++)
            MapNodes.Add(MapNodeGroup.transform.GetChild(i));
    }

    void Start()
    {
        DrawLineByMapNode();
    }

    void Update()
    {
        for (int i = 1; i < MapNodes.Count; i++)
        {
            LineRenderer curRenderer = allLines[i];
            curRenderer.SetPosition(0,MapNodes[i-1].position);
            curRenderer.SetPosition(1,MapNodes[i].position);
        }
    }

    void DrawLineByMapNode()
    {
        for (int i = 1; i < MapNodes.Count; i++)
        {
            LineRenderer curRenderer = InstanceSingleLine();
            allLines.Add(curRenderer);
            curRenderer.SetPosition(0,MapNodes[i-1].position);
            curRenderer.SetPosition(1,MapNodes[i].position);
        }
    }

    LineRenderer InstanceSingleLine()
    {
        GameObject drawLineAsset = ResManager.instance.GetAssetCache<GameObject>(PathConfig.DrawLineAsset);
        GameObject drawLineIns = Instantiate(drawLineAsset, transform);
        LineRenderer curRenderer = drawLineIns.GetComponentInChildren<LineRenderer>();
        return curRenderer;
    }
}
