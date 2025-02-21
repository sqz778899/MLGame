using UnityEngine;

[ExecuteInEditMode]
public class ShaderHoleController : MonoBehaviour
{
    public GameObject FXMark;
    public Material material;  // 引用要修改的材质
    public float radius;
    //Camera.main.WorldToScreenPoint(transform.position); sPos;
    void Start()
    {
        material = ResManager.instance.GetAssetCache<Material>(PathConfig.HoleMat);
        //Camera.main.WorldToScreenPoint(transform.position);
    }

    void Update()
    {
        if (material != null)
        {
            Vector3 sPos = Camera.main.WorldToScreenPoint(transform.position);
            // 获取世界空间中的洞位置
            material.SetVector("_HoleScreenPos", new Vector4(sPos.x,sPos.y, sPos.z, 1f));

            // 设置洞的半径
            material.SetFloat("_HoleRadius", radius);  // 设置所需的半径值
        }
    }
}