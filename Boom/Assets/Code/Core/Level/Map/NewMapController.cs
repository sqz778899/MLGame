using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapController : MonoBehaviour
{
    public float Sensitive;
    public Vector2 MinMaxZ;
    Vector3 dragOrigin; // 用于存储地图原始位置
    void Update()
    {
        if (UIManager.Instance.IsPauseClick)
            return;
        
        // 滚轮缩放地图逻辑
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Sensitive;
        Vector3 curScale = transform.localScale;
        Vector3 curScaleAdd = new Vector3(scroll,scroll,scroll);
        curScale += curScaleAdd;
        if (curScale.x > MinMaxZ.x && curScale.x < MinMaxZ.y) //缩放
            transform.localScale = curScale;

        // 检测鼠标按下事件，记录按下时鼠标的世界坐标
        if (Input.GetMouseButtonDown(0))
        {
            // 计算摄像机到地图平面的距离
            dragOrigin = ClickMaster.instance.GetClickPosInMap();
        }
        
        // 检测鼠标拖拽事件，并计算新的地图位置
        if (Input.GetMouseButton(0))
        {
            Vector3 currentWorldPoint = ClickMaster.instance.GetClickPosInMap();
            if (dragOrigin == Vector3.zero || currentWorldPoint == Vector3.zero)//不知道为啥，Unity有时候捕捉不到鼠标输入的坐标
                return;
            
            Vector3 offset = currentWorldPoint - dragOrigin;
            offset.z = 0;
            
            transform.position += offset ;
            dragOrigin = currentWorldPoint;
        }
    }
}
