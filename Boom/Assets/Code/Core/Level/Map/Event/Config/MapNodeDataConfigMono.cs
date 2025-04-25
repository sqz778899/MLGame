using UnityEngine;
using UnityEngine.Serialization;

public class MapNodeDataConfigMono : MonoBehaviour
{
    [Header("基础信息")]
    public int ID;
    public string NodeName;
    [TextArea(2, 5)]
    public string Desc;

    [FormerlySerializedAs("EventType")] [Header("事件类型")]
    public MapEventType _MapEventType = MapEventType.TreasureBox;

    [Header("显示控制")]
    public bool StartLocked = false;
    public bool StartAsTriggered = false;
    
    // 每种事件配置
    public GoldPileConfigData GoldPileConfig;
    public TreasureBoxConfigData TreasureBoxConfig;
    public BulletEventConfigData BulletEventConfig;
    public RoomKeyConfigData RoomKeyConfig;
    
    public BasicGamblingConfigData BasicGamblingConfig;
    public StoneTabletConfigData StoneTabletConfig;
    public WigglingBoxConfigData WigglingBoxConfig;
    public ShopEventConfigData ShopEventConfig;
    public RoomArrowConfigData RoomArrowConfig;

    public void Awake() => GetComponent<MapNode>().Init(ToRuntimeData());
    
    public MapNodeData ToRuntimeData()
    {
        MapEventRuntimeData runtime = _MapEventType switch
        {
            MapEventType.CoinsPile => GoldPileConfig?.ToRuntimeData(),
            MapEventType.TreasureBox => TreasureBoxConfig?.ToRuntimeData(),
            MapEventType.Bullet => BulletEventConfig?.ToRuntimeData(),
            MapEventType.RoomKey => RoomKeyConfig?.ToRuntimeData(),
            MapEventType.StoneTablet => StoneTabletConfig?.ToRuntimeData(),
            MapEventType.MysticalInteraction => WigglingBoxConfig?.ToRuntimeData(),
            MapEventType.Shop => ShopEventConfig?.ToRuntimeData(),
            MapEventType.BasicGambling => BasicGamblingConfig?.ToRuntimeData(),
            MapEventType.RoomArrow =>RoomArrowConfig?.ToRuntimeData(),
            _ =>null,
        };

        return new MapNodeData(ID, NodeName, Desc, _MapEventType, runtime);
    }
}