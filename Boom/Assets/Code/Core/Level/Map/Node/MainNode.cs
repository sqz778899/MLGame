using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainNode:MapNodeBase
{
    public int LevelID;
    public float Step;
    public Vector2[] LayoutPoints;

    void Start()
    {
        //NodeUtility.CreateLayoutPoints();
    }
    
    
    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
    }

    public void EnterFight()
    {
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(2);
    }
    
    void Testss()
    {
        float maxRadius = 15f;
        
        float step = 0.5f;
        float startY = maxRadius;
        float startX = maxRadius;

        int Count = (int)(maxRadius * 2 / step) + 1;
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < Count; j++)
            {
                float x = startX - step * i;
                float y = startY - step * j;
                if (CheckIden(x, y,maxRadius))
                {
                    CreateSphere(new Vector2(x, y));
                }
            }
        }
    }

    bool CheckIden(float x, float y,float radius)
    {
        bool s = false;
        if ((x * x + y * y) <= radius*radius)
            s = true;
        return s;
    }

    void CreateSphere(Vector2 pos)
    {
        GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        p.transform.position = pos;
    }

    public void SpawnResNode()
    {
        //Shop   1
        //Event   1
        GameObject EventIns = ResManager.instance.CreatInstance(PathConfig.MapNodeEvent);
        EventNode CurNode = EventIns.GetComponent<EventNode>();
        //CoinPile  1-2
        //TreasureBox 1-2
    }
    
    public class tempsss
    {
        //map lv1 1
        //map lv2 1
        //Event lv1 1
        //shop lv1 1
    }
    
    //生成周围Node
}