using UnityEngine;
using UnityEngine.UI;


public class MyImage : Image
{
   public override bool IsRaycastLocationValid(Vector2 screenPoint,Camera eventCamera)
   {
      Debug.Log(GetComponent<Collider2D>().OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint)));
      return GetComponent<Collider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
   }
}
