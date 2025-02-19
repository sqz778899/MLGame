using System.Collections.Generic;
using System.Linq;
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
        GemJson gemDesignJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == GemID) ?? new GemJson();
        FloatingGetItemText(gemDesignJson.Name + 1);
        MainRoleManager.Instance.AddGem(GemID);
        Debug.Log("Open Tressure Box !!");
    }
}