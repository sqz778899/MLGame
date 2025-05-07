using TMPro;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    [Header("依赖资产")]
    public TextMeshProUGUI winCountText;

    public void InitData()
    {
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged -= Simulate;
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged += Simulate;
        Simulate();
    }
    void OnDestroy() => StopSimulate();

    public void StopSimulate() =>
        GM.Root.InventoryMgr._BulletInvData.OnModifiersChanged -= Simulate;
  
    public void Simulate()
    {
        float s = BattleSimulator.SimulateBattle();
        winCountText.text = $"胜率：{s * 100}%";
    }
}
