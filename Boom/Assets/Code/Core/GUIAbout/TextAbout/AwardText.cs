using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AwardText : MonoBehaviour
{
    public TextMeshProUGUI txtAward;

    public void SyncAwardText(int ItemID)
    {
        ItemJson itemDesignJson = TrunkManager.Instance.ItemDesignJsons
            .FirstOrDefault(each => each.ID == ItemID) ?? new ItemJson();
        txtAward.text = itemDesignJson.Name; 
    }
}
