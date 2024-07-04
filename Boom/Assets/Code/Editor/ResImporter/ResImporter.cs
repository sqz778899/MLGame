using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResImporter : AssetPostprocessor
{
    public void OnPreprocessTexture()
    {
        if (assetPath.StartsWith("Assets/Res/Bullet/")||
            assetPath.StartsWith("Assets/Res/UI/") ||
            assetPath.StartsWith("Assets/Res/Character/"))
        {
            int MaxSize = 4096;
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = false;
            textureImporter.filterMode = FilterMode.Bilinear;
            
            var platformAndroid = textureImporter.GetPlatformTextureSettings("Android"); //安卓为主。IOS也根据安卓来
            platformAndroid.overridden = true;
            platformAndroid.maxTextureSize = MaxSize;
            platformAndroid.format = TextureImporterFormat.ASTC_4x4;
            textureImporter.SetPlatformTextureSettings(platformAndroid);
            
            var platformPC  = textureImporter.GetPlatformTextureSettings("Standalone");
            platformPC.overridden = true;
            platformPC.maxTextureSize = MaxSize;
            platformPC.format = TextureImporterFormat.DXT5;
            textureImporter.SetPlatformTextureSettings(platformPC);
            
            var platIOS = textureImporter.GetPlatformTextureSettings("iPhone");
            platIOS.overridden = true;
            platIOS.maxTextureSize = MaxSize;
            platIOS.format = TextureImporterFormat.ASTC_4x4;
            textureImporter.SetPlatformTextureSettings(platIOS);
        }
    }
}
