using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public static class DiaRoleMap
{
    static List<DiaRole> _diaRoles;

    public static void InitData() =>
        _diaRoles = JsonConvert.DeserializeObject<List<DiaRole>>
            (File.ReadAllText(PathConfig.DiaRoleDesignJson));
    
    
}