using UnityEngine;

public class ShieldView : MonoBehaviour
{
    public HealthBar HealthBar;
    public float InsStep;  //根据资源大小直接填在Prefab上
    public Color HitColor;
    public Transform HitTextPos;

    public void Init(ShieldData data) =>  HealthBar.InitHealthBar(() => data.CurHP, () => data.MaxHP);
    public void ShowHitText(int damage)
    {
        GameObject txt = ResManager.instance.CreatInstance(PathConfig.TxtHitPB);
        txt.transform.position = HitTextPos.position;
        txt.transform.SetParent(transform.parent,true);
        txt.GetComponent<FloatingDamageText>().AnimateText($"-{damage}", HitColor, 18f);
    }
}