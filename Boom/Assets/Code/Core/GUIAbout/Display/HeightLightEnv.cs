using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeightLightEnv : SpriteClickHandler
{
    //需要切换的材质球
    Material _defaultMat;
    Material outLineMat;
    Material _outLineMat
    {
        get
        {
            if (outLineMat == null)
                outLineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatOutLine);
            return outLineMat;
        }
    }
    
    void Start()
    {
        base.Start();
        _defaultMat = spriteRenderer.material;
    }
    
}
