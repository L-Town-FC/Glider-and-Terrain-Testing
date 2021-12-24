using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator meshGenerator = target as MeshGenerator;
        //base.OnInspectorGUI(); causes update every frame that inspector is open which is bad for performance
        if (DrawDefaultInspector()) //returns true whenever only when a change is made
        {
            meshGenerator.GenerateMap();

        }

        if (GUILayout.Button("Generate Map")) //makes it so map will regenerate when button is selected
        {
            meshGenerator.GenerateMap();
        }

    }
}
