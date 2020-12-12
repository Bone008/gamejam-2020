using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorUtil {

    [MenuItem ("Util/Offset Elements ...")]
    public static void OffsetElements()
    {
        OffsetParamsDialog w = EditorWindow.GetWindow<OffsetParamsDialog>();
        w.Focus();
    }
    [MenuItem ("Util/Offset Elements ...", true)]
    public static bool ValidateOffsetElements()
    {
        return Selection.transforms.Length > 0;
    }

    public static void ExecOffset(Vector3 position, Vector3 rotation)
    {
        foreach(var transform in Selection.transforms)
        {
            transform.localPosition += position;
            transform.localEulerAngles += rotation;
        }
    }
}
