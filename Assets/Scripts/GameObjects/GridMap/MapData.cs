using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MapData", menuName = "Data/GameObject/MapData", order = 5)]
public class MapData : ScriptableObject
{
    public int Width;
    public int Height;
    public int MaxMatchPoints;
    public GridWrapper[] BlocksX;
}

[Serializable]
public class GridWrapper
{
    public GridState[] BlocksY;
}


#if UNITY_EDITOR
[CustomEditor(typeof(MapData)),CanEditMultipleObjects]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target component
        MapData Data = (MapData)target;

        // Add a button to the inspector
        if (GUILayout.Button("Generate Data"))
        {
            // Call the function on the target component
            Data.BlocksX = new GridWrapper[Data.Width];
            for (int i = 0; i < Data.BlocksX.Length; i++)
            {
                Data.BlocksX[i] = new GridWrapper();
                Data.BlocksX[i].BlocksY = new GridState[Data.Height];
            }
        }
    }
}
#endif