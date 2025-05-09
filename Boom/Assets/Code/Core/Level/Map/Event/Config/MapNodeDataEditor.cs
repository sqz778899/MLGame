﻿#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MapNodeDataConfigMono))]
[CanEditMultipleObjects]
public class MapNodeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mono = (MapNodeDataConfigMono)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ClutterTags"));
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
            case MapEventType.RoomKey:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("RoomKeyConfig"));
                break;
            case MapEventType.BasicGambling:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BasicGamblingConfig"));
                break;
            case MapEventType.StoneTablet:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StoneTabletConfig"));
                break;
            case MapEventType.MysticalInteraction:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WigglingBoxConfig"));
                break;
            case MapEventType.Shop:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ShopEventConfig"));
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