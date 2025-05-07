using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuManager:MonoBehaviour
{
    public static RightClickMenuManager Instance { get; private set; }

    [SerializeField] GameObject panelGO; // 整个右键菜单容器
    [SerializeField] GameObject panelArea; // 包含透明背景的大区域
    [SerializeField] Button btnEquip;
    [SerializeField] Button btnRemove;
    
    [SerializeField] float autoCloseDelay = 0.3f; 
    Vector2 rightClickMenuOffset;

    public GameObject CurIns;

    public void Init()
    {
        Instance = this;
        panelGO.SetActive(false);
        btnRemove.onClick.AddListener(OnClickRemove);
        btnEquip.onClick.AddListener(OnClickEquip);
        rightClickMenuOffset = new Vector2(0.75f, -0.35f);
    }

    public void Show(GameObject go, Vector2 screenPos)
    {
        CurIns = go;
        panelGO.SetActive(true);
        panelGO.transform.position = screenPos + rightClickMenuOffset;
        TooltipsManager.Instance.Disable(); //禁用 tooltips
        StartCoroutine(WaitAndEnableAutoClose());
    }

    public void Hide()
    {
        CurIns = null;
        panelGO.SetActive(false);
        TooltipsManager.Instance.Enable();
    }

    void OnClickRemove()
    {
        if (CurIns==null) return;
        BagItemTools<ItemBase>.DeleteObject(CurIns);
        Hide();
    }

    void OnClickEquip()
    {
        if (CurIns==null) return;
        ItemBase curBaseSC = CurIns.GetComponent<ItemBase>();
        if (curBaseSC is Gem curGem)
        {
            GemSlotController curEmptyGemSlot = SlotManager.GetEmptySlotController(SlotType.GemInlaySlot) as GemSlotController;
            if (curEmptyGemSlot == null) return;
            if (curEmptyGemSlot.CanAccept(curGem.Data))
                curEmptyGemSlot.Assign(curGem.Data, CurIns);
        }
        Hide();
    }

    #region 延迟退出
    IEnumerator WaitAndEnableAutoClose()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        canAutoClose = true;
    }
    
    bool canAutoClose = false;
    void Update()
    {
        if (!canAutoClose || !panelArea.activeSelf) return;

        if (!RectTransformUtility.RectangleContainsScreenPoint(
                panelArea.GetComponent<RectTransform>(), Input.mousePosition,Camera.main))
        {
            Hide();
        }
    }
    #endregion
}