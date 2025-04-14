using UnityEngine;

public class SettingLv1:MonoBehaviour,ICloseOnClickOutside
{
    public void Show()
    {
        gameObject.SetActive(true);
        UIClickOutsideManager.Register(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        UIClickOutsideManager.Unregister(this);
    }
    
    public void OnClickOutside() => Hide();
}