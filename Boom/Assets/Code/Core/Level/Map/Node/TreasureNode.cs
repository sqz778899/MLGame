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
        //获得物品跳字
        GameObject textIns = ResManager.instance.CreatInstance(PathConfig.TxtGetItemPB);
        Transform textNode = MainRoleManager.Instance.MainRoleIns.GetComponent<RoleInner>().TextNode;
        textIns.transform.SetParent(textNode.transform,false);
        FloatingDamageText textSc = textIns.GetComponent<FloatingDamageText>();
        
        GemJson gemDesignJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == GemID) ?? new GemJson();
        textSc.AnimateText($"{gemDesignJson.Name} + 1",new Color(218f/255f,218f/255f,218f/255f,1f));
        MainRoleManager.Instance.AddGem(GemID);
        Debug.Log("Open Tressure Box !!");
    }
}