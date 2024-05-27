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
}