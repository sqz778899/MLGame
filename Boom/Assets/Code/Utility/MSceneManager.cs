using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu]
public class MSceneManager: ScriptableObject
{
    public int CurrentSceneIndex;
    public void LoadScene(int level)
    {
        CurrentSceneIndex = level;
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
