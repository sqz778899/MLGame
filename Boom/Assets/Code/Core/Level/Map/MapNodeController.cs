using UnityEngine;

public class MapNodeController
{
    private MapNodeData _data;
    public MapNodeView _view;

    public MapNodeController(MapNodeData data, MapNodeView view)
    {
        _data = data;
        _view = view;
    }

    public void OnInteract()
    {
        if (_data.IsLocked) return;
        
        if (_data.IsTriggered && !EventTypeRules.IsRepeatable(_data.EventType))
        {
            _view.ShowFloatingText("已经没什么好拿的了");
            return;
        }

        var handler = MapEventHandlerRegistry.GetHandler(_data.EventType);
        if (handler != null)
            handler.Handle(_data, _view);
        else
            _view.ShowFloatingText("这里什么也没有");
    }

    public void Locked() => _data.IsLocked = true;
    public void UnLocked() => _data.IsLocked = false;
}