using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class LevelEdit
{
    #region 渲染
    [TitleGroup("渲染")]
    [Button("设置渲染层级",ButtonSizes.Large),PropertyOrder(1)]
    void SetRenderLayer()
    {
        List<MapRoomNode> nodes = new List<MapRoomNode>();
        foreach (var each in Selection.gameObjects)
        {
            MapRoomNode[] nodeArray = each.GetComponentsInChildren<MapRoomNode>();
            foreach (var eachN in nodeArray)
            {
                if (!nodes.Contains(eachN))
                    nodes.Add(eachN);
            }
        }
        nodes.ForEach(m=> m.SetRenderLayer());
    }
    #endregion

    #region 房间编辑相关
    [TitleGroup("Room编辑")]
    [PropertyOrder(97)]
    [EnumToggleButtons]
    [OnValueChanged("OnDrawArrowGizmoChanged")]
    public OnOff 箭头房间Debug = OnOff.Off;
    public enum OnOff { Off, On }
    
    void OnDrawArrowGizmoChanged()
    {
        ArrowGizmoDrawer.SetEnabled(箭头房间Debug == OnOff.On);
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews(); // 强制刷新 Scene
    }
    [PropertyOrder(97),FoldoutGroup("敌人色板")]public Color NormalEnemy = new Color(161/255f, 161/255f, 161/255f,255);
    [PropertyOrder(97),FoldoutGroup("敌人色板")]public Color EliteEnemy = new Color(152/255f, 38/255f, 38/255f,255);
    [PropertyOrder(97),FoldoutGroup("敌人色板")]public Color Boss = new Color(207/255f, 180/255f, 74/255f,255);
    
    [HorizontalGroup("房间编辑", 0.5f)]
    [Button("房间ID整理", ButtonSizes.Large), PropertyOrder(97)]
    void SetRoomID()
    {
        GameObject Root = Selection.gameObjects[0];
        if (!Root.name.StartsWith("P_Map")) return;
        MapRoomNode[] allRoom = Root.GetComponentsInChildren<MapRoomNode>();
        int IDStart = 2;
        for (int i = 0; i < allRoom.Length; i++)
        {
            MapRoomNode curRoom = allRoom[i];
            if (curRoom.IsStartRoom)
            {
                curRoom.RoomID = 1;
                curRoom.State = MapRoomState.Unlocked;
            }
            else
            {
                curRoom.RoomID = IDStart+ i;
                curRoom.State = MapRoomState.IsLocked;
            }

            SetResIDByTagsAndDropTemplate(curRoom.gameObject);
        }
    }

    [HorizontalGroup("房间编辑", 0.5f)]
    [Button("箭头房间绑定", ButtonSizes.Large), PropertyOrder(97)]
    void SetArrowBindRoom()
    {
        GameObject arrowRoot = Selection.activeGameObject;
        MapNodeDataConfigMono[] arrowNodes = arrowRoot.GetComponentsInChildren<MapNodeDataConfigMono>(true);
        foreach (MapNodeDataConfigMono each in arrowNodes)
            MEditTool.BindArrowTargetRoom(each);
    }

    [PropertyOrder(97)]
    [HorizontalGroup("怪物颜色", 0.5f)]
    [Button(ButtonSizes.Large, Name = "精英怪")]
    void SetEliteEnemy()
    {
        GameObject arrowGO = Selection.activeGameObject;
        if (!arrowGO.name.StartsWith("Arrow")) return;
        
        SpriteRenderer arrowRenderer = arrowGO.GetComponentInChildren<SpriteRenderer>();
        arrowRenderer.color = EliteEnemy;
    }
    
    [PropertyOrder(97)]
    [HorizontalGroup("怪物颜色",0.5f)]
    [Button(ButtonSizes.Large, Name = "Boss")]
    void SetBoss()
    {
        GameObject arrowGO = Selection.activeGameObject;
        if (!arrowGO.name.StartsWith("Arrow")) return;
        
        SpriteRenderer arrowRenderer = arrowGO.GetComponentInChildren<SpriteRenderer>();
        arrowRenderer.color = Boss;
    }

    #region 编辑敌人配置相关
    public class EnemyEditEntry
    {
        [InlineProperty, OnValueChanged("OnEdited")]
        public EnemyConfigData EnemyConfig;

        [HideInInspector]
        public Action OnValueChanged;

        void OnEdited() => OnValueChanged?.Invoke();
    }

    [TitleGroup("房间内敌人配置预览")]
    [EnumToggleButtons, PropertyOrder(98)]
    public OnOff 房间敌人预览开关 = OnOff.Off;
    
    [TableList(ShowIndexLabels = true), ShowInInspector, PropertyOrder(98)]
    public List<EnemyEditEntry> PreviewEnemyEntries = new();

    [OnInspectorInit]
    void SyncEnemyConfigsFromSelection()
    {
        Selection.selectionChanged -= DisplyEnemyConfig; // 避免重复注册
        Selection.selectionChanged += DisplyEnemyConfig;
    }

    void DisplyEnemyConfig()
    {
        if (房间敌人预览开关 != OnOff.On) return;
        PreviewEnemyEntries.Clear();
        foreach (var go in Selection.gameObjects)
        {
            var monoList = go.GetComponentsInChildren<MapNodeDataConfigMono>(true);
            foreach (var each in monoList)
            {
                if (each._MapEventType != MapEventType.RoomArrow || each.RoomArrowConfig.ArrowType != RoomArrowType.Fight)
                    continue;

                var configRef = each; // 捕获房间配置引用

                var enemyConfigCopy = configRef.RoomArrowConfig.BattleConfig;
                var entry = new EnemyEditEntry
                {
                    EnemyConfig = enemyConfigCopy
                };

                // 回调绑定（此时 entry 已构造完成）
                entry.OnValueChanged = () =>
                {
                    configRef.RoomArrowConfig.BattleConfig = entry.EnemyConfig;
                    EditorUtility.SetDirty(configRef);
                };

                PreviewEnemyEntries.Add(entry);
            }
        }
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }
    #endregion
    
    [PropertyOrder(98)]
    [Button("测试伪随机掉落")]
    [HorizontalGroup("伪随机", 0.5f)]
    public void TestPseudoRandomDrop()
    {
        int testSeed = 12345; // 测试种子
        string poolName = "NormalLoot"; // 测试池名
        int testCount = 500; // 测试次数
        List<string> testTags = new List<string> { "Bone", "Alchemy" }; // 假设杂物Tag

        Dictionary<string, int> dropStat = new();

        for (int i = 0; i < testCount; i++)
        {
            // 模拟每次不同节点ID扰动
            int fakeMapNodeID = i;

            DropedObjEntry result = DropTableService.Draw(poolName, fakeMapNodeID, testTags);
            if (result == null) continue;

            string key = $"{result.DropedCategory}_{result.ID}";
            if (!dropStat.ContainsKey(key))
                dropStat[key] = 0;
            dropStat[key]++;
        }

        Debug.Log($"====== 测试结果 [{poolName}]，总次数 {testCount} ======");
        foreach (var pair in dropStat.OrderByDescending(p => p.Value))
        {
            Debug.Log($"{pair.Key} : {pair.Value} 次，占比 {(pair.Value * 100f / testCount):F1}%");
        }
    }
    
    [PropertyOrder(98)]
    [Button("测试大池子掉落分布")]
    [HorizontalGroup("伪随机", 0.5f)]
    public void TestRoomNodePseudoRandomOrder()
    {
        int testSeed = 12345; // 地图种子
        int nodeCount = 20;   // 房间内节点数量
        string dropPoolName = "BasicGambling"; // 大池子名字

        // 大池子的权重
        Dictionary<string, int> weights = new Dictionary<string, int>
        {
            { "Empty", 30 },
            { "BuffChance", 20 },
            { "DebuffChance", 20 },
            { "NormalLoot", 15 },
            { "MetaResource", 10 },
            { "RareLoot", 5 }
        };

        List<string> resultList = new List<string>();
        Dictionary<string, int> resultStat = new Dictionary<string, int>();

        Debug.Log($"====== 房间内伪随机掉落顺序测试，节点数量 {nodeCount} ======");

        for (int i = 0; i < nodeCount; i++)
        {
            int nodeID = 1000 + i; // 模拟MapNode ID
            int localSeed = testSeed + nodeID * 17000;
            string bucketKey = $"weaponrack_{nodeID}";
            string result = ProbabilityService.Draw(bucketKey, weights, localSeed);

            resultList.Add(result);

            if (!resultStat.ContainsKey(result))
                resultStat[result] = 0;
            resultStat[result]++;

            // 打印顺序
            Debug.Log($"第 {i + 1} 次 翻找 -> {result}");
        }

        Debug.Log($"====== 最终掉落统计结果 ======");
        foreach (var pair in resultStat.OrderByDescending(p => p.Value))
        {
            Debug.Log($"{pair.Key} : {pair.Value} 次，占比 {(pair.Value * 100f / nodeCount):F1}%");
        }
    }


    #endregion
    

    public class TagTableJson
    {
        public string Tag;            // 标签
        public int FixedID;           // 固定ID，用于伪随机池分组
        public int EmptyChance;       // 空掉落权重
        public int KeyChance;         // 掉落钥匙权重
        public int BuffChance;        // 临时Buff权重
        public int DeBuffChance;        // 临时Buff权重
        public int NormalLoot;        // 普通掉落权重
        public int MetaResource;      // 局外资源权重（黑曜石、秘银）
        public int RareLoot;          // 稀有掉落权重
    }
    
 
    void SetResIDByTagsAndDropTemplate(GameObject root)
    {
        // 读取Tag表
        List<TagTableJson> tagTable = JsonConvert.DeserializeObject<List<TagTableJson>>(File.ReadAllText(PathConfig.TagDesignJson));
        MapNodeDataConfigMono[] configMonos = root.GetComponentsInChildren<MapNodeDataConfigMono>(true);

        foreach (var eachConfigMono in configMonos)
        {
            if (eachConfigMono._MapEventType != MapEventType.BasicGambling)
                continue;
            
            List<string> tags = eachConfigMono.ClutterTags;
            if (tags == null || tags.Count == 0)
                tags = new List<string> { "Default" };

            string mainTag = tags[0];

            // 查Tag表
            TagTableJson template = tagTable.FirstOrDefault(t => t.Tag == mainTag);
            if (template == null)
            {
                Debug.LogWarning($"找不到Tag模板：{mainTag}，跳过！");
                continue;
            }

            // 赋ID
            eachConfigMono.ID = template.FixedID;

            // 初始化大池子配置（只针对BasicGambling类型）
            if (eachConfigMono._MapEventType == MapEventType.BasicGambling)
            {
                if (eachConfigMono.BasicGamblingConfig == null)
                    eachConfigMono.BasicGamblingConfig = new BasicGamblingConfigData();

                eachConfigMono.BasicGamblingConfig.EmptyChance = template.EmptyChance;
                eachConfigMono.BasicGamblingConfig.KeyChance = template.KeyChance;
                eachConfigMono.BasicGamblingConfig.TempBuffChance = template.BuffChance;
                eachConfigMono.BasicGamblingConfig.TempDebuffChance = template.DeBuffChance;
                eachConfigMono.BasicGamblingConfig.NormalLootChance = template.NormalLoot;
                eachConfigMono.BasicGamblingConfig.MetaResourceChance = template.MetaResource;
                eachConfigMono.BasicGamblingConfig.RareLootChance = template.RareLoot;
            }

            EditorUtility.SetDirty(eachConfigMono);
        }

        Debug.Log($"统一设置完毕，共处理 {configMonos.Length} 个节点。");
    }
    
    #region 雾
    GameObject MapRoot;
    [TitleGroup("雾")]
    [EnumToggleButtons,PropertyOrder(99)]
    [OnValueChanged("OpenFogOnOff")]
    public OnOff 雾开关 = OnOff.Off;
    
    void OpenFogOnOff()
    {
        if (!MapRoot)
            MapRoot = GameObject.Find("MapRoot");
        MapRoomNode[] allRoom = MapRoot.GetComponentsInChildren<MapRoomNode>();
        allRoom.Where(each => each.RoomFog != null).ToList() // 将结果转换为列表
            .ForEach(each => each.RoomFog.gameObject.SetActive(雾开关 == OnOff.On)); // 执行操作

        foreach (var each in allRoom)
        {
            if (each.RoomFog == null)
            {
                Transform[] allChildren = each.GetComponentsInChildren<Transform>(true);
                Transform roomFog = allChildren.FirstOrDefault(each => each.gameObject.name.StartsWith("RoomFog"));
                if (roomFog != null)
                    each.RoomFog = roomFog.GetComponent<SpriteRenderer>();
            }
            if(each.RoomFog ==null)continue;
            
            each.RoomFog.sortingLayerID =  SortingLayer.NameToID("Fog");
            each.RoomFog.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Res/Shader/CommonMaterial/RoomFogDissolve.mat");
        }
        EditorUtility.SetDirty(MapRoot);
    }
    #endregion

    #region 关卡测试开关
    [TitleGroup("关卡测试开关")]
    [EnumToggleButtons,PropertyOrder(100)]
    [OnValueChanged("SetTestMode")]
    public OnOff 关卡测试开关 = OnOff.Off;

    void SetTestMode()
    {
        GameObject MM = GameObject.Find("MapManager");
        MM.GetComponent<MapManager>().IsTest = (关卡测试开关 == OnOff.On);
        GameObject QuestManager = GameObject.Find("QuestManager(singleton)");
        QuestManager.GetComponent<QuestManager>().IsTestMode = (关卡测试开关 == OnOff.On);
        EditorUtility.SetDirty(MM);
        EditorUtility.SetDirty(QuestManager);
    }
    #endregion
}