using System;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNodeView:MonoBehaviour
{
    [Header("Sprite 渲染")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer triggeredRenderer;
    
    [Header("Spine 渲染")]
    public Renderer spineRenderer;
    public SkeletonAnimation Skeleton;
    
    public MapNodeData Data { get; private set; }
    public MapNodeController controller;
    
    [Header("跳字相关")]
    float lastFloatingTime = -999f;
    const float FloatingCooldown = 0.5f; // 半秒内不重复跳字
    [Header("飞行特效参数")] 
    public EParameter EPara;
    EffectManager eEffectManager => EternalCavans.Instance._EffectManager;

    public Action OnClick; //新手引导等外部注册
    
    void Awake() => AniUtility.PlayIdle(Skeleton);

    public void Init(MapNodeData data)
    {
        Data = data;
        if (Data.EventData is BulletEventRuntimeData bulletEventRuntimeData)
        {
            int bulletID = bulletEventRuntimeData.BulletID;
            Skeleton.skeletonDataAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>(
                PathConfig.GetBulletImageOrSpinePath(bulletID, BulletInsMode.Inner));
            StartCoroutine(InitSkeleton());
        }
        controller = new MapNodeController(Data, this);
       
    }
    IEnumerator InitSkeleton()
    {
        // 等待 1 帧，确保 Spine 所有依赖生命周期跑完
        yield return null;
        Skeleton.Initialize(true);
    }

    #region 触发之后的表现
    public void SetAsTriggered(int coinsAmount = 0)
    {
        if(Data.IsTriggered)
            QuitHighLight();
        
        // 针对不同事件类型添加额外表现逻辑
        switch (Data.EventType)
        {
            case MapEventType.CoinsPile:
                PlayCoinsEffect(coinsAmount);
                break;
            case MapEventType.TreasureBox:
                NormalTriggered();
                VFXFactory.PlayFx(PathConfig.OpenBoxSmokeFX, transform.position);//播放动画/粒子等
                break;
            case MapEventType.WonderWorkshop:
                WorkshopTriggered();
                VFXFactory.PlayFx(PathConfig.OpenBoxSmokeFX, transform.position);
                break;
        }
    }

    void NormalTriggered()
    {
        if (triggeredRenderer != null)
        {
            spriteRenderer.gameObject.SetActive(false);
            triggeredRenderer.gameObject.SetActive(true);
            spriteRenderer = triggeredRenderer;
        }
    }

    void WorkshopTriggered()
    {
        if (triggeredRenderer != null) triggeredRenderer.gameObject.SetActive(false);
    }
    #endregion
    void PlayCoinsEffect(int coinsAmount)
    {
        EPara.StartPos = transform.position;
        EPara.InsNum = coinsAmount;
        eEffectManager.CreatEffect(EPara);
        Destroy(gameObject);
    }

    public void NonFind()
    {
        VFXFactory.PlayFx(PathConfig.ClickSmokeFX, transform.position);
        ShowFloatingText("已经没什么好拿的了");
    }

    public void ShowFloatingText(string message)
    {
        if (Time.time - lastFloatingTime < FloatingCooldown) return;
        FloatingTextFactory.CreateWorldText(message, transform.position,FloatingTextType.MapHint,
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
        
        OnClick?.Invoke();
    }
    
    #region 高亮显示相关
    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // 如果鼠标在 UI 上，则直接返回 false
        if (Data.IsLocked || (Data.IsTriggered && !EventTypeRules.IsRepeatable(Data.EventType))) return;
        HighLight();
    }
    
    void OnMouseExit() => QuitHighLight();
    
    public void HighLight()
    {
        uint layerToAdd = 1u << 1;
        if (ActiveRenderer != null)
            ActiveRenderer.renderingLayerMask |= layerToAdd;
    }
    
    public void QuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        if (ActiveRenderer != null)
            ActiveRenderer.renderingLayerMask &= ~layerToRemove;
    }
    
    public Renderer ActiveRenderer
    {
        get
        {
            if (triggeredRenderer != null && triggeredRenderer.gameObject.activeSelf)
                return triggeredRenderer;
            if (spriteRenderer != null && spriteRenderer.gameObject.activeSelf)
                return spriteRenderer;
            if (spineRenderer != null && spineRenderer.gameObject.activeSelf)
                return spineRenderer;
            return null;
        }
    }
    #endregion
}