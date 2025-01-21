using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeBase : SpriteClickHandler
{
    internal override void OnMouseEnter()
    {
        outLineMat.color = OutlineColor;
        spriteRenderer.material = outLineMat;// 高亮勾边
        if (Input.GetMouseButtonDown(0))
            transform.localScale = defaultScale * 0.8f;
        if (Input.GetMouseButtonUp(0))
            transform.localScale = defaultScale;
    }

    internal override void OnMouseExit()
    {
        spriteRenderer.material = defaultMat;// 还原
    }

}
