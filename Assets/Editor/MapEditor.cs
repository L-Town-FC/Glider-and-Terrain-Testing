using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoostSpawner))]
public class MapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        BoostSpawner map = target as BoostSpawner;
        //base.OnInspectorGUI(); causes update every frame that inspector is open which is bad for performance
        if (DrawDefaultInspector()) //returns true whenever only when a change is made
        {
            map.GenerateMap();

        }

        if (GUILayout.Button("Generate Map")) //makes it so map will regenerate when button is selected
        {
            map.GenerateMap();
        }

    }
}
