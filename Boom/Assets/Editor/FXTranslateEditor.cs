using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class FXTranslateEditor
{
    [Button(ButtonSizes.Large)]
    void RaplaceShader()
    {
        Shader curFXShader = Shader.Find("Universal Render Pipeline/Particles/Simple Lit");
        Debug.Log(curFXShader);
        foreach (var each in Selection.gameObjects)
        {
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(each);
            string[] DepStrs = AssetDatabase.GetDependencies(path);
            foreach (var eachStr in DepStrs)
            {
                if (eachStr.EndsWith(".mat"))
                {
                    Material curMat = AssetDatabase.LoadAssetAtPath<Material>(eachStr);
                    if (curMat.shader != curFXShader)
                    {
                        Texture mTexture = curMat.mainTexture;
                        curMat.shader = curFXShader;
                        curMat.SetTexture("_BaseMap",mTexture);
                        
                        var propertyID = Shader.PropertyToID("_Surface");
                        curMat.GetFloat(propertyID);
                        
                        curMat.SetInt(propertyID,1);
                        curMat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        curMat.renderQueue = 3000;
                        EditorUtility.SetDirty(curMat);
                    }
                }
            }
        }
        AssetDatabase.SaveAssets();
    }

    [Button(ButtonSizes.Large)]
    void RanameFXMat()
    {
        foreach (var each in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(each);
            Material curMat = AssetDatabase.LoadAssetAtPath<Material>(path);
            string newName = "M_FX_" + curMat.name;
            AssetDatabase.RenameAsset(path, newName);
        }
    }
    
    [Button(ButtonSizes.Large)]
    void RanameFXTexture()
    {
        foreach (var each in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(each);
            Texture curMat = AssetDatabase.LoadAssetAtPath<Texture>(path);
            string newName = "fx_" + curMat.name;
            AssetDatabase.RenameAsset(path, newName);
        }
    }
    
}
