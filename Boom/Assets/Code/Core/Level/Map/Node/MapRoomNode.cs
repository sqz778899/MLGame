using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapRoomNode : MonoBehaviour
{
    [Header("重要属性")]
    public int RoomID;

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
    ArrowNode[] _arrows;     //全部的箭头
    MapNodeBase[] _resources; //全部的资源

    void Awake()
    {
        _arrows = GetComponentsInChildren<ArrowNode>();
        _resources = _resRoot.GetComponentsInChildren<MapNodeBase>();
        if (RoomFog)
        {
            _instanceFogMat = new Material(RoomFog.material);
            RoomFog.material = _instanceFogMat;
        }
        State = MapRoomState.IsLocked;
    } 

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
    
    private IEnumerator UnlockRoomAnimation()
    {
        // 设置溶解方向
        _instanceFogMat.SetInt("_Flip", (DissolveDir.x < 0 || DissolveDir.y < 0) ? 1 : 0);
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
    }
}
