using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    GameObject draggedObject;
    Vector3 originalPosition;
    Transform originalParent;
    RectTransform dragRoot;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dragRoot = UIManager.Instance.CommonUI.DragObjRoot.GetComponent<RectTransform>();
    }

    public bool CanDrag() => !EternalCavans.Instance.TutorialDragGemLock;

    public void BeginDrag(GameObject go, PointerEventData eventData)
    {
        draggedObject = go;
        originalParent = go.transform.parent;
        originalPosition = go.transform.position;
        go.transform.SetParent(dragRoot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragRoot, eventData.position, eventData.enterEventCamera, out var worldPoint);
            draggedObject.transform.position = worldPoint;
        }
    }

    public void EndDrag(PointerEventData eventData)
    {
        if (draggedObject == null) return;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool dropped = false;
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotView slotView))
            {
                GemNew gem = draggedObject.GetComponent<GemNew>();
                if (slotView.Controller.CanAccept(gem.Data))
                {
                    slotView.Controller.Assign(gem.Data, draggedObject);
                    dropped = true;
                    break;
                }
            }
        }

        if (!dropped)
        {
            draggedObject.transform.SetParent(originalParent);
            draggedObject.transform.position = originalPosition;
        }

        draggedObject = null;
        originalParent = null;
    }
}