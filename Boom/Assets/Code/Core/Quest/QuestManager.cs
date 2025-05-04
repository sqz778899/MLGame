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
        PlayerManager.Instance._QuestData.UpdateQuestState(questID, QuestState.InProgress);
        //2)读取天赋
        PlayerManager.Instance.LoadTalent();
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
        {
            if (IsTestMode)
                mapManager.InitializeMap(new Quest(TestMapID));
            else
                mapManager.InitializeMap(currentQuest);
        }
    }

    public void CompleteQuest(bool IsMidway = false)
    {
        //1)更新任务数据
        if (!IsMidway)//如果中途返回则不更新任务状态
            PlayerManager.Instance._QuestData.UpdateQuestState(
                currentQuest.ID, QuestState.Completed);
        //2）清理战斗数据
        BattleManager.Instance.battleData.ClearData();
        PlayerManager.Instance.ClearPlayerData();
        InventoryManager.Instance.ClearInventoryData();
        
        SaveManager.SaveFile();
        //3）加载固定的游戏场景，返回城镇
        MSceneManager.Instance.LoadScene(1);
    }
    
    public void FailQuest()
    {
        //1)更新任务数据
        PlayerManager.Instance._QuestData.UpdateQuestState(
            currentQuest.ID, QuestState.Failed);
        //2）清理战斗数据
        BattleManager.Instance.battleData.ClearData();
        SaveManager.SaveFile();
        //3）加载固定的游戏场景，返回城镇
        MSceneManager.Instance.LoadScene(1);
    }
    
    #region 数据加载卸载
    public void InitData() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
    #endregion
}