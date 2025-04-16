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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EventType"));

        switch (mono.EventType)
        {
            case MapEventType.CoinsPile:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("GoldPileConfig"));
                break;
            case MapEventType.WeaponRack:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WeaponRackConfig"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif