using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public MainNode MainNode;
    public Transform testSphere;
    
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        float maxRadius = MainNode.ColCotain.radius;
        float Step = MainNode.Step;
        List<Vector3> LayoutPoints = NodeUtility.CreateLayoutPoints(
            maxRadius,0.1f,MainNode.transform.position.z);
        GameObject root = new GameObject("Root");
        foreach (var each in LayoutPoints)
        {
            CreateSphere(each,root.transform);
        }
    }

    void CreateSphere(Vector2 pos,Transform root)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        p.transform.position = pos;
        p.transform.SetParent(root);
    }
    
    
    [Button(ButtonSizes.Large)]
    void Debugsss2()
    {
        Vector2 pos = new Vector2(6.5f,1.5f);
        Debug.Log(MainNode.ColExclude.radius);
        if (MainNode.ColExclude.bounds.Contains(pos))
        {
            Debug.Log("Overlap");
        }
        else
        {
            Debug.Log("Not Overlap");
        }
    }
    
}
