using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGenEditor : Editor
{
   public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator) target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
        }
    }
}
