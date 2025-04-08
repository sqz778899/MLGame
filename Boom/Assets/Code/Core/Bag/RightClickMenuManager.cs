using System;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuManager:MonoBehaviour
{
    public static RightClickMenuManager Instance { get; private set; }

    [SerializeField] GameObject panelGO; // 整个右键菜单容器
    [SerializeField] Button btnRemove;
    [SerializeField] Button btnDetails;

    GemNew currentGem;

    void Awake()
    {
        Instance = this;
        panelGO.SetActive(false);
        btnRemove.onClick.AddListener(OnClickRemove);
        btnDetails.onClick.AddListener(OnClickDetails);
    }

    public void Show(GemNew gem, Vector2 screenPos)
    {
        currentGem = gem;
        panelGO.SetActive(true);
        panelGO.transform.position = screenPos;
    }

    public void Hide()
    {
        currentGem = null;
        panelGO.SetActive(false);
    }

    void OnClickRemove()
    {
        //currentGem?.HandleRemove(); // GemNew 里实现 HandleRemove()
        Hide();
    }

    void OnClickDetails()
    {
        // 可选：显示详情面板
        Debug.Log("查看详情");
        Hide();
    }
}