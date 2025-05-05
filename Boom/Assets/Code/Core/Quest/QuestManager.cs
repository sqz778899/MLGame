using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public QuestDatabaseOBJ questDatabase;
    public Quest currentQuest;
    
    [Header("测试模式")]
    public bool IsTestMode;
    public int TestMapID;

    // 选择任务并加载对应地图Prefab
    public void SelectQuest(int questID)
    {
        Quest quest = questDatabase.GetQuestByID(questID);
        if (quest == null)
        {
            Debug.LogError($"未找到任务: {questID}");
            return;
        }

        currentQuest = quest;

        //1)同步数据给PlayerManager
        GM.Root.PlayerMgr._QuestData.UpdateQuestState(questID, QuestState.InProgress);
        //2)读取天赋
        GM.Root.PlayerMgr.LoadTalent();
        SaveManager.SaveFile();
        //3)加载固定的游戏场景
        MSceneManager.Instance.LoadScene(2);
    }

    //异步加载场景完成后调用
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景加载完成后通知MapManager加载对应Prefab
        var mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null && currentQuest != null)
            mapManager.InitializeMap(currentQuest);
        if (IsTestMode)
            mapManager.InitializeMap(new Quest(TestMapID));
    }

    public void CompleteQuest(bool IsMidway = false)
    {
        //1)更新任务数据
        if (!IsMidway)//如果中途返回则不更新任务状态
            GM.Root.PlayerMgr._QuestData.UpdateQuestState(
                currentQuest.ID, QuestState.Completed);
        //2）清理战斗数据
        GM.Root.BattleMgr.battleData.ClearData();
        GM.Root.PlayerMgr.ClearPlayerData();
        GM.Root.InventoryMgr.ClearInventoryData();
        
        SaveManager.SaveFile();
        //3）加载固定的游戏场景，返回城镇
        MSceneManager.Instance.LoadScene(1);
    }
    
    public void FailQuest()
    {
        //1)更新任务数据
        GM.Root.PlayerMgr._QuestData.UpdateQuestState(
            currentQuest.ID, QuestState.Failed);
        //2）清理战斗数据
        GM.Root.BattleMgr.battleData.ClearData();
        SaveManager.SaveFile();
        //3）加载固定的游戏场景，返回城镇
        MSceneManager.Instance.LoadScene(1);
    }
    
    #region 数据加载卸载
    public void InitData() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
    #endregion
}