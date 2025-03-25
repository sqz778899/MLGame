using UnityEngine;
using UnityEngine.UI;

public class SiderbarSync : MonoBehaviour
{
    public GameObject IconRoot;
    void Start()
    {
        InventoryManager.Instance._BulletInvData.OnBulletsChanged += SyncBulletIcon;
        SyncBulletIcon();//最开始同步一次
    }

    void SyncBulletIcon()
    {
        for (int i = 0; i < 5; i++)
        {
            int curSlotID = i + 1;
            BulletData quipBullet = null;
            foreach (BulletData each in InventoryManager.Instance._BulletInvData.EquipBullets)
            {
                if (each.CurSlot.SlotID == curSlotID)
                    quipBullet = each;
            }
            GameObject curIconSlot = IconRoot.transform.GetChild(curSlotID).gameObject;//找到对应的IconSlot
            Image curImg = curIconSlot.transform.GetChild(0).GetComponent<Image>();
            if (quipBullet == null)
                curImg.color = Color.clear;
            else
            {
                curImg.color = Color.white;
                curImg.sprite = ResManager.instance.GetAssetCache<
                    Sprite>(PathConfig.GetBulletImageOrSpinePath(quipBullet.ID, BulletInsMode.Icon));
            }
        }
    }

    private void OnDestroy()
    {
        InventoryManager.Instance._BulletInvData.OnBulletsChanged -= SyncBulletIcon;
    }
}
