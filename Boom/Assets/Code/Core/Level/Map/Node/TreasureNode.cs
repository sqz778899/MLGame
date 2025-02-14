using System.Collections.Generic;
using System.Security.Principal;
using DG.Tweening;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    [Header("重要功能")]
    public int GemID;
    public Sprite TreasureOpened;
    public void OpenTreasureBox()
    {
        //抽宝石
        spriteRenderer.sprite = TreasureOpened;
        //GetBulletEntryPB
        //UIManager.Instance.SetOtherUIPause();
        MainRoleManager.Instance.AddGem(GemID);
        Debug.Log("Open Tressure Box !!");
    }
}