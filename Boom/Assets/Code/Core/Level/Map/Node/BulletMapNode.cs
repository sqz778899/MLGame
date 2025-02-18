using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting;

public class BulletMapNode : MapNodeBase
{
    public int BulletID;
    public string DialogueName;
    SkeletonAnimation _ain;
    Renderer _renderer;

    void Start()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
    }

    internal override void OnMouseEnter()
    {
        uint layerToAdd = 1u << 1;
        _renderer.renderingLayerMask |= layerToAdd;
    }

    internal override void OnMouseExit()
    {
        uint layerToRemove = 1u << 1;
        _renderer.renderingLayerMask &= ~layerToRemove;
    }

    public void JoinYou()
    {
        Dialogue curDia = MMapLogic.CurDialogue;
        curDia.LoadDialogue(DialogueName);
        curDia.OnDialogueEnd += OnDiaCallBack;
    }

    void OnDiaCallBack()
    {
        MainRoleManager.Instance.AddSpawner(BulletID);
        BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(b => b.ID == BulletID) ?? new BulletJson();
        FloatingGetItemText(bulletDesignJson.Name);
        Destroy(gameObject);
    }
}
