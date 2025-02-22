using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    [Header("重要功能")]
    public int GemID;
    public Sprite TreasureOpened;
    public bool isOpened = false;
    
    public void OpenTreasureBox()
    {
        if(isOpened) return;
        //抽宝石
        isOpened = true;
        spriteRenderer.sprite = TreasureOpened;
        GemJson gemDesignJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == GemID) ?? new GemJson();
        
        GameObject GemIns = ResManager.instance.CreatInstance(PathConfig.GemTemplate);
        GemIns.GetComponent<Gem>().ID = gemDesignJson.ID;
        GemIns.transform.SetParent(transform,false);
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GemIns.transform.position = startPos;
        GemIns.transform.localScale = Vector3.one*0.7f;
        EPara.StartPos = startPos;
        MEffectManager.CreatEffect(EPara,GemIns,false,()=>FloatingGetItemText($"获得{gemDesignJson.Name}！"));
       
        MainRoleManager.Instance.AddGem(GemID);
    }
}