using System;
using System.Collections.Generic;
using UnityEngine;



public static class EventManager
{
    public static Action<int> OnBulletPicked;
    public static Action<int> OnBulletEquipped;
    public static Action OnFightStart;
    public static Action<int> OnGemInlayed;
    public static Action<int> OnRoomEntered;

    #region 主线剧情
    public static Action OnChapterOne;
    

    #endregion

}