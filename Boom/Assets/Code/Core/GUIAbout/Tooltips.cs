using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tooltips : MonoBehaviour
{
    [SerializeField]  float YOffset = 45f;
    [Header("标题")]
    [SerializeField]  TextMeshProUGUI txtTitle;
    [SerializeField]  TextMeshProUGUI txtItemTitle;
    
    [Header("Group")]
    [SerializeField]  GameObject ItemGroup;
    [SerializeField]  GameObject OtherGroup;
    [Header("属性")] 
    [SerializeField]  GameObject AttriRoot;
    [SerializeField]  GameObject OriginAttriGO;
    [Header("描述")]
    [SerializeField]  TextMeshProUGUI txtDescription;

    [Header("背景")]
    [SerializeField] GameObject BG_Attri;
    [SerializeField] GameObject BG_Normal;
    [SerializeField] RectTransform BGRectTransformAttri;
    [SerializeField] RectTransform BGRectTransformItem;
     
    [Header("分割线")]
    [SerializeField]  Image IconDrividerLine;
    
    public void SetInfo(ToolTipsInfo toolTipsInfo)
    {
        txtTitle.color = ColorPalette.NormalTextColor;
        OriginAttriGO.SetActive(false);
        ClearInfo();
        if (toolTipsInfo.CurToolTipsType == ToolTipsType.Gem || 
            toolTipsInfo.CurToolTipsType == ToolTipsType.Bullet)
            SetGemBulletInfo(toolTipsInfo);
        else
            SetItemInfo(toolTipsInfo);
    }
    
    void SetItemInfo(ToolTipsInfo toolTipsInfo)
    {
        ItemGroup.SetActive(true);
        OtherGroup.SetActive(false);
        //加载标题
        txtItemTitle.text = toolTipsInfo.Name;
        //设置背景
        SetBGLength(toolTipsInfo);
        //加载描述
        txtDescription.text = TextProcessor.Parse(toolTipsInfo.Description);
        //设置稀有度颜色
        IconDrividerLine.color = ColorPalette.Rarity(toolTipsInfo.Rarity);
    }
    
    void SetGemBulletInfo(ToolTipsInfo toolTipsInfo)
    { 
        ItemGroup.SetActive(false);
        OtherGroup.SetActive(true);
        //加载标题
        txtTitle.text = toolTipsInfo.Name;
        //设置背景
        SetBGLength(toolTipsInfo);
        //逐条加载属性
        for (int i = 0; i < toolTipsInfo.AttriInfos.Count; i++)
        {
            ToolTipsAttriSingleInfo attriInfo = toolTipsInfo.AttriInfos[i];
            //实例化属性词条UI
            GameObject attriGO = Instantiate(OriginAttriGO, AttriRoot.transform);
            attriGO.SetActive(true);
            Vector2 curPos = attriGO.GetComponent<RectTransform>().anchoredPosition;
            Vector2 newPos = new Vector2(curPos.x, curPos.y - YOffset * i);
            attriGO.GetComponent<RectTransform>().anchoredPosition = newPos;
            //装填属性
            attriGO.GetComponent<ToolTipsAttriSingle>().InitData(attriInfo);
        }
    }

    #region Tooltips自适应背景高度相关
    bool isOpenLoop = false;
    int preLineCount = 0;
    int rightCount;
    void Update()
    {
        if (isOpenLoop)
        {
            int lineCount = txtDescription.textInfo.lineCount;
            int displayLine = Mathf.Max(2, lineCount); // 最少两行逻辑单位
            float bottom = Mathf.Min(-120,(-120 - (displayLine - 2) * 60));
            BGRectTransformItem.offsetMin = new Vector2(BGRectTransformItem.offsetMin.x,bottom); // Bottom (Y)
            
            //如果五次都没有变化，则关闭循环
            if(lineCount == preLineCount)
                rightCount++;
            preLineCount = lineCount;
            if(rightCount > 5)
            {
                isOpenLoop = false;
                rightCount = 0;
            }
        }
    }

    void SetBGLength(ToolTipsInfo toolTipsInfo)
    {
        if (toolTipsInfo.CurToolTipsType == ToolTipsType.Gem ||
            toolTipsInfo.CurToolTipsType == ToolTipsType.Bullet)
        {
            BG_Attri.SetActive(true);
            BG_Normal.SetActive(false);
            int attriCount = toolTipsInfo.AttriInfos.Count;//属性数量
            int bottom = Mathf.Min(-30,(-30 - (attriCount-2) * 50));
            BGRectTransformAttri.offsetMin = new Vector2(BGRectTransformAttri.offsetMin.x,bottom); // Bottom (Y)
        }
        else
        {
            BG_Attri.SetActive(false);
            BG_Normal.SetActive(true);
            //93 => -400
            //20 = 1line
            isOpenLoop = true;
            preLineCount = 0;
            rightCount = 0;
        }
    }
    #endregion
    void ClearInfo()
    {
        for (int i = AttriRoot.transform.childCount - 1; i >= 0; i--)
            Destroy(AttriRoot.transform.GetChild(i).gameObject);
    }
    
}