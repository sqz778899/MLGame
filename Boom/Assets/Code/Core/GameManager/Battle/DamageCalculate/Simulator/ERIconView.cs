using UnityEngine;
using UnityEngine.EventSystems;

public class ERIconView: MonoBehaviour
{
    public SpriteRenderer Icon;
    ElementReactionData elementReactionData = new ElementReactionData();
    [Header("偏移相关")]
    public Vector2 TooltipOffset;
    bool _isMouseOver = false;
    
    public void InitData(ElementReactionType reaction)
    {
        if (elementReactionData == null)
            elementReactionData = new ElementReactionData();
        elementReactionData.InitData(reaction);
        
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == reaction.ToString())
            {
                Icon = child.GetComponent<SpriteRenderer>();
                Icon.gameObject.SetActive(true);
                break;
            }
        }
    }
    
    void OnMouseEnter()
    {
        _isMouseOver = true;
        TooltipsManager.Instance.Show(elementReactionData.BuildTooltip(), transform.position+ (Vector3)TooltipOffset);
    }

    void OnMouseExit()
    {
        _isMouseOver = false;
        TooltipsManager.Instance.Hide();
    }


    void Update()
    {
        if (_isMouseOver)
            TooltipsManager.Instance.UpdatePosition(transform.position + (Vector3)TooltipOffset);
    }
}