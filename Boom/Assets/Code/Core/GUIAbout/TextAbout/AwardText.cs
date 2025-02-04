using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AwardText : MonoBehaviour
{
    public TextMeshProUGUI txtAward;

    public void SyncAwardText(int ItemID)
    {
        ItemSS curItemSs = new ItemSS(ItemID);
        txtAward.text = curItemSs.name; 
    }
}
