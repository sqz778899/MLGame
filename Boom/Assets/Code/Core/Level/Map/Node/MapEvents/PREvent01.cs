using System;
using UnityEngine;

public class PREvent01: MonoBehaviour
{
    public GameObject ePart01;
    public GameObject ePart02_Accept;
    public GameObject ePart03_Accept;
    public GameObject ePart02_Quit;

    void Start()
    {
        ePart01.SetActive(true);
        ePart03_Accept.SetActive(false);
        ePart02_Accept.SetActive(false);
        ePart02_Quit.SetActive(false);
    }

    public void PRAccept01()
    {
        ePart01.SetActive(false);
        ePart02_Accept.SetActive(true);
        MainRoleManager.Instance.CurRollPREveIDs.Add(1);
    }
    
    public void PRAccept02()
    {
        ePart02_Accept.SetActive(false);
        ePart03_Accept.SetActive(true);
        MainRoleManager.Instance.AddPREve(1);
    }
    
    public void PRQuit01()
    {
        ePart01.SetActive(false);
        ePart02_Quit.SetActive(true);
    }
    
    public void EndEvent()
    {
        REvent curR = GetComponent<REvent>();
        curR.CurEventNode.GetComponent<EventNode>().QuitEvent();
        DestroyImmediate(this.gameObject);
    }
}