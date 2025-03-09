using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class EnemyMiniMap : MonoBehaviour
{
    public Enemy CurEnemy;
    [Header("资产")]
    public SkeletonGraphic SkeletonGraphic; //敌人的UI层Spine资产
    public TextMeshProUGUI EnemyHPText; //敌人的血量文本
    public GameObject SheildsNode; //敌人的护盾节点
    public GameObject SheildGO; //护盾的预制体
}
