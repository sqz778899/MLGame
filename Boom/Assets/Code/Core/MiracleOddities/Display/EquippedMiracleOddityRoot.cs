using System.Collections.Generic;
using UnityEngine;

public class EquippedMiracleOddityRoot : MonoBehaviour
{
    public GameObject EquippedMiracleOddityPrefab;
    public Vector3 StartPos;
    public float IconSpacing = 100f;

    private readonly List<GameObject> activeIcons = new();

    void Start()
    {
        GM.Root.InventoryMgr._InventoryData.OnStructureChanged += RefreshIcons;
        RefreshIcons();
    }

    void OnDestroy() => GM.Root.InventoryMgr._InventoryData.OnStructureChanged -= RefreshIcons;

    void RefreshIcons()
    {
        // 清空旧图标
        foreach (GameObject icon in activeIcons)
            Destroy(icon.gameObject);
        activeIcons.Clear();

        // 根据当前装备的道具重新生成
        List<MiracleOddityData> equippedItems = GM.Root.InventoryMgr._InventoryData.EquipMiracleOddities;
        int index = 0;
        for (int i = 0; i < equippedItems.Count; i++)
        {
            MiracleOddityData itemData = equippedItems[i];
            GameObject iconGO = Instantiate(EquippedMiracleOddityPrefab, transform);
            iconGO.SetActive(true);

            MiracleOddityView moView = iconGO.GetComponent<MiracleOddityView>();
            moView.BindingData(itemData);

            RectTransform rt = moView.GetComponent<RectTransform>();
            rt.anchoredPosition = StartPos + new Vector3(index * IconSpacing, 0f, 0f);

            activeIcons.Add(iconGO);
            index++;
        }
    }
}