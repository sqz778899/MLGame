using System;
using TMPro;
using UnityEngine;

public class WallwalkSkill: MonoBehaviour
{
    [Header("资产")]
    public TextMeshProUGUI txtWalkcount;
    public GameObject WallwalkUILeft;
    public GameObject WallwalkUIRight;

    PlayerData _playerData => GM.Root.PlayerMgr._PlayerData;

    void Start()
    {
        _playerData.OnWallwalkSkillCountChange += RefreshUI;
        GM.Root.HotkeyMgr.OnEscapePressed += CloseUI;//注册快捷键
        RefreshUI();
    }
    void OnDestroy()
    {
        _playerData.OnWallwalkSkillCountChange -= RefreshUI;
        GM.Root.HotkeyMgr.OnEscapePressed -= CloseUI;//注册快捷键
    }

    public void OnClickWallwalk()
    {
        if (!_playerData.CanUseWallwalkSkill)
        {
            FloatingTextFactory.CreateWorldText("技能次数已用完");
            return;
        }
        MapRoomNode curRoom = GM.Root.BattleMgr._MapManager.GetCurRoomNode();
        if (curRoom.hideLeft == null && curRoom.hideRight == null)
        {
            FloatingTextFactory.CreateWorldText("没有墙壁可以穿越");
            return;
        }

        if (curRoom.hideLeft != null)
            WallwalkUILeft.SetActive(!WallwalkUILeft.activeSelf);
        if (curRoom.hideRight != null)
            WallwalkUIRight.SetActive(!WallwalkUILeft.activeSelf);
    }

    public void OnWallwalk(bool isRight)
    {
        MapRoomNode curRoom = GM.Root.BattleMgr._MapManager.GetCurRoomNode();
        _playerData.UseWallwalkSkill();
        if (isRight)
            curRoom.hideRight.SetActive(true);
        else
            curRoom.hideLeft.SetActive(true);

        CloseUI();
    }

    void CloseUI()
    {
        WallwalkUILeft.SetActive(false);
        WallwalkUIRight.SetActive(false);
    }
    
    void RefreshUI() => txtWalkcount.text = $"x{_playerData.WallwalkSkillCount}";
}