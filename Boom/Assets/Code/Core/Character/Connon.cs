using UnityEngine;

public class Connon : BaseMove
{
    LevelLogicMono LevelLogic;
    
    internal override void Awake()
    {
        base.Awake();
        LevelLogic =  GameObject.Find("LevelLogic").GetComponent<LevelLogicMono>();
    }
    
    internal override void Update()
    {
        base.Update();
        //快捷键响应
        LevelLogic.CheckForKeyPress(transform.position);
    }
    
    public void Fire()
    {
        LevelLogic.Fire(transform.position);
    }
}