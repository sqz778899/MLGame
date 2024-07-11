using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public MainNode MainNode;
    public MapNodeBase ExceptNode;
    
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        float maxRadius = 30;
        float Step = MainNode.Step;
        Debug.Log(MainNode.transform.position.z);
        List<Vector3> LayoutPoints = NodeUtility.CreateLayoutPoints(
            maxRadius,1f,MainNode.transform.position);
        GameObject root = new GameObject("Root");
        //NodeUtility.ExcludePointsPool(ref LayoutPoints, MainNode.ColExclude);
        //NodeUtility.ExcludePointsPool(ref LayoutPoints, ExceptNode.ColExclude);
        foreach (var each in LayoutPoints)
        {
            CreateSphere(each,root.transform);
        }
    }

    void CreateSphere(Vector3 pos,Transform root)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //p.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        p.transform.position = pos;
        p.transform.SetParent(root);
    }
}
