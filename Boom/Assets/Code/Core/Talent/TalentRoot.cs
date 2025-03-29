using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class TalentRoot : MonoBehaviour
{
    [Header("资产")]
    public GameObject LineRoot;
    public GameObject LinePrefab;
    
    [Header("动画参数")]
    public float LineFlowduration = 1f;
    
    TalentNode[] _talentNodes;
    List<TalentLine> _allLines = new List<TalentLine>();

    void Start()
    {
        InitTalentRoot();
    }  

    public void InitTalentRoot()
    {
        _talentNodes = GetComponentsInChildren<TalentNode>(true);
        _talentNodes.ForEach(t=>t.InitTalent());
        _allLines.Clear();
        for (int i = LineRoot.transform.childCount-1; i >=0; i--)
            Destroy(LineRoot.transform.GetChild(i).gameObject);
        for (int i = 0; i < _talentNodes.Length; i++)
        {
            TalentData curData = _talentNodes[i]._talentData;
            //if (curData.IsLocked) continue;
            for (int j = 0; j < curData.UnlockTalents.Count; j++)
            {
                TalentNode unlockNode = _talentNodes.FirstOrDefault(t => t.ID == curData.UnlockTalents[j]);
                if (unlockNode == null) continue;
                GameObject line = Instantiate(LinePrefab, LineRoot.transform);
                line.SetActive(true);
                RectTransform lineRect = line.GetComponent<RectTransform>();
                // 转换坐标
                Vector3 localA = lineRect.parent.InverseTransformPoint(_talentNodes[i].transform.position);
                Vector3 localB = lineRect.parent.InverseTransformPoint(unlockNode.transform.position);
                // 中点 & 方向
                Vector3 middle = (localA + localB) / 2f;
                Vector3 dir = localB - localA;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                float distance = dir.magnitude;
                float flowHeadTarget = Mathf.RoundToInt(distance / 100f);
                // 设置 RectTransform
                lineRect.localPosition = middle;
                lineRect.localRotation = Quaternion.Euler(0, 0, angle);
                lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);
                //实例化材质球
                Image img = line.GetComponent<Image>();
                Material matInstance = new Material(img.material); // 克隆
                img.material = matInstance;
                // 计算 tileX
                int tileX = Mathf.Max(1, Mathf.RoundToInt(distance / 100f));
                Vector2 tiling = new Vector2(tileX, 1);
                matInstance.mainTextureScale = tiling;
                // 记录
                var lineInfo = new TalentLine
                {
                    fromID = curData.ID,
                    toID = unlockNode.ID,
                    material = matInstance,
                    flowMaxHead = flowHeadTarget,
                    toNode = unlockNode
                };
                _allLines.Add(lineInfo);
                // 设置初始 FlowHead 值
                if (curData.IsLearned)
                    matInstance.SetFloat("_FlowHead", flowHeadTarget); // 满值
                else
                    matInstance.SetFloat("_FlowHead", 0); // 默认 0
            }
        }
        
        foreach (var node in _talentNodes)
        {
            node.OnLearned += OnNodeLearned;
        }
    }
    
    void OnNodeLearned(int id)
    {
        TalentData data = _talentNodes.FirstOrDefault(t=>t.ID == id)._talentData;

        foreach (var unlockID in data.UnlockTalents)
        {
            TalentData unlocked = PlayerManager.Instance._PlayerData.GetTalent(unlockID);
            if (unlocked == null) continue;
            unlocked.IsLocked = false;

            // 找出对应的线条，播放流动动画
            TalentLine line = _allLines.FirstOrDefault(l => l.fromID == data.ID && l.toID == unlockID);
            if (line != null && line.material.HasProperty("_FlowHead"))
                StartCoroutine(AnimateFlow(line.material,line.flowMaxHead,line.toNode));
        }
    }
    
    IEnumerator AnimateFlow(Material mat, float maxHead,TalentNode unlockNode)
    {
        float t = 0f;
        mat.SetFloat("_FlowHead", 0);

        while (t < 1.0f)
        {
            t += Time.deltaTime / LineFlowduration;
            float current = Mathf.Lerp(0f, maxHead, t);
            mat.SetFloat("_FlowHead", current);
            yield return null;
        }

        mat.SetFloat("_FlowHead", maxHead);

        // 动画完成后再解锁节点
        unlockNode.InitTalent();
    }
    
    class TalentLine
    {
        public int fromID;
        public int toID;
        public Material material;
        public float flowMaxHead;
        public TalentNode toNode;
    }
}
