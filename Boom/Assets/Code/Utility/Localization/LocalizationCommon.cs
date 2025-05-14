using System;
using System.Collections.Generic;

[Serializable]
public class LocDataJson
{
    public Dictionary<string,LocTable> LocTableDict;

    public LocDataJson()
    {
        LocTableDict = new Dictionary<string, LocTable>();
    }
}

[Serializable]
public class LocTable
{
    public string TableName;
    public Dictionary<string,MultipleLanguage> LocDict;
    public LocTable()
    {
        TableName = "";
        LocDict = new Dictionary<string, MultipleLanguage>();
    }
}

[Serializable]
public class MultipleLanguage
{
    public string zh;
    public string en;
    public string ja;

    public MultipleLanguage()
    {
        zh = "";
        en = "";
        ja = "";
    }
}
