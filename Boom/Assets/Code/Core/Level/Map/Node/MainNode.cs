using System;

public class MainNode:MapNodeBase
{
    public int LevelID;

    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
    }

    public void EnterFight()
    {
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(2);
    }
    
    public class tempsss
    {
        //map lv1 1
        //map lv2 1
        //Event lv1 1
        //shop lv1 1
    }
    
    //生成周围Node
}