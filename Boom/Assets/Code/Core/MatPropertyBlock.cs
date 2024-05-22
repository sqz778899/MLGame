using UnityEngine;

[ExecuteAlways]
public class MatPropertyBlock : MonoBehaviour
{
    public Color MColor;
    void Start()
    {
        SetBlock();
    }

    void Update()
    {
#if UNITY_EDITOR
        SetBlock();
#endif
    }

    void SetBlock()
    {
        Renderer curRenderer = GetComponent<Renderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_BaseColor", MColor);
        curRenderer.SetPropertyBlock(mpb);
    }
}
