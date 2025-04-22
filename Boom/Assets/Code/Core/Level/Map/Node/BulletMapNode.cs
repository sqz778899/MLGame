using System.Linq;
using Spine.Unity;

public class BulletMapNode : MapNodeBase
{
    public int BulletID;
    public string DialogueName;
    SkeletonAnimation _ain;
    public Dialogue _dialogue;
    
    void Awake()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
    }

    internal override void Start()
    {
        base.Start();
        _dialogue = EternalCavans.Instance.DialogueSC;
        QuitHighLight();
    }
    
    public void JoinYou()
    {
        QuitHighLight();
        _dialogue.LoadDialogue(DialogueName);
        _dialogue.OnDialogueEnd += OnDiaCallBack;
    }

    public void OnDiaCallBack()
    {
        _dialogue.OnDialogueEnd -= OnDiaCallBack;
        
        InventoryManager.Instance._BulletInvData.AddSpawner(BulletID);
        BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(b => b.ID == BulletID) ?? new BulletJson();
        FloatingGetItemText($"获得{bulletDesignJson.Name}");
        Destroy(gameObject);
    }
}
