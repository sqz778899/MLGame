using System.Collections.Generic;

public static class ExcellUtility
{
    public static List<int> GetListInt(string CellStr)
    {
        List<int> curList = new List<int>();
        string[] tmp = CellStr.Split(";");
        for (int i = 0; i < tmp.Length; i++)
        {
            int rInt = -1;
            int.TryParse(tmp[i],out rInt);
            curList.Add(rInt);
        }
        return curList;
    }
    
    public static List<float> GetListFloat(string CellStr)
    {
        List<float> curList = new List<float>();
        string[] tmp = CellStr.Split(";");
        for (int i = 0; i < tmp.Length; i++)
        {
            float rFloat = -1f;
            float.TryParse(tmp[i],out rFloat);
            curList.Add(rFloat);
        }
        return curList;
    }
}