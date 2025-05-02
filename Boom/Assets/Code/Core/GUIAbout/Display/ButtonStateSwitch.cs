using System;
using UnityEngine;

public class ButtonStateSwitch : MonoBehaviour
{
    UILockedState _state;
    public UILockedState State
    {
        set
        {
            if (_state != value)
            {
                _state = value;
                ChangeState();
            }
        }
        get { return _state; }
    }
    
    [Header("Button 资产")]
    public GameObject Btn_Locked;
    public GameObject Btn_Selected;
    void Awake() => ChangeState();

    void ChangeState()
    {
        switch (State)
        {
            case UILockedState.isNormal:
                Btn_Locked.SetActive(false);
                Btn_Selected.SetActive(false);
                break;
            case UILockedState.isLocked:
                Btn_Locked.SetActive(true);
                Btn_Selected.SetActive(false);
                break;
            case UILockedState.isSelected:
                Btn_Locked.SetActive(false);
                Btn_Selected.SetActive(true);
                break;
        }
    }
}
