using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGUI : MonoBehaviour
{
    public Image TutorialBG;
    public ParticleSystem FXArrow;
    public GameObject QuestRoot;
    public Button QuestGO;
    [Header("图书馆解锁相关资源")]
    public TalentNode _TalentNode;

    void Awake()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var each in particleSystems)
        {
            each.Clear();
            each.Stop();
        }
    }
}