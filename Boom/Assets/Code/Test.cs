using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float Distance;
    public Vector2 GenRange;
    public Vector2 ScaleRange;
    
    internal void GenerateMisc()
    {
        float _distance = Distance;
        float xRangeSum = -7f;
        float z = transform.position.z;
        int safeCount = 0;
        while (_distance > 0)
        {
            float xRange = Random.Range(GenRange.x, GenRange.y);
            xRangeSum += xRange;
            float yRange = Random.Range(1.08f, 5.12f);
            Vector3 nextPos = new Vector3(xRangeSum,yRange,z);
            //...............
            float xSRange = Random.Range(ScaleRange.x, ScaleRange.y);
            float ySRange = Random.Range(ScaleRange.x, ScaleRange.y);
            Vector3 nextScale = new Vector3(xSRange,ySRange,0);
            
            CreateOneGO(nextPos,nextScale);
            
            Debug.Log("xxxxx" + _distance);
            _distance -= xRange;
            
            ///.............安全退出
            safeCount++;
            if (safeCount > 1000)
                _distance = -1;
        }
    }

    GameObject CreateOneGO(Vector3 pos,Vector3 scale)
    {
        GameObject go = new GameObject("xx");
        go.transform.SetParent(this.transform);
        go.transform.position = pos;
        go.transform.localScale = scale;
        SpriteRenderer sRender = go.AddComponent<SpriteRenderer>();
        int picIndex = Random.Range(1, 6);
        Sprite curSprite = ResManager.instance.GetAssetCache<Sprite>(PathConfig.MiscDir + string.Format("T_Misc_0{0}.png",picIndex));
        sRender.sprite = curSprite;
        return go;
    }
}
