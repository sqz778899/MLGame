using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BulletBase : MonoBehaviour
{
    public GameObject Ins;
    public Vector3 forward = new Vector3(1, 0, 0);
    public int InstanceID;
    public BulletData _bulletData;
    public BulletInsMode bulletInsMode;
    public GameObject Edit_a;
    public GameObject Edit_b;
    GameObject GroupStar;
    internal GameObject TooltipsGO;
    
    public virtual void Update()
    {
        if (GroupStar != null)
        {
            SetStart(_bulletData.Level);
        }
    }

    void SetStart(int Level)
    {
        switch (Level)
        {
            case 1:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(false);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }

    void SetInfo(GameObject curRoot)
    {
        //...........GroupStar...................
        GroupStar = null;
        for (int i = 0; i < curRoot.transform.childCount; i++)
        {
            if (curRoot.transform.GetChild(i).name == "GroupStar")
                GroupStar = curRoot.transform.GetChild(i).gameObject;
        }
        //...........Set Image&&SpineAsset...................
        if (bulletInsMode == BulletInsMode.Inner)
        {
            SkeletonAnimation SkeleSC = curRoot.GetComponentInChildren<SkeletonAnimation>();
            if (SkeleSC == null)
                return;
            
            SkeleSC.skeletonDataAsset = _bulletData.bulletSpineAsset;
        }
        else
        {
            Image target = null;
            Image[] allImage = GetComponentsInChildren<Image>();
            foreach (var each in allImage)
            {
                if (each.gameObject.name == "imgBullet")
                {
                    target = each;
                    break;
                }
            }
            if (target == null)
                return;
            //
            target.sprite = _bulletData.imgBullet;
        }
    }

    //初始化子弹的资产
    public void InitBulletData()
    {
        if (_bulletData == null)
            _bulletData = new BulletData(1);
        _bulletData.SetDataByID(bulletInsMode);
        
        switch (bulletInsMode)
        {
            case BulletInsMode.Inner:
                SetInfo(this.gameObject);
                break;
            case BulletInsMode.Spawner:
                SetInfo(this.gameObject);
                break;
            case BulletInsMode.EditA:
                Edit_a.SetActive(true);
                Edit_b.SetActive(false);
                SetInfo(Edit_a);
                break;
            case BulletInsMode.EditB:
                Edit_a.SetActive(false);
                Edit_b.SetActive(true);
                SetInfo(Edit_b);
                break;
            default:
                GroupStar = null;
                break;
        }
    }

    internal void DisplayTooltips(Vector3 pos)
    {
        if (TooltipsGO == null)
        {
            TooltipsGO = Instantiate(ResManager.instance
                .GetAssetCache<GameObject>(PathConfig.TooltipAsset));
            CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
            curTip.SyncInfo(_bulletData.ID,ItemTypes.Bullet);
            TooltipsGO.transform.SetParent(UIManager.Instance.TooltipsRoot.transform);
            TooltipsGO.transform.localScale = Vector3.one;
        }
        TooltipsGO.transform.position = pos;
    }

    internal void DestroyTooltips()
    {
        for (int i = UIManager.Instance.TooltipsRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(UIManager.Instance.TooltipsRoot
                .transform.GetChild(i).gameObject);
        }
    }
}