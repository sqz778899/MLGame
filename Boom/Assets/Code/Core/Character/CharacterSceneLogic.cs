using UnityEngine;
using UnityEngine.Serialization;

public class CharacterSceneLogic : KeyBoardBase
{
    public int MapID;
    public GameObject btnGO;

    void Start()
    {
        //.................Global...........................
        TrunkManager.Instance.LoadSaveFile();
        //.................Local...........................
        UIManager.Instance.InitCharacterScene();
        MainRoleManager.Instance.InitData();
    }

    public void EditBullet()
    {
        //Button
        btnGO?.SetActive(false);
    }

    public void GO()
    {
        if ( MainRoleManager.Instance.CurBullets.Count == 0)
        {
            GameObject curTexttipIns = ResManager.instance.CreatInstance(PathConfig.TexttipAsset);
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
