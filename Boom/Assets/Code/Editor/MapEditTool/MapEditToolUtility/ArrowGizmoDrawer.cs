#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

[InitializeOnLoad]
public static class ArrowGizmoDrawer
{
    private static bool _enabled = false;
    public static void SetEnabled(bool enable) => _enabled = enable;
    public static bool Enabled => _enabled;

    static ArrowGizmoDrawer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (!_enabled) return;

        var arrows = GameObject.FindObjectsOfType<MapNodeDataConfigMono>()
            .Where(m => m._MapEventType == MapEventType.RoomArrow && m.RoomArrowConfig != null);

        foreach (var arrow in arrows)
        {
            var targetID = arrow.RoomArrowConfig.TargetRoomID;
            var allRooms = GameObject.FindObjectsOfType<MapRoomNode>();
            var targetRoom = allRooms.FirstOrDefault(r => r.RoomID == targetID);

            if (targetRoom == null) continue;

            var from = arrow.transform.position;
            var to = targetRoom.transform.position;

            Color color = arrow.RoomArrowConfig.ArrowType switch
            {
                RoomArrowType.Fight => Color.red,
                RoomArrowType.KeyGate => Color.gray,
                RoomArrowType.ReturnStone => Color.magenta,
                _ => Color.cyan,
            };

            Handles.color = Selection.activeGameObject == arrow.gameObject ? Color.yellow : color;
            Handles.DrawAAPolyLine(3f, from, to);
            Handles.SphereHandleCap(0, to, Quaternion.identity, 0.2f, EventType.Repaint);
            Handles.Label(to + Vector3.up * 0.5f, $"Room {targetID}");
        }
    }
}
#endif