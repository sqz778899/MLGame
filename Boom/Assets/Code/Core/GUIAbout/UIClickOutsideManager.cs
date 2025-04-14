using System.Collections.Generic;
using UnityEngine;

public class UIClickOutsideManager : MonoBehaviour
{
    static List<ICloseOnClickOutside> tracked = new();
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键
        {
            Vector2 mousePos = Input.mousePosition;

            for (int i = tracked.Count - 1; i >= 0; i--)
            {
                var popup = tracked[i] as MonoBehaviour;
                if (popup == null)
                {
                    tracked.RemoveAt(i);
                    continue;
                }

                RectTransform rect = popup.transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, mousePos))
                    continue;

                // 点击在外部，触发关闭
                tracked[i].OnClickOutside();
            }
        }
    }

    public static void Register(ICloseOnClickOutside target)
    {
        if (!tracked.Contains(target))
            tracked.Add(target);
    }

    public static void Unregister(ICloseOnClickOutside target) => tracked.Remove(target);
}

public interface ICloseOnClickOutside
{
    void Show();
    void Hide();
    /// 面板点击外部后触发的关闭逻辑
    void OnClickOutside();
}
