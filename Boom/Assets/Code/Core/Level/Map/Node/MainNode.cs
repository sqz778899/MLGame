using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainNode:MapNodeBase
{
    public int LevelID;
    public float Step = 0.05f;
    public List<Vector3> LayoutPoints;
    
    public CircleCollider2D ColCotain;
    
    //
    bool isSpwaned = false;

    internal override void Start()
    {
        base.Start();
        float maxRadius = ColCotain.radius;
        Debug.Log(transform.position.z);
        LayoutPoints = NodeUtility.CreateLayoutPoints(maxRadius,Step,transform.position.z);
        NodeUtility.ExcludePointsPool(ref LayoutPoints, ColExclude);
    }
    
    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
        if (!isSpwaned)
        {
            SpawnResNode();
            isSpwaned = true;
        }
    }

    public void EnterFight()
    {
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(2);
    }
    
    //生成周围Node
    public void SpawnResNode()
    {
        //Shop   1
        //Event   2
        //CoinPile  3
        //TreasureBox 4
        int type = Random.Range(1, 5);
        SpawnResNodeSingle(type);
    }

    void SpawnResNodeSingle(int type)
    {
        Vector3 curP = LayoutPoints[Random.Range(0, LayoutPoints.Count)];
        GameObject curNode = null;
        switch (type)
        {
            case 1:
                curNode = ResManager.instance.CreatInstance(PathConfig.MapShop);
                break;
            case 2:
                curNode = ResManager.instance.CreatInstance(PathConfig.MapNodeEvent);
                break;
            case 3:
                curNode = ResManager.instance.CreatInstance(PathConfig.MapGoldPile);
                break;
            case 4:
                curNode = ResManager.instance.CreatInstance(PathConfig.MapTreasureBox);
                break;
        }
        
        curNode.transform.position = curP;
        curNode.transform.SetParent(UIManager.Instance.MapNode.transform,false);
    }
}