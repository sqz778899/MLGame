using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public Vector3 forward = new Vector3(1, 0, 0);
    public BulletData _bulletData;

    public void InitBulletData()
    {
        if (_bulletData == null)
        {
            _bulletData = new BulletData();
            _bulletData.ID = 1;
        }
        _bulletData.SetDataByID();
    }
}