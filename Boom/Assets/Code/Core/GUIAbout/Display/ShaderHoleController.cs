using UnityEngine;

[ExecuteInEditMode]
public class ShaderHoleController : MonoBehaviour
{
    public float radius;
    //Camera.main.WorldToScreenPoint(transform.position); sPos;
    void Update()
    {
        Vector3 sPos = Camera.main.WorldToScreenPoint(transform.position);
        Shader.SetGlobalVector("_HoleScreenPos", new Vector4(sPos.x,sPos.y, sPos.z, 1f));
        Shader.SetGlobalFloat("_HoleRadius", radius);
    }
}