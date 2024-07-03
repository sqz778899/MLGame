using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainNode:MapNodeBase
{
    public int LevelID;

    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
    }

    public void EnterFight()
    {
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(2);
    }

    public void ppp()
    {
        Vector3 myPos = transform.position;
        float radius = 100;
        //Random.insideUnitCircle
        
        for (int i = 0; i < 150; i++) {
            var rand = Random.insideUnitCircle * 3;
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = new Vector3(rand.x, 0, rand.y);
            obj.transform.localScale = Vector3.one * 0.3f;
        }
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