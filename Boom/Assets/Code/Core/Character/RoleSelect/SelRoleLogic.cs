using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelRoleLogic : KeyBoardBase
{
    internal override void Start()
    {
        base.Start();
        UIManager.Instance.InitSelectRole();
    }
}
