using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    public Sprite TreasureOpened;
    public void OpenTreasureBox()
    {
        //抽宝石
        spriteRenderer.sprite = TreasureOpened;
        //GetBulletEntryPB
        //UIManager.Instance.SetOtherUIPause();
        Debug.Log("Open Tressure Box !!");
    }
}