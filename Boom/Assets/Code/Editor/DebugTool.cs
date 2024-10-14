using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public int accountElement;
    public int maxSub;
    public float elementRatio;
    
    
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        float s = Mathf.Lerp(0, 100, elementRatio);
        Debug.Log(s);
    }
}
