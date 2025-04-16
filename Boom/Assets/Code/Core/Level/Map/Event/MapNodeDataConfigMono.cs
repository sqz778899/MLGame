using UnityEngine;

public class MapNodeDataConfigMono : MonoBehaviour
{
    [Header("基础信息")]
    public int ID;
    public string NodeName;
    [TextArea(2, 5)]
    public string Desc;

    [Header("事件类型")]
    public MapEventType EventType = MapEventType.Treasure;

    [Header("显示控制")]
    public bool StartLocked = false;
    public bool StartAsTriggered = false;
    
    // 每种事件配置
    public GoldPileConfigData GoldPileConfig;
    public WeaponRackConfigData WeaponRackConfig;

    public void Start() => GetComponent<MapNode>().Init(ToRuntimeData());
    
    public MapNodeData ToRuntimeData()
    {
        MapEventRuntimeData runtime = EventType switch
        {
            MapEventType.CoinsPile => GoldPileConfig?.ToRuntimeData(),
            MapEventType.WeaponRack => WeaponRackConfig?.ToRuntimeData(),
            _ => null
        };

        return new MapNodeData(ID, NodeName, Desc, EventType, runtime);
    }
}