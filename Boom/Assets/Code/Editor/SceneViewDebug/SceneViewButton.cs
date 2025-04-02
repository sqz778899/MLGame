using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class DraggableSceneButtonsWithFold
{
    private static Rect windowRect;
    private static bool isDragging = false;
    private static Vector2 dragOffset;

    private static bool isFolded = false;

    // 存储键
    const string EditorPrefKeyX = "DraggableSceneButtons_PosX";
    const string EditorPrefKeyY = "DraggableSceneButtons_PosY";
    const string EditorPrefKeyFold = "DraggableSceneButtons_Folded";

    static DraggableSceneButtonsWithFold()
    {
        float x = EditorPrefs.GetFloat(EditorPrefKeyX, 10f);
        float y = EditorPrefs.GetFloat(EditorPrefKeyY, 10f);
        isFolded = EditorPrefs.GetBool(EditorPrefKeyFold, false);

        windowRect = new Rect(x, y, 320, isFolded ? 22 : 60);

        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        // 拖拽逻辑
        var e = Event.current;
        var dragArea = new Rect(windowRect.x, windowRect.y, windowRect.width, 20);

        if (e.type == EventType.MouseDown && dragArea.Contains(e.mousePosition))
        {
            isDragging = true;
            dragOffset = e.mousePosition - windowRect.position;
            e.Use();
        }

        if (e.type == EventType.MouseDrag && isDragging)
        {
            windowRect.position = e.mousePosition - dragOffset;
            e.Use();
        }

        if (e.type == EventType.MouseUp && isDragging)
        {
            isDragging = false;
            EditorPrefs.SetFloat(EditorPrefKeyX, windowRect.x);
            EditorPrefs.SetFloat(EditorPrefKeyY, windowRect.y);
            e.Use();
        }

        // 折叠时更小
        windowRect.height = isFolded ? 22 : 60;
        windowRect = GUILayout.Window(432198, windowRect, DrawWindow, "调试面板");

        Handles.EndGUI();
    }

    private static void DrawWindow(int id)
    {
        // 折叠按钮区域（始终可用）
        GUILayout.BeginHorizontal();
        GUILayout.Label(isFolded ? "▶ 展开" : "▼ 折叠", GUILayout.Width(60));

        if (GUILayout.Button(isFolded ? "展开" : "折叠", GUILayout.Width(50)))
        {
            isFolded = !isFolded;
            EditorPrefs.SetBool(EditorPrefKeyFold, isFolded);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (!isFolded)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("主界面"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/1.MainEnv.unity");
            }

            if (GUILayout.Button("关卡"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/2.LevelScene.unity");
            }

            if (GUILayout.Button("开始游戏"))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/0.StartGame.unity");
            }

            GUILayout.EndHorizontal();
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}