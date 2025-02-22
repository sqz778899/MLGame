
using System;

public class RoomKeyNode : MapNodeBase
{
    public int RoomKeysNum = 1;
    
    public void GetRoomKeys()
    {
        EPara.InsNum = RoomKeysNum;
        EPara.StartPos = transform.position;
        MEffectManager.CreatEffect(EPara,false,()=>FloatingGetItemText("获得一个钥匙！"));
        DestroyImmediate(gameObject);
        MainRoleManager.Instance.RoomKeys += RoomKeysNum;
    }
}
