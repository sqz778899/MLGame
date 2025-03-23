using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEnvStory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sss());
    }

    IEnumerator sss()
    {
        yield return new WaitForFixedUpdate();
        EventManager.OnChapterOne?.Invoke();
    }
}
