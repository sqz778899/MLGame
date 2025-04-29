using UnityEngine;
using UnityEngine.Serialization;

public class BulletInnerDragAgent : MonoBehaviour
{
    [Header("战斗表现引用")]
    public BulletInner BulletInnerSC;
    public Renderer MainRenderer;

    [Header("UI 分身资产")]
    BulletNew dragGhostSC;

    BagRootMini bagRootMini;
    BulletData data;

    void Start()
    {
        data = BulletInnerSC.controller.Data;
        // 初始化拖拽影子
        dragGhostSC = BulletFactory.CreateBullet(data, BulletInsMode.EditA) as BulletNew;
        dragGhostSC.gameObject.SetActive(false);
        dragGhostSC.transform.SetParent(DragManager.Instance.dragRoot.transform,false);
        bagRootMini = EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>();
    }

    void OnMouseDown()
    {
        // 隐藏本体,显示Ghost
        MainRenderer.enabled = false;
        dragGhostSC.gameObject.SetActive(true);
        //开始拖拽
        bagRootMini.BulletDragged();
        DragManager.Instance.ForceDrag(dragGhostSC.gameObject);
        GM.Root.InventoryMgr.RemoveBulletToFight(gameObject);
    }
    
    void Update()
    {
        // 如果需要：松开恢复显示逻辑
        if (Input.GetMouseButtonUp(0) && 
            dragGhostSC != null && 
            dragGhostSC.gameObject.activeSelf)
        {
            MainRenderer.enabled = true;
            dragGhostSC.gameObject.SetActive(false);
        }
    }

    #region 不太需要关心的逻辑
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
    
    void SpineHighLight()
    {
        uint layerToAdd = 1u << 1;
        MainRenderer.renderingLayerMask |= layerToAdd;
    }
    
    void SpineQuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        MainRenderer.renderingLayerMask &= ~layerToRemove;
    }
    #endregion
}
