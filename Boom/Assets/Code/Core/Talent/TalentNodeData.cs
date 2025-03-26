using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TalentNodeData : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public List<string> connectedNodeIds;
    public TalentType type;
    public int unlockCost;
}
