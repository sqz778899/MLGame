using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace Code.Editor
{
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
            tree.Add("ExcelExport", new ExcelExport());
            tree.Add("DesignTool", new DesignTool());
            tree.Add("主界面策划工具", new MainEnvTool());
            tree.Add("FXTranslate", new FXTranslateEditor());
            tree.Add("Debug", new DebugTool());
            return tree;
        }
    }
}