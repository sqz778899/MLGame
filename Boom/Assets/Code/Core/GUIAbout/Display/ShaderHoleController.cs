using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderHoleController : MonoBehaviour
{
    public Material material;  // 引用要修改的材质

    void Update()
    {
        if (material != null)
        {
            // 获取世界空间中的洞位置
            Vector3 holePosition = transform.position; // 或者任何你想要的世界坐标位置

            // 将世界坐标传递给材质
            material.SetVector("_HoleWorldPos", new Vector4(holePosition.x, holePosition.y, holePosition.z, 1f));

            // 设置洞的半径
            material.SetFloat("_HoleRadius", 0.2f);  // 设置所需的半径值
        }
    }
}