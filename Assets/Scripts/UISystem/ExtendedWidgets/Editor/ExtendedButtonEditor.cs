using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Isekai.UI.ExtendedWidgets
{
    [CustomEditor(typeof(ExtendedButton))]
    public class ExtendedButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty OnClick = serializedObject.FindProperty("OnClick");
            SerializedProperty OnDoubleClick = serializedObject.FindProperty("OnDoubleClick");
            SerializedProperty DoubleClickGap = serializedObject.FindProperty("DoubleClickGap");

            EditorGUILayout.PropertyField(OnClick);
            EditorGUILayout.PropertyField(OnDoubleClick);
            EditorGUILayout.PropertyField(DoubleClickGap);
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

}
