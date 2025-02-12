using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace Code.Editor
{
    public class MapEditToolBox : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Map Edit Tool")]
        private static void OpenWindow()
        {
            GetWindow<MapEditToolBox>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            //tree.Add("编辑房间", new MapRoomEdit());
            return tree;
        }
    }
}