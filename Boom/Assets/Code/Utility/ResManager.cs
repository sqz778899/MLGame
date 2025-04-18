using UnityEditor;
using UnityEngine;

public class ResManager : Singleton<ResManager>
{
    public T GetAssetCache<T>(string AssetPath) where T : Object
    {
#if UNITY_EDITOR
       Object target = AssetDatabase.LoadAssetAtPath<T>(AssetPath);
       return target as T;
#endif
       string FileName = GetFileNameByPath(AssetPath).Split('.')[0];  
       return ABManager.instance.LoadRes<T>(FileName) as T;
    }

    //直接创建GamObject的简便方法
    public GameObject CreatInstance(string AssetPath) => GameObject.Instantiate(GetAssetCache<GameObject>(AssetPath));
    
    //直接获得ItemIcon的简便方法
    public Sprite GetItemIcon(int id)
    {
        ItemJson json = TrunkManager.Instance.GetItemJson(id);
        string AssetPath = PathConfig.GetItemPath(json.ResName, json.Category);
        return GetAssetCache<Sprite>(AssetPath);
    }
    
    //直接获得GemIcon的简便方法
    public Sprite GetGemIcon(int id)
    {
        GemJson json = TrunkManager.Instance.GetGemJson(id);
        string AssetPath = PathConfig.GetGemPath(json.ImageName);
        return GetAssetCache<Sprite>(AssetPath);
    }

    static string GetFileNameByPath(string CurPath)
    {
        CurPath = CurPath.Replace("\\","/");
        var x = CurPath.Split('/');
        string FileName = x[x.Length - 1];
        return FileName;
    }
}
