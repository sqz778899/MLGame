using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public static class EnemyStatsGizmoDrawer
{
    static bool isEnabled = false;
    static GUIStyle labelStyle;

    static EnemyStatsGizmoDrawer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public static void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        SceneView.RepaintAll();
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (!isEnabled) return;

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                normal = new GUIStyleState { textColor = Color.red }
            };
        }

        Handles.BeginGUI();
        foreach (var config in GameObject.FindObjectsOfType<MapNodeDataConfigMono>())
        {
            if (config._MapEventType != MapEventType.RoomArrow ||
                config.RoomArrowConfig.ArrowType != RoomArrowType.Fight)
                continue;

            var enemyConfig = config.RoomArrowConfig.BattleConfig;
            if (enemyConfig == null) continue;

            Transform arrowTransform = config.transform;
            Vector3 worldPos = arrowTransform.position + new Vector3(0, 1.5f, 0); // 调整显示位置
            Vector2 guiPos = HandleUtility.WorldToGUIPoint(worldPos);

            string shieldDetails = string.Join(", ", enemyConfig.ShieldConfig?.ShieldsHPs ?? new List<int>());
            string label = $"HP: {enemyConfig.HP}\nShield: [{shieldDetails}]";
            GUI.Label(new Rect(guiPos.x, guiPos.y, 150, 40), label, labelStyle);
        }
        Handles.EndGUI();
    }
}