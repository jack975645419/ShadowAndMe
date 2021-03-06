﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
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
            GameManager.Instance.RefreshTables();
        }
        if(GUI.changed)
        {
            GameManager.Instance.RefreshTables();
        }
    }
}
#endif