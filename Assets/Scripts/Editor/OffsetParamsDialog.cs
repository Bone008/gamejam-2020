using UnityEditor;
using UnityEngine;

public class OffsetParamsDialog : EditorWindow
{
    private Vector3 offPosition = Vector3.zero;
    private Vector3 offRotation = Vector3.zero;

    void OnGUI()
    {
        offPosition = EditorGUILayout.Vector3Field("Position Offset", offPosition);
        offRotation = EditorGUILayout.Vector3Field("Rotation Offset", offRotation);
        if (GUILayout.Button("Apply"))
        {
            EditorUtil.ExecOffset(offPosition, offRotation);
            Close();
        }
    }
}