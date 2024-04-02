using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

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
