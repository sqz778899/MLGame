using UnityEngine;
using UnityEngine.Events;

public class SpriteClickHandler : MonoBehaviour
{
    public UnityEvent onClick = new UnityEvent();
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UIManager.Instance.IsPauseClick)
                return;
            
            if (IsClicked())
                onClick.Invoke();
        }
    }

    bool IsClicked()
    {
        bool isClick = false;
        RaycastHit[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 进行射线检测
        hits = Physics.RaycastAll(ray.origin, ray.direction);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == this.transform && hit.transform.tag == "SButton")
                isClick = true;
        }
        return isClick;
    }
}
