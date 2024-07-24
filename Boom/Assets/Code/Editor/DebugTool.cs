using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DebugTool
{
    public Enemy curEnemy;
    
    
    [Button(ButtonSizes.Large)]
    void Debugsss()
    {
        BulletManager.Instance.InstanceBullet(1, BulletInsMode.Inner);
    }
}
