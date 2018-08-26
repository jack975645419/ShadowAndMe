using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExpectedAngleDrawer))]
public class EditorForExpectedAngleDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ExpectedAngleDrawer drawer = target as ExpectedAngleDrawer;
        //当GUI改变时
        if (GUI.changed && drawer != null)
        {
            GameManager.Instance.RefreshTables();
        }
    }
}