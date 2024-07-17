using System;
using UnityEngine;

public class Connon : BaseMove
{
    internal override void Update()
    {
        base.Update();
        //快捷键响应
        _fightLogic.CheckForKeyPress(transform.position);
    }
    
    public void Fire()
    {
        _fightLogic.Fire(transform.position);
    }
}