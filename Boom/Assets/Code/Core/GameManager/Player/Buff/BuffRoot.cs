using UnityEngine;
using System.Collections.Generic;

public class BuffRoot : MonoBehaviour
{
    [Header("Buff图标Prefab")]
    public GameObject BuffIconPrefab;

    [Header("Buff图标间距")]
    public Vector2 StartPos;
    public float IconSpacing = 100f;

    readonly List<BuffIcon> activeIcons = new();
    PlayerData _playerData => GM.Root.PlayerMgr._PlayerData;
    void Start() => Init();

    public void Init()
    {
        _playerData.OnBuffAdded += AddBuffIcon;
        _playerData.OnBuffRemoved += RemoveBuffIcon;
    }

    void OnDestroy()
    {
        _playerData.OnBuffAdded -= AddBuffIcon;
        _playerData.OnBuffRemoved -= RemoveBuffIcon;
    }

    void AddBuffIcon(IBuffEffect buff)
    {
        GameObject iconGO = Instantiate(BuffIconPrefab, transform);
        iconGO.SetActive(true);
        BuffIcon icon = iconGO.GetComponent<BuffIcon>();
        icon.BindingData(buff.Data);
        activeIcons.Add(icon);
        Reflow();
    }

    void RemoveBuffIcon(IBuffEffect buff)
    {
        BuffIcon iconToRemove = activeIcons.Find(icon => icon.Data.ID == buff.Id);
        if (iconToRemove != null)
        {
            activeIcons.Remove(iconToRemove);
            Destroy(iconToRemove.gameObject);
            Reflow();
        }
    }

    void Reflow()
    {
        for (int i = 0; i < activeIcons.Count; i++)
        {
            RectTransform rt = activeIcons[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(StartPos.x + i * IconSpacing, StartPos.y);
        }
    }
}