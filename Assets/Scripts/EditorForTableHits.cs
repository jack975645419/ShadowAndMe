using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Table_Hits))]
public class EditorForTableHits : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Table_Hits t = (Table_Hits)target;
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

