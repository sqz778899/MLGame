using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManager: Singleton<ABManager>
{
    AssetBundle mainAB = null;
    AssetBundleManifest manifest = null;
    Dictionary<string, AssetBundle> abCheckDict = new Dictionary<string, AssetBundle>();
    AssetBundle AB= null;
    private int Count = 0;

    string PathUrl { get { return Application.streamingAssetsPath + "/"; } }

    private List<string> ABNames = new List<string>()
    {
       "StandaloneWindows","res"
    };

    #region 同步加载
    public Object LoadRes<T>(string resName) where T : Object
    {
        if (Count == 0)
        {
            foreach (string each in ABNames)
                AB = AssetBundle.LoadFromFile(PathUrl + each);
        }
        Count++;
        return AB.LoadAsset<T>(resName);
    }
    #endregion

    #region 卸载
    public void UnLoad(string abName)
    {
        if (abCheckDict.ContainsKey(abName))
        {
            abCheckDict[abName].Unload(false);
            abCheckDict.Remove(abName);
        }
    }
    
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abCheckDict.Clear();
    }
    #endregion
}
