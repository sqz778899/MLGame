using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class GMTools
{
    [Title("魔尘&&金币添加")]
    public int v;
    [Button("魔尘添加",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("魔尘&&金币添加")]
    void AddDust()
    {
        PlayerManager.Instance._PlayerData.ModifyMagicDust(10000);
    }
    [Button("金币添加",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("魔尘&&金币添加")]
    void AddCoins()
    {
        PlayerManager.Instance._PlayerData.ModifyCoins(1000);
    }
    
    [Button("钥匙添加",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("魔尘&&金币添加")]
    void AddKeys()
    {
        PlayerManager.Instance._PlayerData.ModifyRoomKeys(1);
    }
    
    [Title("剧情解锁程度"),PropertyOrder(1)]
    public int progress;
    [Button("剧情解锁程度",ButtonSizes.Large),PropertyOrder(1)]
    void SetStoryLine()
    {
        PlayerManager.Instance._QuestData.MainStoryProgress = progress;
    }
    
    [Title("道具测试")]
    [PropertyOrder(2)]
    public int ItemID;
    [Button("获得道具",ButtonSizes.Large),PropertyOrder(2)]
    void AddItem()
    {
        InventoryManager.Instance.AddItemToBag(ItemID);
    }
    [Title("宝石测试")]
    [PropertyOrder(3)]
    public int GemID;
    [Button("获得宝石",ButtonSizes.Large),PropertyOrder(3)]
    void AddGem()
    {
        InventoryManager.Instance.AddGemToBag(GemID);
    }
}
