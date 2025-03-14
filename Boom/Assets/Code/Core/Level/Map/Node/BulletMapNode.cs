using System.Linq;
using UnityEngine;
using Spine.Unity;

public class BulletMapNode : MapNodeBase
{
    public int BulletID;
    public string DialogueName;
    SkeletonAnimation _ain;
    Renderer _renderer;
    
    void Awake()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        SpineQuitHighLight();
    }

    internal override void Start() {}
    
    internal override void OnMouseEnter()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineHighLight();
    }

    internal override void OnMouseExit()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineQuitHighLight();
    }

    public void JoinYou()
    {
        SpineQuitHighLight();
        MMapManager.CurDialogue.LoadDialogue(DialogueName);
        MMapManager.CurDialogue.OnDialogueEnd += OnDiaCallBack;
    }

    public void OnDiaCallBack()
    {
        MMapManager.CurDialogue.OnDialogueEnd -= OnDiaCallBack;
        
        MainRoleManager.Instance.AddSpawner(BulletID);
        BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(b => b.ID == BulletID) ?? new BulletJson();
        FloatingGetItemText($"获得{bulletDesignJson.Name}");
        Destroy(gameObject);
    }

    #region 一些私有方法
    void SpineHighLight()
    {
        uint layerToAdd = 1u << 1;
        _renderer.renderingLayerMask |= layerToAdd;
    }
    
    public void SpineQuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        _renderer.renderingLayerMask &= ~layerToRemove;
    }
    #endregion
}
