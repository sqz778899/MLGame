using UnityEngine;

public class MapControl : MonoBehaviour
{
    public Vector2 MinMaxZ;
    
    Vector3 dragOrigin; // 用于存储地图原始位置
    
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
            
            transform.position += offset ;
            dragOrigin = currentWorldPoint;
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
}
