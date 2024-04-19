using UnityEngine;

public class CharacterScene : MonoBehaviour
{
    public int MapID;
    
    public GameObject btnBeReady;
    public GameObject btnEditBullet;
    public GameObject btnGO;
    public GameObject GroupBulletSlot;
    public GameObject GroupCharacter;

    void Start()
    {
        //.................Global...........................
        TrunkManager.Instance.LoadSaveFile();
        //.................Local...........................
        UIManager.Instance.InitCharacterScene();
        CharacterManager.Instance.InitData();
    }

    public void EditBullet()
    {
        //Button
        btnBeReady?.SetActive(true);
        btnEditBullet?.SetActive(false);
        btnGO?.SetActive(false);
        //GUI
        GroupCharacter?.SetActive(false);
        GroupBulletSlot?.SetActive(true);
    }

    public void BeReady()
    {
        //Button
        btnBeReady?.SetActive(false);
        btnEditBullet?.SetActive(true);
        btnGO?.SetActive(true);
        //GUI
        GroupCharacter?.SetActive(true);
        GroupBulletSlot?.SetActive(false);
    }

    public void GO()
    {
        if ( CharacterManager.Instance.CurBullets.Count == 0)
        {
            GameObject curTexttipIns = ResManager.instance.CreatInstance<GameObject>(PathConfig.TexttipAsset);
            curTexttipIns.GetComponent<TextTip>().CurText.text = "Non Bullet Ready !";
            curTexttipIns.transform.SetParent(UIManager.Instance.TooltipsRoot.transform,false);
            curTexttipIns.transform.localScale = Vector3.one;
            return;
        }
        LoadSceneInCharacter(3);
    }
    
    public void LoadSceneInCharacter(int ScenceID)
    {
        MSceneManager.Instance.CurMapSate.MapID = MapID;
        MSceneManager.Instance.LoadScene(ScenceID);
    }
}
