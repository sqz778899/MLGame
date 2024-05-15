using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class UnLockNode : MonoBehaviour
{
    public float _FadeRange = 10;
    // Update is called once per frame
    void Update()
    {
        // 设置球心的世界坐标作为Shader全局变量
        Shader.SetGlobalVector("_UnLockNodeCenter", transform.position);
        // 假设球体的transform.localScale.x是球体的直径，那么半径是它的一半
        Shader.SetGlobalFloat("_UnLockNodeRadius", transform.localScale.x * 0.5f);
        // 假设球体的transform.localScale.x是球体的直径，那么半径是它的一半
        Shader.SetGlobalFloat("_FadeRange", _FadeRange);
    }
}
