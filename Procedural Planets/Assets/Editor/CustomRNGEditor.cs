using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RNG))]
public class CustomRNGEditor : Editor
{
    RNG script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (RNG)target;
        if (GUILayout.Button("Randomise ! (press play before)"))
        {
            script.UpdateNoise();
        }
    }
}
