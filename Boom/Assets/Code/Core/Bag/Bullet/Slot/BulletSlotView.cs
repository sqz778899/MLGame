using UnityEngine;

public class BulletSlotView: MonoBehaviour
{
    public GameObject LockedGO;
    public bool IsUnlocked => LockedGO != null && !LockedGO.activeSelf;
    public BulletSlotController Controller { get; private set; }

    public void Init()
    {
        Controller = new BulletSlotController();
        Controller.BindView(this);
    }

    public void Display(GameObject bulletGO)
    {
        bulletGO.transform.SetParent(transform);
        bulletGO.transform.localPosition = Vector3.zero;
    }

    public void Clear()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}