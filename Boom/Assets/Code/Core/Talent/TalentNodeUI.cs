using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentNodeUI : MonoBehaviour
{
    public string nodeId;
    public TextMeshProUGUI label;
    public Image icon;
    public Button button;

    /*public void Init(TalentNodeData data) {
        nodeId = data.id;
        label.text = data.displayName;
        UpdateVisualState(data);
    }

    void UpdateVisualState(TalentNodeData data) {
        // 控制颜色 / 发光 / 锁定状态等
    }*/
}
