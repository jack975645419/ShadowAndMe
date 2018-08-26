using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Table_Touzhiwu))]
public class EditorForTableTouzhiwu : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Table_Touzhiwu t = (Table_Touzhiwu)target;
        if (GUILayout.Button("Add"))
        {
            t.AddOnEditor();
        }
        if(GUILayout.Button("Refresh"))
        {
            GameManager.Instance.RefreshTables();
        }
        if(GUI.changed)
        {
            GameManager.Instance.RefreshTables();
        }
    }
}

