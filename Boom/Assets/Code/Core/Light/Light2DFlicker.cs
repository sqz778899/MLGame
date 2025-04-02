using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Light2DFlicker : MonoBehaviour
{
    [Header("Sway Motion Settings")]
    public float swayAmplitude = 0.05f;     // 摆动幅度（单位距离）
    public float swaySpeed = 2.0f;          // 摆动速度
    public float swayNoiseStrength = 0.02f; // 噪声幅度（更不规则）
    public Vector2 swayDirection = new Vector2(1f, 0.5f); // 摆动方向（归一化）

    private Vector3 initialPosition;
    private float timeOffset;

    void Awake()
    {
        initialPosition = transform.localPosition;
        timeOffset = Random.Range(0f, 100f); // 避免所有灯同步
    }

    void Update()
    {
        float t = Time.time + timeOffset;
        float sine = Mathf.Sin(t * swaySpeed);
        float noise = (Mathf.PerlinNoise(t * 1.3f, 0f) - 0.5f) * 2f;

        Vector2 offsetDir = swayDirection.normalized;
        float totalOffset = sine * swayAmplitude + noise * swayNoiseStrength;

        Vector3 swayOffset = new Vector3(offsetDir.x, offsetDir.y, 0) * totalOffset;
        transform.localPosition = initialPosition + swayOffset;
    }
}