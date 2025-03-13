using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteClickHandler : MonoBehaviour
{
    public UnityEvent onClick = new UnityEvent();

    [Header("显示相关")]
    public SpriteRenderer spriteRenderer;
    public Color HeighLightColor = Color.white;
    internal Color defaultColor;
    internal Vector3 defaultScale;
    
    [Header("功能相关")]
    public bool IsLocked = false;
    
    internal virtual void Start()
    {
        defaultColor = spriteRenderer.color;
        defaultScale = spriteRenderer.transform.localScale;
    }

    #region 虚函数们
    internal virtual void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // 如果鼠标在 UI 上，则直接返回 false
        if (IsLocked || UIManager.Instance.IsLockedClick) return;
        HighLight();
    }
    
    internal virtual void OnMouseExit()
    {
        QuitHighLight();
    }
    
    internal virtual void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())  return;
        if (IsLocked || UIManager.Instance.IsLockedClick) return;
        onClick.Invoke();
    }
    #endregion

    public void HighLight()
    {
        uint layerToAdd = 1u << 1;
        spriteRenderer.renderingLayerMask |= layerToAdd;
    }
    
    public void QuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        spriteRenderer.renderingLayerMask &= ~layerToRemove;
    }
}
