using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainNode:MapNodeBase
{
    public int LevelID;

    #region SpaenNode相关
    //SpaenNode相关
    public float Step = 0.05f;
    public float CotainRaius = 55f;
    public List<Vector3> LayoutPoints = new List<Vector3>();
    public Vector2Int ShopNodeCountRange = new Vector2Int(1, 1);
    public Vector2Int EventNodeCountRange = new Vector2Int(1, 2);
    public Vector2Int CoinPileNodeCountRange = new Vector2Int(1, 2);
    public Vector2Int TreasureBoxNodeCountRange = new Vector2Int(1, 2);
    //Shop   1
    //Event   2
    //CoinPile  3
    //TreasureBox 4
    #endregion
    
#if UNITY_EDITOR
    internal override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CotainRaius);
    }
#endif
    
    //
    bool isSpwaned = false;

    internal override void Start()
    {
        base.Start();
        LayoutPoints = NodeUtility.CreateLayoutPoints(
            CotainRaius,Step,transform.position);
        NodeUtility.ExcludePointsPool(ref LayoutPoints, ExcludeRadius,transform.position);
    }
    
    void Update()
    {
        txtTitle.text = string.Format("LV{0}", LevelID);
        if (!isSpwaned && State == MapNodeState.IsFinish)
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
        SpawnByType(1,ShopNodeCountRange);
        SpawnByType(2,EventNodeCountRange);
        SpawnByType(3,CoinPileNodeCountRange);
        SpawnByType(4,TreasureBoxNodeCountRange);
        LayoutPoints.Clear();
    }

    public void SpawnByType(int type,Vector2Int ShopNodeCountRange)
    {
        int curShopCount = Random.Range(ShopNodeCountRange.x, ShopNodeCountRange.y + 1);
        for (int i = 0; i < curShopCount; i++)
        {
            SpawnResNodeSingle(type);
        }
    }

    void SpawnResNodeSingle(int type)
    {
        //............Spawn GO..................
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
        
        //............Refresh Pool..................
        MapNodeBase curNodeSC = curNode.GetComponent<MapNodeBase>();
        NodeUtility.ExcludePointsPool(ref LayoutPoints, curNodeSC.ExcludeRadius,curP);
    }
}