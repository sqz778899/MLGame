using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
public class LevelEdit
{
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
}