using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementZoneRoot:MonoBehaviour
{
    public GameObject ElementZoneIconGO;
    public Vector3 StartPos;
    public float IconSpacing = 100f;
    
    List<ElementZoneIcon> activeIcons = new();
    List<ElementZoneData> activeZones => GM.Root.BattleMgr.elementZoneMgr.ActiveZones;

    public void InitData()
    {
        GM.Root.BattleMgr.elementZoneMgr.OnZoneChanged -= UpdateZoneIcon;
        GM.Root.BattleMgr.elementZoneMgr.OnZoneChanged += UpdateZoneIcon;
    }

    void OnDestroy() => GM.Root.BattleMgr.elementZoneMgr.OnZoneChanged -= UpdateZoneIcon;

    public void UpdateZoneIcon()
    {
        foreach (var icon in activeIcons)
            Destroy(icon.gameObject);
        activeIcons.Clear();

        for (int i = 0; i < activeZones.Count; i++)
        {
            ElementZoneData zone = activeZones[i];
            AddZoneIcon(zone.ElementalType, zone.ElementalInfusionValue);
        }
    }

    public void AddZoneIcon(ElementalTypes type, int value)
    {
        int existCount = activeIcons.Count;
        
        GameObject go = Instantiate(ElementZoneIconGO, transform);
        go.SetActive(true);
        go.transform.localPosition = StartPos - new Vector3(IconSpacing * existCount, 0, 0);

        ElementZoneIcon iconComp = go.GetComponent<ElementZoneIcon>();
        iconComp.InitData(type);
        iconComp.AddValue(value);

        activeIcons.Add(iconComp);
    }
}