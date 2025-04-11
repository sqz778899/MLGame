using System;
using UnityEngine;
using Spine.Unity;

public class BulletInnerSelect:MonoBehaviour
{
    BulletData _data; //绝对核心数据
    BagRootMini _bagRootMini;
    SkeletonAnimation _ain;
    Renderer _renderer;
    
    [Header("表现层")]
    public BulletEditInner DragBase;
    bool _isDragging = false;
    public Action OnBulletDragged;
    
    void Awake()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        SpineQuitHighLight();
    }

    void Start()
    {
        _data = GetComponent<BulletInner>()._data;//同步数据
        DragBase = BulletFactory.CreateBullet(_data, BulletInsMode.EditInner) as BulletEditInner;
        DragBase.gameObject.transform.SetParent(DragManager.Instance.dragRoot.transform,false);
        DragBase._data = _data;
        DragBase.InitData();
        DragBase.gameObject.SetActive(false);
        
        _bagRootMini = UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        
        OnBulletDragged += _bagRootMini.BulletDragged;
    }
    
    internal void OnMouseEnter()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineHighLight();
    }

    internal void OnMouseExit()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineQuitHighLight();
    }
    
    internal void OnMouseDown()
    {
        DragBase.gameObject.SetActive(true);
        UpdateDragTempGoPosition();

        //隐藏表现层(本体Renderer)
        _renderer.enabled = false;
        OnBulletDragged?.Invoke();
        _isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!_renderer.enabled) // 拖拽时更新位置
            UpdateDragTempGoPosition();
    }

    internal void OnMouseUp()
    {
        //松开时，模拟结束拖拽
        if (DragBase.EndDragSimulation())//如果未发生逻辑，则弹回原位
        {
            if (!_renderer.enabled)
            {
                DragBase.gameObject.SetActive(false);
                _renderer.enabled = true;
            }
        }
    }

    #region 一些私有方法
    void UpdateDragTempGoPosition()
    {
        // 将鼠标位置转为 UI 世界坐标
        Vector3 screenPoint = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            DragManager.Instance.dragRoot.GetComponent<RectTransform>(),
            screenPoint,
            Camera.main,
            out Vector3 uiWorldPoint
        );

        // 更新 UI 分身的位置
        DragBase.gameObject.transform.position = uiWorldPoint;
    }
    void SpineHighLight()
    {
        uint layerToAdd = 1u << 1;
        _renderer.renderingLayerMask |= layerToAdd;
    }
    
    void SpineQuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        _renderer.renderingLayerMask &= ~layerToRemove;
    }
    
    void OnDestroy()
    {
        if (DragBase != null)
            Destroy(DragBase.gameObject);
        if (_bagRootMini != null)
            OnBulletDragged -= _bagRootMini.BulletDragged;
    }

    #endregion      
}