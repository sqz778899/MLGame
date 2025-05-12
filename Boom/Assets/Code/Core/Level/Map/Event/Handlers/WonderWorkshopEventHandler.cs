using UnityEngine;

public class WonderWorkshopEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not WonderWorkshopRuntimeData runtimeData)
        {
            Debug.LogWarning("WonderWorkshopRuntimeData 为 null，未能弹出 UI");
            return;
        }
        
        if (runtimeData.UpgradeCount == 0)
        {
            view.ShowFloatingText("奇迹工坊的升级次数已用完！");
            return;
        }
        EternalCavans.Instance.WonderWorkshopSC.Bind(view.controller);
        EternalCavans.Instance.WonderWorkshopSC.Show();
        
        view.SetAsTriggered();  
    }
}