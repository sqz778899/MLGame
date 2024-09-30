using UnityEngine;
using UnityEngine.Events;

public class SpriteClickHandler : MonoBehaviour
{
    internal SpriteRenderer spriteRenderer;
    public UnityEvent onClick = new UnityEvent();
    
    [Header("显示相关")]
    Color normalColor;
    public Color HeighLightColor = Color.white;
    internal Vector3 originalScale;
    
    internal virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (UIManager.Instance.IsPauseClick)
            return;
        
        // 高亮显示
        if (IsMouseIn())
        {
            spriteRenderer.color = HeighLightColor;// 将精灵高亮显示
            if (Input.GetMouseButtonDown(0))
            {
                transform.localScale = originalScale * 0.8f;
            }
            if (Input.GetMouseButtonUp(0))
            {
                transform.localScale = originalScale;
                onClick.Invoke();
            }
        }
        else
            spriteRenderer.color = normalColor;// 取消高亮显示
    }
    
    bool IsMouseIn()
    {
        // 转换鼠标位置到世界坐标
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 发射一条从鼠标位置出发的射线
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        //Debug.Log(hit.collider.gameObject);
        // 检查是否命中了我们的精灵
        if (hit.collider != null && hit.collider.gameObject == spriteRenderer.gameObject)
            return true;
        else
            return false;
    }
}
