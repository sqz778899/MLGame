using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTest : MonoBehaviour
{
    [Header("资产")] public GameObject DialogueRoot;
    bool _isDialogOpen = false;

    void Start()
    {
        _isDialogOpen = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isDialogOpen)
        {
            CloseDialog();
        }
    }
    
    public void OpenDialog()
    {
        _isDialogOpen = true;
        DialogueRoot.SetActive(true);
    }
    
    public void CloseDialog()
    {
        _isDialogOpen = false;  
       DialogueRoot.SetActive(false);
    }
}
