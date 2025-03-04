using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideLogic : MonoBehaviour
{
    public void GoToTask()
    {
        MSceneManager.Instance.LoadScene(2);
    }
}
