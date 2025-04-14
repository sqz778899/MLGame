using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Action OnDialogueEnd;
    [Header("基本挂件")]
    public GameObject DialogueRoot;
    public TextMeshProUGUI Content;
    
    public TextMeshProUGUI NameLeft;
    public TextMeshProUGUI NameRight;
    
    public Image PLeft;
    public Image PRight;

    [Header("左右需要隐藏的全部GO")]
    public List<GameObject> LeftGOs;
    public List<GameObject> RightGOs;

    //重要数据
    Dictionary<int, DiaSingle> _curDialogueDict;
    Dictionary<string, Portrait> _spriteDict;
    DiaState _curState;
    int nextDialogueID;

    void Awake()
    {
        _curState = DiaState.Clossed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NextDialogue();
    }

    public void NextDialogue()
    {
        if (_curState == DiaState.Start)
        {
            if (nextDialogueID == -1)//对话结束
            {
                CloseDialogue();
                return;
            }
            UpdateNextDialogue(nextDialogueID);
        }
    }
    
    void CloseDialogue()
    {
        UIManager.Instance.IsLockedClick = false;
        _curState = DiaState.Clossed;
        DialogueRoot.SetActive(false);
        Content.text = "";
        NameLeft.text = NameRight.text = "";
        PLeft.sprite = PRight.sprite = null;
        _curDialogueDict?.Clear();
        OnDialogueEnd?.Invoke();
    }

    //加载当前对话块信息
    public void LoadDialogue(string loadDiaName,bool isHidePotrait = false)
    {
        if (isHidePotrait)
        {
            PLeft.enabled = false;
            PRight.enabled = false;
        }
        else
        {
            PLeft.enabled = true;
            PRight.enabled = true;
        }
        UIManager.Instance.IsLockedClick = true;
        _curState = DiaState.Start;
        DialogueRoot.SetActive(true);
        if(_curDialogueDict == null)
            _curDialogueDict = new Dictionary<int, DiaSingle>();
        if(_spriteDict == null)
            _spriteDict = DialoguePortraitConfig.Instance.GetDictionary();
        Dictionary<string,List<DiaSingle>> designJsons = TrunkManager.Instance.DialogueDesignJsons;
        List<DiaSingle> curDias = designJsons[loadDiaName];
        
        _curDialogueDict.Clear();
        foreach (var each in curDias)
            _curDialogueDict.Add(each.ID, each);
        
        //开始第一段对话
        UpdateNextDialogue(0);
    }

    void UpdateNextDialogue(int dialogueId)
    {
        if (!_curDialogueDict.TryGetValue(dialogueId, out DiaSingle curDia))
        {
            Debug.LogError($"对话 ID {dialogueId} 不存在！");
            return;
        }
        
        Content.text = TextProcessor.Parse(curDia.Content,false);//标记的地方换色处理
        if (curDia.IsLeft == 1)
        {
            LeftGOs.ForEach(each => each.SetActive(true));
            RightGOs.ForEach(each => each.SetActive(false));
            NameLeft.text = curDia.Name;
            PLeft.sprite = _spriteDict[curDia.Name].PortraitSprite;
            RectTransform PLeftRect = PLeft.GetComponent<RectTransform>();
            PLeftRect.sizeDelta= _spriteDict[curDia.Name].PortraitSize;
            PLeftRect.anchoredPosition = new Vector2(PLeftRect.anchoredPosition.x, _spriteDict[curDia.Name].PortraitY);
        }
        else
        {
            LeftGOs.ForEach(each => each.SetActive(false));
            RightGOs.ForEach(each => each.SetActive(true));
            NameRight.text = curDia.Name;
            PRight.sprite = _spriteDict[curDia.Name].PortraitSprite;
            RectTransform PLeftRect = PRight.GetComponent<RectTransform>();
            PLeftRect.sizeDelta= _spriteDict[curDia.Name].PortraitSize;
            PLeftRect.anchoredPosition = new Vector2(PLeftRect.anchoredPosition.x, _spriteDict[curDia.Name].PortraitY);
        }
        nextDialogueID = curDia.NextIdex;
    }
}
