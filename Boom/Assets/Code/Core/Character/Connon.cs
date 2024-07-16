using UnityEngine;

public class Connon : BaseMove
{
    FightLogic _fightLogic;
    
    internal override void Awake()
    {
        base.Awake();
        _fightLogic =  GameObject.Find("LevelLogic").GetComponent<FightLogic>();
    }
    
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