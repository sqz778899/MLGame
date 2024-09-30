using UnityEngine;

public class ClickMaster: Singleton<ClickMaster>
{
    // 非UI空间，获取点击的位置,需要有碰撞
    public Vector3 GetClickPosInMap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 进行射线检测
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }
    
    // 非UI空间，获取点击的位置,需要有碰撞2D
    public Vector3 GetClickPosInMap2D()
    {
        // 转换鼠标位置到世界坐标
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 发射一条从鼠标位置出发的射线
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        return hit.point;
    }
}