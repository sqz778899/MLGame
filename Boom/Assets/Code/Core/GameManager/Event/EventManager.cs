using System;
using System.Collections.Generic;
using UnityEngine;



public static class EventManager
{
    public static Action OnBulletPicked;
    public static Action OnBulletEquipped;
    public static Action OnFirstBattleEnd;
    public static Action OnGemEquipped;
    public static Action<int> OnRoomEntered;

    #region 主线剧情
    public static Action OnChapterOne;
    

    #endregion

}