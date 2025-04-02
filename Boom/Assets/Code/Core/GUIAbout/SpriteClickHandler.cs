using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteClickHandler : MonoBehaviour
{
    public UnityEvent onClick = new UnityEvent();

    [Header("显示相关")]
    public SpriteRenderer spriteRenderer;
    public Renderer _renderer;
    
    [Header("功能相关")]
    public bool IsLocked = false;
    public bool IsSpeTutorial = false;//新手教程要用的flag，优先级高于一切
    
    internal virtual void Start()
    {
        if (spriteRenderer != null) {}
        else
            _renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    #region 虚函数们
    internal virtual void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // 如果鼠标在 UI 上，则直接返回 false
        if ((IsLocked || UIManager.Instance.IsLockedClick) && !IsSpeTutorial) return;
        HighLight();
    }
    
    internal virtual void OnMouseExit()
    {
        QuitHighLight();
    }
    
    internal virtual void OnMouseUp()
    {
        // 检测是否有UI遮挡
        if (EventSystem.current.IsPointerOverGameObject()) return; // 如果鼠标在 UI 上，则直接返回 false
        /*{
            // 获取所有被点击的UI对象
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // 打印所有遮挡的UI对象名字
            if (results.Count > 0)
            {
                Debug.Log("遮挡的UI对象数量: " + results.Count);
                foreach (var result in results)
                {
                    Debug.Log("遮挡的UI对象: " + result.gameObject.name);
                }
            }
            return; // 如果有UI遮挡，直接返回
        }*/
        if ((IsLocked || UIManager.Instance.IsLockedClick) && !IsSpeTutorial) return;
        onClick.Invoke();
    }
    #endregion

    #region 高亮显示相关
    public void HighLight()
    {
        uint layerToAdd = 1u << 1;
        if (spriteRenderer == null)
            _renderer.renderingLayerMask |= layerToAdd;
        else
            spriteRenderer.renderingLayerMask |= layerToAdd;
     
    }
    
    public void QuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        if (spriteRenderer == null)
            _renderer.renderingLayerMask &= ~layerToRemove;
        else
            spriteRenderer.renderingLayerMask &= ~layerToRemove;
    }
    #endregion
}
