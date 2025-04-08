using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GemNew: MonoBehaviour,IItemInteractionBehaviour
{
    public GemData Data { get; private set; }
    [Header("UI表现")]
    public Image Icon;
    public Image Frame;
    RectTransform rectTransform => GetComponent<RectTransform>();
    public SlotController CurSlotController;

    public event Action<GemNew> OnDoubleClick;//双击的外部绑定事件
    public event Action<GemNew> OnRightClick;//右击的外部绑定事件

    float lastClickTime;
    const float doubleClickThreshold = 0.3f;

    #region 数据交互相关
    public void BindData(GemData data)
    {
        Data = data;
        RefreshUI();
    }

    void RefreshUI()
    {
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(Data.ImageName));
        // TODO: 设置稀有度边框颜色等
    }
    #endregion
    

    void IItemInteractionBehaviour.OnDoubleClick()
    {
        Debug.Log("GemNew OnDoubleClick");
    }

    void IItemInteractionBehaviour.OnRightClick()
    {
        Debug.Log("GemNew OnRightClick");
    }
}