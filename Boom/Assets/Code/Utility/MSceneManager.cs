using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
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
    
    public int MapID;
    public int LevelID;
    
    public int CurrentSceneIndex;
    
    public void LoadScene(int SceneID)
    {
        CurrentSceneIndex = SceneID;
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
