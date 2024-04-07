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
        TrunkManager.Instance.LoadSaveFile();
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

    public void LoadSceneInCharacter(int ScenceID)
    {
        MSceneManager.Instance.CurMapSate.MapID = MapID;
        MSceneManager.Instance.LoadScene(ScenceID);
    }
}
