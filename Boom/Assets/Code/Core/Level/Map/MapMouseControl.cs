using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMouseControl : MonoBehaviour
{
    public float Sensitive;
    public Vector2 MinMaxZ;
    Vector3 dragOrigin; // 用于存储地图原始位置

    Vector2 preMousePos;
    Vector2 lastMousePos;
    public float dragThreshold = 1f; // 最小偏移量阈值，避免微小的移动触发抖动
    
    bool isLocked = false;
    public void LockMap() => isLocked = true;
    public void UnLockMap() => isLocked = false;
    
    void Update()
    {
        if (UIManager.Instance.IsLockedClick)
            return;
        if (isLocked) return;
        // 滚轮缩放地图逻辑
        HandleZoom();
        //拖拽地图逻辑
        MapDrag();
    }

    #region 缩放拖拽
    //拖拽地图逻辑
    void MapDrag()
    {
        // 检测鼠标按下事件，记录按下时鼠标的世界坐标
        if (Input.GetMouseButtonDown(0))
        {
            // 计算摄像机到地图平面的距离
            dragOrigin = GetClickPosInMap();
            preMousePos = Input.mousePosition; // 初始化 preMousePos
        }
        
        // 检测鼠标拖拽事件，并计算新的摄像机位置
        if (Input.GetMouseButton(0))
        {
            lastMousePos = Input.mousePosition;
            Vector2 mouseDelta = lastMousePos - preMousePos;

            // 只有当鼠标移动超过阈值时才计算偏移
            if (mouseDelta.magnitude < dragThreshold)
                return;
            preMousePos = lastMousePos;

            // 计算鼠标偏移量
            Vector3 offset = dragOrigin - GetClickPosInMap();
            offset.z = 0; // 保证只在X和Y轴上移动
            // 移动摄像机位置
            Camera.main.transform.position += offset;
            // 新摄像机位置下重新锚定坐标
            dragOrigin = GetClickPosInMap();
        }
    }
    // 处理滚轮缩放
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Sensitive;
        float newSize = Camera.main.orthographicSize - scroll;

        // 确保缩放在规定的范围内
        if (newSize > MinMaxZ.x && newSize < MinMaxZ.y)
            Camera.main.orthographicSize = newSize;
    }
    
    Vector3 GetClickPosInMap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 进行射线检测
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }
    #endregion
}
