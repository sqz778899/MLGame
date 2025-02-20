using UnityEngine;
using UnityEngine.Events;

public class SpriteClickHandler : MonoBehaviour
{
    internal SpriteRenderer spriteRenderer;
    public UnityEvent onClick = new UnityEvent();
    
    [Header("显示相关")]
    [ColorUsage(true, true)] 
    public Color OutlineColor;
    public Color HeighLightColor = Color.white;
    
    internal Color defaultColor;
    internal Material defaultMat;
    internal Vector3 defaultScale;
    internal Material outLineMat
    {
        get
        {
            if (_outLineMat == null)
                _outLineMat = ResManager.instance.GetAssetCache<Material>(PathConfig.MatOutLine);
            return _outLineMat;
        }
    }
    Material _outLineMat;

    [Header("功能相关")]
    public bool IsLocked = false;
    
    internal virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        defaultMat = spriteRenderer.material;
        defaultScale = transform.localScale;
    }
    void Update()
    {
        if (TrunkManager.Instance.IsGamePause)
            return;
        if (UIManager.Instance.IsLockedClick)
            return;
        if(IsLocked) return;
        
        // 高亮显示
        if (IsMouseIn())
        {
            OnMouseEnter();
            if (Input.GetMouseButtonUp(0))
                onClick.Invoke();
        }
        else
            OnMouseExit();
    }

    #region 虚函数们
    internal virtual void OnMouseEnter()
    {
    }
    
    internal virtual void OnMouseExit()
    {
    }
    #endregion

    #region 射线检测
    bool IsMouseIn()
    {
        // 转换鼠标位置到世界坐标
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 发射一条从鼠标位置出发的射线
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        // 检查是否命中了我们的精灵
        return hit.collider != null && hit.collider.gameObject==gameObject;
    }
    #endregion
}
