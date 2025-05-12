public class ElementReactionData : ITooltipBuilder
{
    public int ID;
    public string Name;
    public string Description;
    public ElementReactionType Type;

    public void InitData(ElementReactionType Type)
    {
        ElementReactionJson curJson = TrunkManager.Instance.GetElementReactionJson(Type);
        ID = curJson.ID;
        Name = curJson.Name;
        Description = curJson.Description;
        BuildTooltip();
    }

    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name, 0, Description, ToolTipsType.ElementReaction);
        return info;
    }
}