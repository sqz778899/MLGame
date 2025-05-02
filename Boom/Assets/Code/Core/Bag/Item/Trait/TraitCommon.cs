using UnityEngine;


//定义事件接口，地图类特质触发
public class TraitData:ITooltipBuilder
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Desc { get; private set; }
    public DropedRarity Rarity { get; private set; }
    public string Flavor { get; private set; }
    public Sprite Icon { get; private set; }

    public TraitData(int id)
    {
        ID = id;
        TraitJson json = TrunkManager.Instance.GetTraitJson(id);
        Name = json.Name;
        Desc = json.Desc;
        Rarity = json.Rarity;
        Flavor = json.Flavor;
        Icon = ResManager.instance.GetTraitIcon(id); // 注意资源路径区分正负面
    }
    
    // 帮 Tooltips 用
    public ToolTipsInfo BuildTooltip()
    {
        return new ToolTipsInfo(Name, 0, Flavor, ToolTipsType.Trait, Rarity);
    }
}