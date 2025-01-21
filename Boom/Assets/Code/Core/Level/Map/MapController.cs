using System;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public float Sensitive;
    public Vector2 MinMaxZ;
    public Canvas CurCanvas;
    
    Vector3 dragOrigin; // 用于存储地图原始位置

    void Update()
    {
        if (UIManager.Instance.IsLockedClick)
            return;
        
        // 滚轮缩放地图逻辑
        float scroll = Input.GetAxis("Mouse ScrollWheel") * Sensitive;
        Vector3 curPos = transform.position;
        curPos.z -= scroll; // 根据滚轮输入修改Z值
        if (curPos.z > MinMaxZ.x && curPos.z < MinMaxZ.y)
        {
            transform.position = curPos;
        }

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
            if (dragOrigin == Vector3.zero || currentWorldPoint == Vector3.zero)//不知道为啥，Unity有时候捕捉不到鼠标输入的坐标
                return;
            
            Vector3 offset = currentWorldPoint - dragOrigin;
            offset.z = 0;
            
            transform.position += offset ;
            dragOrigin = currentWorldPoint;
        }

        FixMapEdge();
    }

    #region 好费劲写的地图边缘检测逻辑，好像终于准了
    void FixMapEdge()
    {
        Vector3 fixV = Vector3.zero;
        bool posLegal = true;
        CornersDetection(ref posLegal,ref fixV);
        if (!posLegal)
            transform.position += fixV ;
    }
    void CornersDetection(ref bool posLegal,ref Vector3 fixV)
    {
        int detectStep = 2;
        RectTransform rectTransform = CurCanvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];

        // 获取四个角的位置
        rectTransform.GetWorldCorners(corners);
        Vector3[] dirs = GetDirs();
        
        //射线检测四个角有无碰撞
        Ray curRayBottomLeft = new Ray(corners[0], dirs[0]);
        Physics.Raycast(curRayBottomLeft, out RaycastHit hitBottomLeft);
        
        Ray curRayTopLeft = new Ray(corners[1], dirs[1]);
        Physics.Raycast(curRayTopLeft, out RaycastHit hitTopLeft);
        
        Ray curRayTopRight = new Ray(corners[2], dirs[2]);
        Physics.Raycast(curRayTopRight, out RaycastHit hitTopRight);
        
        Ray curRayBottomRight = new Ray(corners[3], dirs[3]);
        Physics.Raycast(curRayBottomRight, out RaycastHit hitBottomRight);

        #region 四边Case
        //.....................小地图到左侧边缘了.........................
        if (hitBottomLeft.transform == null && hitTopLeft.transform == null)
        {
            bool isFind = false;
            posLegal = false;
            for (int i = 0; i < 3000; i++)
            {
                Vector3 curV = new Vector3(corners[0].x + i * detectStep, corners[0].y, corners[0].z);
                Ray curTempRay = new Ray(curV, dirs[0]);
                Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                if (tempHitBottomLeft.transform != null)
                {
                    isFind = true;
                    fixV = new Vector3(i * detectStep,0,0) * -1;
                    break;
                }
            }

            if (!isFind)
            {
                for (int i = 0; i < 3000; i++)
                {
                    Vector3 curV = new Vector3(corners[1].x + i * detectStep, corners[1].y, corners[1].z);
                    Ray curTempRay = new Ray(curV, dirs[1]);
                    Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                    if (tempHitBottomLeft.transform != null)
                    {
                        isFind = true;
                        fixV = new Vector3(i * detectStep,0,0) * -1;
                        break;
                    }
                }
            }
        }
        //.....................小地图到右侧边缘了.........................
        if (hitTopRight.transform == null && hitBottomRight.transform == null)
        {
            bool isFind = false;
            posLegal = false;
            for (int i = 0; i < 3000; i++)
            {
                Vector3 curV = new Vector3(corners[3].x - i * detectStep, corners[3].y, corners[3].z);
                Ray curTempRay = new Ray(curV, dirs[3]);
                Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                if (tempHitBottomLeft.transform != null)
                {
                    isFind = true;
                    fixV = new Vector3(i * detectStep,0,0);
                    break;
                }
            }
            if (!isFind)
            {
                for (int i = 0; i < 3000; i++)
                {
                    Vector3 curV = new Vector3(corners[2].x - i * detectStep, corners[2].y, corners[2].z);
                    Ray curTempRay = new Ray(curV, dirs[2]);
                    Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                    if (tempHitBottomLeft.transform != null)
                    {
                        fixV = new Vector3(i * detectStep,0,0);
                        break;
                    }
                }
            }
        }
        
        //.....................小地图到上面边缘了.........................
        if (hitTopRight.transform == null && hitTopLeft.transform == null)
        {
            bool isFind = false;
            posLegal = false;
            for (int i = 0; i < 3000; i++)
            {
                Vector3 curV = new Vector3(corners[1].x , corners[1].y - i * detectStep, corners[1].z);
                Ray curTempRay = new Ray(curV, dirs[1]);
                Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                if (tempHitBottomLeft.transform != null)
                {
                    isFind = true;
                    fixV = new Vector3(0,i * detectStep,0);
                    break;
                }
            }
            if (!isFind)
            {
                for (int i = 0; i < 3000; i++)
                {
                    Vector3 curV = new Vector3(corners[2].x , corners[2].y - i * detectStep, corners[2].z);
                    Ray curTempRay = new Ray(curV, dirs[2]);
                    Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                    if (tempHitBottomLeft.transform != null)
                    {
                        fixV = new Vector3(0,i * detectStep,0);
                        break;
                    }
                }
            }
        }
        
        //.....................小地图到下面边缘了.........................
        if (hitBottomRight.transform == null && hitBottomLeft.transform == null)
        {
            bool isFind = false;
            posLegal = false;
            for (int i = 0; i < 3000; i++)
            {
                Vector3 curV = new Vector3(corners[3].x , corners[3].y + i * detectStep, corners[3].z);
                Ray curTempRay = new Ray(curV, dirs[3]);
                Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                if (tempHitBottomLeft.transform != null)
                {
                    isFind = true;
                    fixV = new Vector3(0,i * detectStep,0) * -1;
                    break;
                }
            }

            if (!isFind)
            {
                for (int i = 0; i < 3000; i++)
                {
                    Vector3 curV = new Vector3(corners[2].x , corners[2].y + i * detectStep, corners[2].z);
                    Ray curTempRay = new Ray(curV, dirs[2]);
                    Physics.Raycast(curTempRay, out RaycastHit tempHitBottomLeft);
                    if (tempHitBottomLeft.transform != null)
                    {
                        fixV = new Vector3(0,i * detectStep,0) * -1;
                        break;
                    }
                }
            }
        }
        #endregion
    }
    

    // 获取屏幕坐标对应于给定深度平面的世界坐标
    Vector3 GetClickPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 进行射线检测
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }

    Vector3[] GetDirs()
    {
        Vector3[] dirs = new Vector3[4];
        CameraCulRay curCulRay = GetCameraCulRay();
        // 获取四个角的位置
        RectTransform rectTransform = CurCanvas.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        dirs[0] = Vector3.Normalize(curCulRay.bottomLeft - corners[0]);
        dirs[1] = Vector3.Normalize(curCulRay.topLeft - corners[1]);
        dirs[2] = Vector3.Normalize(curCulRay.topRight - corners[2]);
        dirs[3] = Vector3.Normalize(curCulRay.bottomRight - corners[3]);

        return dirs;
    }
    
    
    internal class CameraCulRay
    {
        public Vector3 topRight;
        public Vector3 topLeft;
        public Vector3 bottomRight;
        public Vector3 bottomLeft;
    }
    
    CameraCulRay GetCameraCulRay()
    {
        Camera _camera = Camera.main;
        CameraCulRay _cameraRay = new CameraCulRay();
    
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);
    
        _cameraRay.topLeft = _camera.transform.forward - toRight + toTop;
        float camScale = _cameraRay.topLeft.magnitude * camFar;
    
        _cameraRay.topLeft.Normalize();
        _cameraRay.topLeft *= camScale;

        _cameraRay.topRight = _camera.transform.forward + toRight + toTop;
        _cameraRay.topRight.Normalize();
        _cameraRay.topRight *= camScale;
    
        _cameraRay.bottomRight = _camera.transform.forward + toRight - toTop;
        _cameraRay.bottomRight.Normalize();
        _cameraRay.bottomRight *= camScale;
    
        _cameraRay.bottomLeft = _camera.transform.forward - toRight - toTop;
        _cameraRay.bottomLeft.Normalize();
        _cameraRay.bottomLeft *= camScale;

        return _cameraRay;
    }
    #endregion

}
