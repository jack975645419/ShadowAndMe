using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Table_Paowuxian))]
public class EditorForTablePaowuxian : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Table_Paowuxian t = (Table_Paowuxian)target;
        if (GUILayout.Button("Add"))
        {
            t.AddOnEditor();
        }
        if (GUILayout.Button("Refresh"))
        {
            t.RefreshOnEditor();
        }
    }
}
