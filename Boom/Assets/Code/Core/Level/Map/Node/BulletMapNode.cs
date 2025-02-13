using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting;

public class BulletMapNode : MapNodeBase
{
    SkeletonAnimation _ain;

    void Start()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
    }
}
