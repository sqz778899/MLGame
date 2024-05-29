using UnityEngine;

[ExecuteAlways]
public class MatPropertyBlock : MonoBehaviour
{
    public Color MColor;
    public Color FinishColor;
    void Start()
    {
        SetBlockDefault();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            SetBlockDefault();
        }
#endif
    }

    public void SetBlockDefault()
    {
        SetBlock(MColor);
    }
    public void SetBlockFinish()
    {
        SetBlock(FinishColor);
    }
    void SetBlock(Color curColor)
    {
        Renderer curRenderer = GetComponent<Renderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_BaseColor", curColor);
        curRenderer.SetPropertyBlock(mpb);
    }
}
