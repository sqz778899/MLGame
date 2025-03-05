using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoleBase
{
    public int ID;
    public string BloodGroup;
    public string ZodiacSign;
    public string MBTI;
    public string Description;
    public RoleAttri Attri;
}


public enum RoleState
{
    Idle = 0,
    MoveForward = 1,
    MoveBack = 2,
    Attack = 3
}
public class RoleAttri
{
    public int StandbyAdd;
}