using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

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
    #endregion
    
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