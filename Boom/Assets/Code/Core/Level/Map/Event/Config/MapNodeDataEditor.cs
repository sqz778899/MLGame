#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MapNodeDataConfigMono))]
public class MapNodeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mono = (MapNodeDataConfigMono)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NodeName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Desc"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_MapEventType"));

        switch (mono._MapEventType)
        {
            case MapEventType.CoinsPile:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("GoldPileConfig"));
                break;
            case MapEventType.TreasureBox:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TreasureBoxConfig"));
                break;
            case MapEventType.Bullet:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BulletEventConfig"));
                break;
            case MapEventType.WeaponRack:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WeaponRackConfig"));
                break;
            case MapEventType.Skeleton:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SkeletonConfig"));
                break;
            case MapEventType.StoneTablet:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StoneTabletConfig"));
                break;
            case MapEventType.MysticalInteraction:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WigglingBoxConfig"));
                break;
            case MapEventType.RoomArrow:
                var arrowProp = serializedObject.FindProperty("RoomArrowConfig");
                EditorGUILayout.PropertyField(arrowProp.FindPropertyRelative("ArrowType"));
                EditorGUILayout.PropertyField(arrowProp.FindPropertyRelative("TargetRoomID"));

                RoomArrowType arrowType = mono.RoomArrowConfig.ArrowType;
                if (arrowType == RoomArrowType.Fight)
                {
                    EditorGUILayout.PropertyField(arrowProp.FindPropertyRelative("BattleConfig"));
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif