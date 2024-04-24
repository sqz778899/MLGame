using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelRole : MonoBehaviour,IPointerMoveHandler,IPointerClickHandler
{
    public int roleID;
    public GameObject fxSelBox;

    public void OnPointerMove(PointerEventData eventData)
    {
        /*fxSelBox.transform.SetParent(eventData.pointerEnter.transform,false);
        fxSelBox.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        Debug.Log(eventData.pointerEnter.name);*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        fxSelBox.transform.SetParent(eventData.pointerEnter.transform,false);
        fxSelBox.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        Debug.Log(eventData.pointerEnter.name);
    }
}
