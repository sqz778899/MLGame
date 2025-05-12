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
            _view.NonFind();
            return;
        }

        IMapEventHandler handler = MapEventHandlerRegistry.GetHandler(_data.EventType);
        if (handler != null)
            handler.Handle(_data, _view);
        else
            _view.ShowFloatingText("这里什么也没有");
    }

    //使用次数相关的走这个
    public void UseCount(int count = 1)
    {
        //奇迹工坊使用次数减一
        if (_data.EventData is WonderWorkshopRuntimeData workshopData)
        {
            workshopData.UpgradeCount -= count;
            if (workshopData.UpgradeCount == 0)
            {
                _view.SetAsTriggered();
            }
        }
    }

    public void Locked() => _data.IsLocked = true;
    public void UnLocked() => _data.IsLocked = false;
}