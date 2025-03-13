using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public QuestDatabaseOBJ questDatabase;
    public Quest currentQuest;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // 选择任务并加载对应场景
    public void SelectQuest(string questID)
    {
        Quest quest = questDatabase.GetQuestByID(questID);
        if (quest == null)
        {
            Debug.LogError($"未找到ID为{questID}的任务！");
            return;
        }

        currentQuest = quest;
        LoadQuestScene(quest.SceneName);
    }

    // 加载任务场景
    void LoadQuestScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("场景名为空！");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    // 更新任务状态
    public void UpdateQuestState(QuestState newState)
    {
        if (currentQuest != null)
        {
            currentQuest.State = newState;
            Debug.Log($"任务状态更新：{currentQuest.Name} → {newState}");
        }
    }
}