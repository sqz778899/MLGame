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

    public GameObject CreatInstance(string AssetPath)
    {
        return GameObject.Instantiate(GetAssetCache<GameObject>(AssetPath));
    }
    
    static string GetFileNameByPath(string CurPath)
   {
      CurPath = CurPath.Replace("\\","/");
      var x = CurPath.Split('/');
      string FileName = x[x.Length - 1];
      return FileName;
   }
}
