using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class MSceneManager: ScriptableObject
{
    #region 单例
    static MSceneManager s_instance;
    
    public static MSceneManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<MSceneManager>(PathConfig.MSceneManagerOBJ);
            return s_instance;
        }
    }
    #endregion

    public MapSate CurMapSate = new MapSate();
    public int CurrentSceneIndex;
    

    public void WinThisLevel()
    {
        CurMapSate.IsFinishedLevels.Add(CurMapSate.LevelID);
    }
    
    public void LoadScene(int SceneID)
    {
        CurrentSceneIndex = SceneID;
        SceneManager.LoadScene(CurrentSceneIndex);
        TrunkManager.Instance.SaveFile();
    }

    public void NewGame()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
        CurrentSceneIndex = 1;
        SceneManager.LoadScene(CurrentSceneIndex);
    }

    public void ContinueGame()
    {
        CurrentSceneIndex = 1;
        SceneManager.LoadScene(CurrentSceneIndex);
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
