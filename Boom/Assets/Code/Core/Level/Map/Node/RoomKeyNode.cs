using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomKeyNode : MapNodeBase
{
    public int RoomKeysNum = 1;
    
    public void GetRoomKeys()
    {
        EPara.InsNum = RoomKeysNum;
        EPara.StartPos = transform.position;
        MEffectManager.CreatEffect(EPara);
        DestroyImmediate(gameObject);
        MainRoleManager.Instance.RoomKeys += RoomKeysNum;
    }
}
