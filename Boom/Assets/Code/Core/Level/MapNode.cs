using System;
using TMPro;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public int LevelID;
    public MapNodeState State;
    public GameObject imLocked;
    public GameObject imNode;
    ParticleSystem[] _fx_imNode;
    public GameObject imIsFinish;

    public TextMeshProUGUI txtTitle;

    void Start()
    {
        _fx_imNode = imNode.GetComponentsInChildren<ParticleSystem>();
        ChangeState();
    }

    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
    }

    void ChangeState()
    {
        switch (State)
        {
            case MapNodeState.Locked:
                imLocked.SetActive(true);
                imNode.SetActive(false);
                imIsFinish.SetActive(true);
                break;
            case MapNodeState.UnLocked:
                imLocked.SetActive(false);
                imNode.SetActive(true);
                foreach (var each in _fx_imNode)
                    each.Play();
                imIsFinish.SetActive(false);
                break;
            case MapNodeState.IsFinish:
                imLocked.SetActive(false);
                imNode.SetActive(false);
                imIsFinish.SetActive(true);
                break;
        }
    }

    public void LoadSceneByNode(int SceneID)
    {
        MSceneManager.Instance.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(SceneID);
    }
}
