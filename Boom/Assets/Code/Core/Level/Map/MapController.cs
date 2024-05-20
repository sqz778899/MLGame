using System;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public Vector2 MinMaxZ;
    
    Vector3 dragOrigin; // 用于存储地图原始位置
    BoxCollider2D curMapBox;

    void Awake()
    {
        curMapBox = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // 滚轮缩放地图逻辑
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 40;
        Vector3 curPos = transform.position;
        curPos.z -= scroll; // 根据滚轮输入修改Z值
        if (curPos.z > MinMaxZ.x && MinMaxZ.x < MinMaxZ.y)
            transform.position = curPos;

        // 检测鼠标按下事件，记录按下时鼠标的世界坐标
        if (Input.GetMouseButtonDown(0))
        {
            // 计算摄像机到地图平面的距离
            dragOrigin = GetClickPos();
        }

        // 检测鼠标拖拽事件，并计算新的地图位置
        if (Input.GetMouseButton(0))
        {
            Vector3 currentWorldPoint = GetClickPos();
            Vector3 offset = currentWorldPoint - dragOrigin;

            Vector3 oldPos = transform.position;
            Vector3 newPos = transform.position + offset;
            
            transform.position += offset ;
            dragOrigin = currentWorldPoint;
        }
        
        Vector3 fixPos = Vector2.zero;
        if (QuadrangleEdgeDetection(ref fixPos))
        {
            Debug.Log("xxxxxxxxxxxxxxxxx");
            transform.position += fixPos;
        }
    }
    

    // 获取屏幕坐标对应于给定深度平面的世界坐标
    Vector3 GetClickPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 进行射线检测
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        return hit.point;
    }

    bool RayDetection(Ray curRay)
    {
        RaycastHit hit;
        Physics.Raycast(curRay, out hit);
        if (hit.transform != null)
            return true;
        else
            return false;
    }

    Vector3 ccc(Vector3 ScreenPos)
    {
        Ray r01 = Camera.main.ScreenPointToRay(ScreenPos);
        RaycastHit hit;
        Physics.Raycast(r01, out hit);
        return hit.point;
    }

    //计算一下屏幕空间两个像素之间，在世界空间下的差值。方便后续修正坐标
    float GetCurScreenPerPixelDis()
    {
        Vector3 curScreenPos01 = new Vector3(100, 100,0);
        Vector3 curScreenPos02 = new Vector3(101, 100,0);
        float dis = Vector3.Distance(ccc(curScreenPos01) , ccc(curScreenPos02));
        return dis;
    }

    bool QuadrangleEdgeDetection(ref Vector3 fixPos)
    {
        bool IsInEdge = false;
        // 屏幕左下角的坐标
        Ray bottomLeftRay = Camera.main.ScreenPointToRay(new Vector2(0, 0));
        if (!RayDetection(bottomLeftRay))
        {
            for (int i = 0; i < 3000; i++)
            {
                Vector2 curV01 = new Vector2(i, 0);
                Vector2 curV02 = new Vector2(0, i);
                Vector2 curV03 = new Vector2(i, i);
                Ray r01 = Camera.main.ScreenPointToRay(curV01);
                RaycastHit hit;
                Physics.Raycast(r01, out hit);
                if (hit.transform != null)
                {
                    fixPos.x = -GetCurScreenPerPixelDis()*i;
                    break;
                }
            }
            IsInEdge = true;
        }

        return IsInEdge;

        /*// 屏幕右下角的坐标
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.nearClipPlane));
    
// 屏幕左上角的坐标
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Camera.main.nearClipPlane));
    
// 屏幕右上角的坐标
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));*/
    }
}
