using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIBase : MonoBehaviour
{
    public virtual void OnOffWindow()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject curGO = transform.GetChild(i).gameObject;
            curGO.SetActive(!curGO.activeSelf);
        }
    }
    
    public virtual void CloseWindow()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}