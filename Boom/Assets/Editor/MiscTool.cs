using UnityEditor;
using Sirenix.OdinInspector.Editor;

public class MiscToolBox : OdinMenuEditorWindow
{
    [MenuItem("Tools/myTool")]
    private static void OpenWindow()
    {
        GetWindow<MiscToolBox>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        tree.Add("FXTranslate", new FXTranslateEditor());

        return tree;
    }
}
