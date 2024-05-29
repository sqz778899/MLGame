public class REvent:GUIBase
{
    public EventNode CurEventNode;
    public override void QuitSelf()
    {
        base.QuitSelf();
        CurEventNode.QuitEvent();
    }
}