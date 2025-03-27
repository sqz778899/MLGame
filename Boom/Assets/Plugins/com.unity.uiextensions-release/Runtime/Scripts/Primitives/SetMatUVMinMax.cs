using UnityEngine;
using UnityEngine.UI.Extensions;

public class SetMatUVMinMax : MonoBehaviour
{
    public UIPrimitiveBase lineRenderer;
    public UILineConnector lineConnector;
    public Material curMat;
    
    Material _realMat;
   
    void Start()
    {
        _realMat = Instantiate(curMat);
        lineRenderer.material = _realMat;
    }
}
