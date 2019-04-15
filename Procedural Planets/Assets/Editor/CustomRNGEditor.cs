using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RNG))]
public class CustomRNGEditor : Editor
{
    RNG script;
    Planet planet;

    

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        script = (RNG)target;
        planet = script.planet;
     

        if (GUILayout.Button("Apply Preset", GUILayout.Height(30)))
        {
            script.ApplyPreset();
            if (script.autoRegenerateOnPresetModification == false)
            {
            Debug.Log("Preset applied. Please re-generate");
            }
        }

        if (GUILayout.Button("Generate planet", GUILayout.Height(30)))
        {
            if (script.lockNoiseUpdate)
            {
                planet.GenerateColours();
                Debug.Log("Colours updated");
            }
            else
            {
                script.Randomise();
                Debug.Log("Planet Generated");
            }

            

        }

    }
}
