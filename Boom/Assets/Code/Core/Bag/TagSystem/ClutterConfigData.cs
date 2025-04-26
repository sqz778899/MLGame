using System;
using System.Collections.Generic;

[Serializable]
public class ClutterConfigData
{
    public int ClutterID; // 杂物ID
    public List<string> Tags; // 该杂物携带的标签，如 ["Bone", "Cursed"]
}