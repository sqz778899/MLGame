using UnityEngine;
using UnityEngine.EventSystems;

public class MapNodeView:MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite triggeredSprite;
    public MapNodeData Data { get; private set; }
    private MapNodeController controller;
    
    [Header("跳字相关")]
    float lastFloatingTime = -999f;
    const float FloatingCooldown = 0.5f; // 半秒内不重复跳字
    [Header("飞行特效参数")] 
    public EParameter EPara;
    EffectManager _effectManager;
    EffectManager eEffectManager
    {
        get
        {
            if (_effectManager==null)
                _effectManager = UIManager.Instance.CommonUI.EffectRoot.GetComponent<EffectManager>();
            return _effectManager;
        }
    }

    public void Init(MapNodeData data)
    {
        Data = data;
        controller = new MapNodeController(Data, this);
        SetDefaultVisual();
    }
    
    public void SetAsTriggered(int coinsAmount = 0)
    {
        if (triggeredSprite != null)
            spriteRenderer.sprite = triggeredSprite;
        QuitHighLight();
        
        // 针对不同事件类型添加额外表现逻辑
        switch (Data.EventType)
        {
            case MapEventType.CoinsPile:
                PlayCoinsEffect(coinsAmount);
                break;
            case MapEventType.WeaponRack:
                // 可选：播放开柜子动画/粒子等
                break;
        }
    }
    void PlayCoinsEffect(int coinsAmount)
    {
        EPara.StartPos = transform.position;
        EPara.InsNum = coinsAmount;
        eEffectManager.CreatEffect(EPara);
        Destroy(gameObject);
    }

    public void ShowFloatingText(string message)
    {
        if (Time.time - lastFloatingTime < FloatingCooldown) return;
        FloatingTextFactory.CreateWorldText(message, transform.position,
            new Color(0.8f,0.8f,0.8f,1),3f);
        lastFloatingTime = Time.time;
    }
  
    void OnMouseUp()
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
        controller.OnInteract();
    }
    
    void SetDefaultVisual()
    {
        if (Data.IsTriggered && triggeredSprite != null)
            spriteRenderer.sprite = triggeredSprite;
    }

    #region 高亮显示相关
    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // 如果鼠标在 UI 上，则直接返回 false
        if ((Data.IsLocked || Data.IsTriggered)) return;
        HighLight();
    }
    
    void OnMouseExit() => QuitHighLight();
    
    public void HighLight()
    {
        uint layerToAdd = 1u << 1;
        if (spriteRenderer == null)
            spriteRenderer.renderingLayerMask |= layerToAdd;
        else
            spriteRenderer.renderingLayerMask |= layerToAdd;
     
    }
    
    public void QuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        if (spriteRenderer == null)
            spriteRenderer.renderingLayerMask &= ~layerToRemove;
        else
            spriteRenderer.renderingLayerMask &= ~layerToRemove;
    }
    #endregion
}