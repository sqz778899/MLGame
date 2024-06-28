using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public Canvas ss;
    public Vector3 CurScreenP;
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        Testss();
        Debug.Log(Camera.main.nearClipPlane);
        /*Vector3 s = MyRay(CurScreenP);
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p.transform.position = s;*/
    }
    void Testss()
    {
        RectTransform rectTransform = ss.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];

        // 获取四个角的位置
        rectTransform.GetWorldCorners(corners);

        for(int i = 0;i < corners.Length;i++)
        {
            Ray curRay = new Ray(corners[i], new Vector3(0, 0, 1));
            RaycastHit hit;
            Physics.Raycast(curRay, out hit);
            
            // 创建球体并设置位置
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere" + i;
            sphere.transform.position = hit.point;
        }
        
    }
    
}
