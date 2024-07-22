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
public class Role: RoleBase
{
    public Sprite _spRole;
    

    public void InitRoleData()
    {
        List<RoleBase> roleDesign = TrunkManager.Instance.RoleDesignJsons;
        int curIndex = -1;
        for (int i = 0; i < roleDesign.Count; i++)
        {
            if (roleDesign[i].ID == ID)
            {
                curIndex = i;
                break;
            }
        }
        if (curIndex == -1) return;
        _spRole = ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetRoleImgPath(ID));
        if (_spRole == null)
        {
            _spRole = ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetRoleImgPath(99));
        }
        BloodGroup = roleDesign[curIndex].BloodGroup;
        ZodiacSign = roleDesign[curIndex].ZodiacSign;
        MBTI = roleDesign[curIndex].MBTI;
        Description = roleDesign[curIndex].Description;
        Attri = roleDesign[curIndex].Attri;
    }
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