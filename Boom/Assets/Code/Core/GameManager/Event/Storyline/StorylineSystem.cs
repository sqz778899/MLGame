using System.Collections.Generic;
using UnityEngine;

public class StorylineSystem: MonoBehaviour
{
    public List<StorylineNodeStateData> StorylineSaveData;
    List<StorylineNode> nodes = new();
    void Start()
    {
        if (GM.Root.IsSkipStorylineMode)
            return;
        LoadStorylineData(); // 注册所有剧情节点，并读取存档，同步节点状态
    }

    void Update()
    {
        foreach (var node in nodes)
        {
            if (node.CanTrigger())
            {
                node.State = StorylineState.Active;
                node.OnStart?.Invoke();
            }
        }
    }

    public void MarkNodeComplete(int id)
    {
        var node = nodes.Find(n => n.ID == id);
        if (node == null) return;
        node.State = StorylineState.Completed;
        node.OnComplete?.Invoke();//调用后处理逻辑
    }

    /// 注册所有剧情节点
    void RegisterAllStorylines()
    {
        var builders = new List<IStorylineNodeBuilder>
        {
            new Chapter1Step1(),
            new Chapter1Step2(),
            new Chapter1Step3()
        };
        
        foreach (var builder in builders)
            nodes.Add(builder.Build());
    }

    #region 存档读档
    public void LoadStorylineData()
    {
        RegisterAllStorylines();
        foreach (var data in StorylineSaveData)
        {
            var node = nodes.Find(n => n.ID == data.ID);
            if (node == null) continue;
            node.State = data.State;
        }
    }
    
    public List<StorylineNodeStateData> SaveStorylineData()
    {
        List<StorylineNodeStateData> storylineSaveData = new List<StorylineNodeStateData>();
        foreach (var node in nodes)
        {
            storylineSaveData.Add(new StorylineNodeStateData
            {
                ID = node.ID,
                State = node.State,
                CustomDataJson = "{}"
            });
        }
        return storylineSaveData;
    }
    #endregion

    #region 单例的加载卸载
    public static StorylineSystem Instance { get; private set; }
    public void InitData()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion
}