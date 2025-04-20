using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
public class LevelEdit
{
    #region 渲染
    [Title("渲染"),PropertyOrder(1)]
    public int ss;
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
    
    [Title("房间编辑"),PropertyOrder(97)]
    public int RayDistance = 100;
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
                curRoom.RoomID = 1;
            else
                curRoom.RoomID = IDStart+ i;
        }
    }

    [Button("箭头房间绑定", ButtonSizes.Large), PropertyOrder(97)]
    void SetArrowBindRoom()
    {
        GameObject arrowGO = Selection.gameObjects[0];
        if (!arrowGO.name.StartsWith("Arrow")) return;
        ArrowNode arrowNode = arrowGO.GetComponent<ArrowNode>();
        //
        
    }
    
    #region 雾
    [Title("雾"),PropertyOrder(98)]
    public GameObject MapRoot;
    [Button("雾关闭",ButtonSizes.Large),PropertyOrder(99)]
    [ButtonGroup("雾")]
    void CloseFog()
    {
        if (!MapRoot)
            MapRoot = GameObject.Find("MapRoot");
        MapRoomNode[] allRoom = MapRoot.GetComponentsInChildren<MapRoomNode>();

        allRoom.Where(each => each.RoomFog != null).ToList() // 将结果转换为列表
            .ForEach(each => each.RoomFog.gameObject.SetActive(false)); // 执行操作
        EditorUtility.SetDirty(MapRoot);
    }
    
    [Button("雾开启",ButtonSizes.Large),PropertyOrder(99)]
    [ButtonGroup("雾")]
    void OpenFog()
    {
        if (!MapRoot)
            MapRoot = GameObject.Find("MapRoot");
        MapRoomNode[] allRoom = MapRoot.GetComponentsInChildren<MapRoomNode>();
        allRoom.Where(each => each.RoomFog != null).ToList() // 将结果转换为列表
            .ForEach(each => each.RoomFog.gameObject.SetActive(true)); // 执行操作

        foreach (var each in allRoom)
        {
            if( each.RoomFog==null)continue;
            each.RoomFog.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Res/Shader/CommonMaterial/RoomFogDissolve.mat");
        }
        EditorUtility.SetDirty(MapRoot);
    }
    #endregion

    #region 关卡测试开关
    [Title("关卡测试开关"),PropertyOrder(100)]
    public bool IsLevelTest;
    [Button("关卡测试开",ButtonSizes.Large),PropertyOrder(100)]
    [ButtonGroup("关卡测试开关")]
    void LevelTestOn()
    {
        IsLevelTest = true;
        SetTestMode();
    }
    [Button("关卡测试关闭",ButtonSizes.Large),PropertyOrder(100)]
    [ButtonGroup("关卡测试开关")]
    void LevelTestOff()
    {
        IsLevelTest = false;
        SetTestMode();
    }

    void SetTestMode()
    {
        GameObject MM = GameObject.Find("MapManager");
        MM.GetComponent<MapManager>().IsTest = IsLevelTest;
        GameObject QuestManager = GameObject.Find("QuestManager(singleton)");
        QuestManager.GetComponent<QuestManager>().IsTestMode = IsLevelTest;
        EditorUtility.SetDirty(MM);
        EditorUtility.SetDirty(QuestManager);
    }
    #endregion
}