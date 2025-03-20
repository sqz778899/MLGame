using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEnvStory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void sss()
    {
        EventManager.OnChapterOne?.Invoke(); 
    }
}
