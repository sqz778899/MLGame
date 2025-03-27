using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;

public class TalentNode : MonoBehaviour
{
    public int ID;
    public TalentData _talentData;
    
    [Header("显示相关")]
    public UIHeighLight _heighLight;
    public Image _mainIcon;
    public Image _normalIcon;
    public Image _lockedIcon;
    public Image _learnedIcon;
    public TextMeshProUGUI _nameText;

    public event Action<int> OnLearned;
    public void InitTalent()
    {
#if UNITY_EDITOR && !APPLICATION_IS_PLAYING
        // 编辑器模式下、非运行时：用设计数据
        if (!Application.isPlaying)
        {
            List<TalentJson> TalentDesignJsons = JsonConvert.DeserializeObject<List<TalentJson>>(File.ReadAllText(PathConfig.TalentDesignJson));
            TalentData designData = TalentDesignJsons
                .Select(j => new TalentData(j.ID))
                .FirstOrDefault(d => d.ID == ID);

            if (designData != null)
                _talentData = designData;
        }
        else
#endif
        {
            // 正常运行时，从玩家数据中加载
            _talentData = PlayerManager.Instance._PlayerData.GetTalent(ID);
        }
        
        _nameText.text = _talentData.Name;
        _heighLight.IsLocked = _talentData.IsLocked; //同步UI状态
        switch (_talentData.Level)
        {
            case 1:
                GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                break;
            case 2:
                break;
            case 3:
                GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;
        }
        UpdateState();
    }
    
    public void OnClickLearn()
    {
        if (_talentData.IsLocked || _talentData.IsLearned) return;
        _talentData.IsLearned = true; // 学习
        UpdateState();
        OnLearned?.Invoke(ID); // 通知 Root
    }
    
    public void UpdateState()
    {
        if (_talentData.IsLocked)
        {
            _heighLight.SetLocked();
            _mainIcon.sprite = _lockedIcon.sprite;
            _learnedIcon.gameObject.SetActive(_talentData.IsLearned);
        }
        else
        {
            _heighLight.IsLocked = false;
            _mainIcon.sprite = _normalIcon.sprite;
            _learnedIcon.gameObject.SetActive(_talentData.IsLearned);
        }

        if (_talentData.IsLearned)
            _heighLight.SetLocked();
    }
}
