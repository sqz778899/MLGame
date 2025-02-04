using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BagRoot : MonoBehaviour
{
    public GameObject BagBulletRootGO;
    public GameObject BagItemRootGO;
    public GameObject BagGemRootGO;
    public GameObject BagReadySlotGO;
    public GameObject DragObjRootGO;

    public void SwichBullet()
    {
        BagBulletRootGO.SetActive(true);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
    }

    public void SwichItem()
    {
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(false);
        BagItemRootGO.SetActive(true);
        BagGemRootGO.SetActive(false);
    }
    
    public void SwichGem()
    {
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
    }
}
