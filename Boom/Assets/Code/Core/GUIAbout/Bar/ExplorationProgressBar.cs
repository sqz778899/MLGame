using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationProgressBar : MonoBehaviour
{
    [Header("UI 组件引用")]
    public Image progressBarImage; // 引用进度条的Image组件（需设置为Filled）

    [Header("探索进度 (0到1)")]
    [Range(0f, 1f)]
    public float explorationPercent = 0f;

    [Header("颜色设置")]
    public Color colorLow = Color.white;                 // ≤70% 时纯白色
    public Color colorHigh = new Color(1f, 0.85f, 0f);   // 接近100%时金色

    void Update()
    {
        UpdateProgressBar(explorationPercent);
    }

    public void UpdateProgressBar(float percent)
    {
        // 限制进度值范围
        percent = Mathf.Clamp01(percent);

        // 设置进度条填充量
        progressBarImage.fillAmount = percent;

        // 根据进度调整颜色
        if (percent <= 0.7f)
        {
            progressBarImage.color = colorLow; // 纯白色
        }
        else
        {
            // 计算从70%到100%的渐变比例（0~1）
            float t = (percent - 0.7f) / 0.3f;
            progressBarImage.color = Color.Lerp(colorLow, colorHigh, t);
        }
    }
}