using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public GameObject orgin;
    
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        Testss();
    }
    void Testss()
    {
        float maxRadius = 15f;
        
        float step = 0.5f;
        float startY = maxRadius;
        float startX = maxRadius;

        int Count = (int)(maxRadius * 2 / step) + 1;
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < Count; j++)
            {
                float x = startX - step * i;
                float y = startY - step * j;
                if (CheckIden(x, y,maxRadius))
                {
                    CreateSphere(new Vector2(x, y));
                }
            }
        }
    }

    bool CheckIden(float x, float y,float radius)
    {
        bool s = false;
        if ((x * x + y * y) <= radius*radius)
        {
            s = true;
        }
        return s;
    }

    void CreateSphere(Vector2 pos)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p.transform.position = pos;
    }
    
}
