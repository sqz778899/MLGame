using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScene : MonoBehaviour
{
    public GameObject btnBeReady;
    public GameObject btnEditBullet;
    public GameObject btnGO;
    public GameObject GroupBulletSlot;
    public GameObject GroupCharacter;

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
}
