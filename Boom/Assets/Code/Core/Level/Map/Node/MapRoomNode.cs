using System.Collections;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class MapRoomNode : MonoBehaviour
{
    [Header("重要属性")]
    public int RoomID;
    public bool IsStartRoom; //是否是起始房间

    [SerializeField]
    private MapRoomState _state;
    public MapRoomState State
    {
        set
        {
            if (_state != value)
            {
                _state = value;
                UpdateResState();  // 当State变化时，调用UpdateResState
            } 
        }
        get => _state;
    }
    MapRoomState preState;
    public Transform CameraStartPos;
    public Transform RoleStartPos;
    
    [Header("表现相关")]
    public float dissolveDuration = 2f; // 动画持续时间
    public SpriteRenderer RoomFog;
    Material _instanceFogMat;
    public Vector2 DissolveDir = new Vector2(1, 0);
    
    [Header("渲染相关")]
    public string SortingLayerName;
    Renderer[] _renderers;
    
    GameObject resRoot;
    GameObject _resRoot
    {
        get
        {
            if (!resRoot)
                resRoot = transform.Cast<Transform>()
                    .FirstOrDefault(t => t.name == "ResRoot")?.gameObject;
            return resRoot;
        }
    }
    
    //Room节点下全部资产信息
    public ArrowNode[] _arrows;     //全部的箭头
    public MapNodeBase[] _resources; //全部的资源
    public bool IsFogUnLocked; //播放完解锁动画了，已经全部解锁了

    public void InitData()
    {
        _arrows = GetComponentsInChildren<ArrowNode>();
        _resources = _resRoot.GetComponentsInChildren<MapNodeBase>();
        if (RoomFog)
        {
            _instanceFogMat = new Material(RoomFog.material);
            RoomFog.material = _instanceFogMat;
        }
        IsFogUnLocked = false;
        
        //2）设置渲染层级
        SetRenderLayer();
    }

    public void SetRenderLayer()
    {
        if (SortingLayerName == "") return;
        
        _renderers = GetComponentsInChildren<Renderer>(true);
        int targetLayerID = SortingLayer.NameToID(SortingLayerName);
        _renderers.ForEach(r=> r.sortingLayerID = targetLayerID);
        _resources.ForEach(res => res.SortingLayerName = SortingLayerName);
    }

    public void SetRenderLayer(GameObject Role)
    {
        if (SortingLayerName == "") return;
        Renderer[] allRenderers = Role.GetComponentsInChildren<Renderer>(true);
        int targetLayerID = SortingLayer.NameToID(SortingLayerName);
        allRenderers.ForEach(r=> r.sortingLayerID = targetLayerID);
    }
    

    #region 开启背包时锁定所有物体
    public void LockRes()
    {
        if (State == MapRoomState.IsLocked) return;
        _resources.ToList().ForEach(r => r.IsLocked = true);
        _arrows.ToList().ForEach(r => r.IsLocked = true);
    }
    
    public void UnLockRes()
    {
        if (State == MapRoomState.IsLocked) return;
        _resources.ToList().ForEach(r => r.IsLocked = false);
        _arrows.ToList().ForEach(r => r.IsLocked = false);
    }
    #endregion

    //更新资产状态
    void UpdateResState()
    {
        if (State == MapRoomState.IsLocked)
        {
            _resources.ToList().ForEach(r => r.IsLocked = true);
            _arrows.ToList().ForEach(r => r.IsLocked = true);
        }
        else
        {
            if(RoomFog)
                StartCoroutine(UnlockRoomAnimation());
            else
            {
                _resources.ToList().ForEach(r => r.IsLocked = false);
                _arrows.ToList().ForEach(r => r.IsLocked = false);
            }
        }
    }

    #region 迷雾解锁溶解相关
    private IEnumerator UnlockRoomAnimation()
    {
        // 设置溶解方向
        int flip = (DissolveDir.x < 0 || DissolveDir.y < 0) ? 1 : 0;
        _instanceFogMat.SetInt("_Flip", flip);
        DissolveDir = (DissolveDir.x < 0 || DissolveDir.y < 0) ? -DissolveDir : DissolveDir;
        _instanceFogMat.SetVector("_DissolveDirection", new Vector4(DissolveDir.x, DissolveDir.y, 0,0));

        // 动画开始时的初始值
        float startDissolveAmount = -1f;
        float startEdgeSoftness = 0.05f;
        float endDissolveAmount = 1f; // 目标溶解值
        float elapsedTime = 0f;

        // 逐渐变化这两个属性
        while (elapsedTime < dissolveDuration)
        {
            float dissolveAmount = Mathf.Lerp(startDissolveAmount, endDissolveAmount, elapsedTime / dissolveDuration);
            // 更新材质的属性
            _instanceFogMat.SetFloat("_DissolveAmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 确保结束时的值是准确的
        _instanceFogMat.SetFloat("_DissolveAmount", endDissolveAmount);
        
        _resources.ToList().ForEach(r => r.IsLocked = false);
        _arrows.ToList().ForEach(r => r.IsLocked = false);
        IsFogUnLocked = true;
    }
    #endregion
}
