using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Item : DragBase
{
    public ItemData _data;
    
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangeItem; // 先退订旧Data的事件
        
        _data = data as ItemData;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangeItem;
            OnDataChangeItem(); // 立即刷新一遍
        }
    }
    
    [Header("重要属性")]
    public int Rare;
    public ItemAttribute Attribute;
    
    [Header("表现相关")]
    public Image ItemSprite;
    public Image ItemBGInBag;
    public Image ItemBGInElement;
    public Color RareColor;
    
    public Color Rare1;
    public Color Rare2;
    public Color Rare3;
    public Color Rare4;
    
    internal override void VOnDrag()
    {
        //在拖动中把Item显示后面的背景图关掉
        ItemBGInBag.gameObject.SetActive(false);
        ItemBGInElement.gameObject.SetActive(false);
    }
    
    public override void OnDropEmptySlot(SlotBase targetSlot)
    {
        //清除旧的Slot信息
        //SlotManager.ClearSlotByID(SlotID,SlotType.BagSlot);
        //同步新的Slot信息，这一步会改变SlotID,所以放在后面
        //CurSlot.SOnDrop(gameObject);
        //数据层同步
        //MainRoleManager.Instance.MoveItem(this);
        //OnDataChangeGem();
    }
    
    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        ToolTipsInfo curToolTipsInfo = new ToolTipsInfo(_data.Name);
        CurTooltipsSC.SetInfo(curToolTipsInfo);
    }
    
    string GetItemAttriInfo()
    {
        string str = "";
        if (Attribute.waterElement != 0)
            str += $"<color=#32AFE0>水元素: <color=#ECECEC> + {Attribute.waterElement}\n";
        if (Attribute.fireElement != 0)
            str += $"<color=#FF4F00>火元素: <color=#ECECEC> + {Attribute.fireElement}\n";
        if (Attribute.thunderElement != 0)
            str += $"<color=#8927B5>雷元素: <color=#ECECEC> + {Attribute.thunderElement}\n";
        if (Attribute.lightElement != 0)
            str += $"<color=#E7D889>光元素: <color=#ECECEC> + {Attribute.lightElement}\n";
        if (Attribute.darkElement != 0)
            str += $"<color=#2F985B>暗元素: <color=#ECECEC> + {Attribute.darkElement}\n";
        
        if (Attribute.extraWaterDamage != 0)
            str += $"<color=#32AFE0>额外水伤害: <color=#ECECEC> + {Attribute.extraWaterDamage}%\n";
        if (Attribute.extraFireDamage != 0)
            str += $"<color=#FF4F00>额外火伤害: <color=#ECECEC> + {Attribute.extraFireDamage}%\n";
        if (Attribute.extraThunderDamage != 0)
            str += $"<color=#8927B5>额外雷伤害: <color=#ECECEC> + {Attribute.extraThunderDamage}%\n";
        if (Attribute.extraLightDamage != 0)
            str += $"<color=#E7D889>额外光伤害: <color=#ECECEC> + {Attribute.extraLightDamage}%\n";
        if (Attribute.extraDarkDamage != 0)
            str += $"<color=#2F985B>额外暗伤害: <color=#ECECEC> + {Attribute.extraDarkDamage}%\n";
        
        str += $"<color=#ECECEC>总伤害: <color=#ECECEC> + {Attribute.maxDamage}%\n";
        return str;
    }
    #endregion

    #region 同步各种数据
    public void OnDataChangeItem()
    {
        //同步数据
        ItemJson itemDesignData = TrunkManager.Instance.GetItemJson(_data.ID);
        Attribute = itemDesignData.Attribute;
        Rare = itemDesignData.Rare;
        //Debug.LogError(PathConfig.GetItemPath(gemDesignData.ImageName));
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetItemPath(itemDesignData.ImageName));
        //同步背景形状
        switch (_data.CurSlot.SlotType)
        {
            case SlotType.BagSlot:
                ItemBGInBag.gameObject.SetActive(true);
                ItemBGInElement.gameObject.SetActive(false);
                break;
            case SlotType.ElementSlot:
                ItemBGInElement.gameObject.SetActive(true);
                ItemBGInBag.gameObject.SetActive(false);
                break;
        }

        //同步稀有度颜色
        switch (Rare)
        {
            case 1:
                RareColor = Rare1;
                break;
            case 2:
                RareColor = Rare2;
                break;
            case 3:
                RareColor = Rare3;
                break;
            case 4:
                RareColor = Rare4;
                break;
        }

        if (ItemBGInBag != null) ItemBGInBag.color = RareColor;
        if (ItemBGInElement != null) ItemBGInElement.color = RareColor;
    }
    
    #endregion
}
