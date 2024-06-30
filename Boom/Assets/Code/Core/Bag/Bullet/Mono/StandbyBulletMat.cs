using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StandbyBulletMat:MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int ID;
    public Image CurImg;
    
    Vector3 originalPosition;
    public void OnPointerDown(PointerEventData eventData)
    {
        // 记录下我们开始拖动时的位置
        originalPosition = transform.parent.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.parent.position = originalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rectTransform = transform.parent.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
    }
    
    public void InitData(int _id)
    {
        ID = _id;
        CurImg.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImagePath(ID, BulletInsMode.Mat));
    }

    public void DestroySelf()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}