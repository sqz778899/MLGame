using System.Collections.Generic;
using UnityEngine;

public class EquippedItemIconRoot : MonoBehaviour
{
    public GameObject EquippedItemIconPrefab;
    public GameObject TraitIconPrefab;
    public Vector3 StartPos;
    public float IconSpacing = 100f;

    private readonly List<GameObject> activeIcons = new();

    void Start()
    {
        GM.Root.InventoryMgr._InventoryData.OnEquipItemChanged += RefreshIcons;
        RefreshIcons();
    }

    void OnDestroy() => GM.Root.InventoryMgr._InventoryData.OnEquipItemChanged -= RefreshIcons;

    void RefreshIcons()
    {
        // 清空旧图标
        foreach (GameObject icon in activeIcons)
            Destroy(icon.gameObject);
        activeIcons.Clear();

        // 根据当前装备的道具重新生成
        List<ItemData> equippedItems = GM.Root.InventoryMgr._InventoryData.EquipItems;
        int index = 0;
        for (int i = 0; i < equippedItems.Count; i++)
        {
            ItemData itemData = equippedItems[i];
            GameObject iconGO = Instantiate(EquippedItemIconPrefab, transform);
            iconGO.SetActive(true);

            EquippedItemIcon icon = iconGO.GetComponent<EquippedItemIcon>();
            icon.BindingData(itemData);

            RectTransform rt = icon.GetComponent<RectTransform>();
            rt.anchoredPosition = StartPos + new Vector3(index * IconSpacing, 0f, 0f);

            activeIcons.Add(iconGO);
            index++;
        }
        
        // 3. 添加特质图标
        List<TraitData> traits = GM.Root.InventoryMgr._ItemEffectMrg.GetCurrentSynergiesInfos();
        foreach (var traitInfo in traits)
        {
            GameObject iconGO = Instantiate(TraitIconPrefab, transform);
            iconGO.SetActive(true);

            TraitIcon icon = iconGO.GetComponent<TraitIcon>();
            icon.BindingData(traitInfo);

            RectTransform rt = icon.GetComponent<RectTransform>();
            rt.anchoredPosition = StartPos + new Vector3(index * IconSpacing, 0f, 0f);

            activeIcons.Add(iconGO);
            index++;
        }
    }
}