using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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

    public void SaveFile()
    {
        SaveFileJson lll = new SaveFileJson();
        string content01 = JsonConvert.SerializeObject(lll,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    
    public void LoadFile()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        SaveFileJson SaveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);
        CharacterManager.Instance.LoadSaveFile(SaveFile);
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
