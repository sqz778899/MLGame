using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutsideLogic : MonoBehaviour
{
    public GameObject BuildRoot; 
    public List<BuildBase> _builds;

    void Start()
    {
        _builds = new List<BuildBase>();
        for (int i = 0; i < BuildRoot.transform.childCount; i++)
            _builds.Add(BuildRoot.transform.GetChild(i).GetComponent<BuildBase>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _builds.ForEach(menu => menu.CloseBuild());
        }
    }
}
