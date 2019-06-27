using UnityEngine;
using UnityEditor.Presets;


//
// DISCLAIMER
//
//This is really messy but working code, feel free to upgrade and modify
//Made by Adrien Houdoux with the help of Sebastian Lague and libnoise.net.
//I'm not an experienced developer so don't throw your keyboard out of the window if some code sucks, I'm 15.
//Thanks for trying it out
//

public class RNG : MonoBehaviour
{
    [HideInInspector()]
    public Planet planet;

    public UpdateMeshCollider[] meshUpdaters;
    ShapeSettings shape;
    ColourSettings colour;

    NoiseSettings noiseSettings0;
    NoiseSettings noiseSettings1;


    //0 is Simple Noise, 1 is Ridgid Noise. We are only using 2 layers otherwise it would be too heavy


    [Header("Global Graphics settings")]
    public bool maxResolution = false;
    public bool staticLOD = false;
    [Range(1,8)]
    public int staticLODValue = 1;
    

    [Header("X is min. value, Y is max.")]
    //public Vector2 planetRadiusMinMax = new Vector2(2.4f, 3.7f);

    [Header("Simple Noise Attributes")]
    public Vector2 s_Strength = new Vector2(0.01f, 0.1f);
    public Vector2 s_BaseRoughness = new Vector2(0.7f, 2f);
    public Vector2 s_Roughness = new Vector2(2.2f, 3.2f);
    public Vector2 s_Centre = new Vector2(0, 20);


    [Header("Ridgid Noise Attributes")]
    public Vector2 r_Strength = new Vector2(0.6f, 1f);
    public Vector2 r_BaseRoughness = new Vector2(0.1f, 4.5f);
    public Vector2 r_Roughness = new Vector2(0, 1);
    public Vector2 r_Persistence = new Vector2(.3f, .8f);
    public Vector2 r_Centre = new Vector2(0, 20);
    public Vector2 r_MinimalValue = new Vector2(0, 2);
    public Vector2 r_WeightMultiplier = new Vector2(0.3f, 2.5f);



    private void Start()
    {
        planet = GetComponent<Planet>();
        shape = planet.shapeSettings;
        colour = planet.colourSettings;
        noiseSettings0 = shape.noiseLayers[0].noiseSettings;
        noiseSettings1 = shape.noiseLayers[1].noiseSettings;

       

    }


    public void Randomise()
    {
        //shape.planetRadius = Random.Range(planetRadiusMinMax.x, planetRadiusMinMax.y);

        if (maxResolution)
        {
            int lod = 8;
            Debug.Log("Max resolution (256) set, expect delays. LOD set to max value (8).");
            planet.resolution = 256;
            noiseSettings0.simpleNoiseSettings.numLayers = lod;
            noiseSettings1.ridgidNoiseSettings.numLayers = lod;
        }
        else if (staticLOD)
        {
            int lod = staticLODValue;
            Debug.Log("Static LOD value enabled");
            planet.resolution = 80 + (lod * 22); //Bad math stuff but works accordingly to LOD value
            noiseSettings0.simpleNoiseSettings.numLayers = lod;
            noiseSettings1.ridgidNoiseSettings.numLayers = lod;
        } else
        {

            int lod = Random.Range(3, 7);
            Debug.Log(lod);
            planet.resolution = 80 + (lod * 22); //Bad math stuff but works accordingly to LOD value
            noiseSettings0.simpleNoiseSettings.numLayers = lod;
            noiseSettings1.ridgidNoiseSettings.numLayers = lod;
        }


        //Simple noise RNG (highly unoptimised)
        noiseSettings0.simpleNoiseSettings.strength = Random.Range(s_Strength.x, s_Strength.y);
        noiseSettings0.simpleNoiseSettings.baseRoughness = Random.Range(s_BaseRoughness.x, s_BaseRoughness.y);
        noiseSettings0.simpleNoiseSettings.roughness = Random.Range(s_Roughness.x, s_Roughness.y);
        noiseSettings0.simpleNoiseSettings.centre = new Vector3(Random.Range(s_Centre.x, s_Centre.y), Random.Range(s_Centre.x, s_Centre.y), Random.Range(s_Centre.x, s_Centre.y));


        //Ridgid noise RNG (highly unoptimised)
        noiseSettings1.ridgidNoiseSettings.strength = Random.Range(r_Strength.x, r_Strength.y);
        noiseSettings1.ridgidNoiseSettings.baseRoughness = Random.Range(r_BaseRoughness.x, r_BaseRoughness.y);
        noiseSettings1.ridgidNoiseSettings.roughness = Random.Range(r_Roughness.x, r_Roughness.y);
        noiseSettings1.ridgidNoiseSettings.persistence = Random.Range(r_Persistence.x, r_Persistence.y);
        noiseSettings1.ridgidNoiseSettings.centre = new Vector3(Random.Range(r_Centre.x, r_Centre.y), Random.Range(r_Centre.x, r_Centre.y), Random.Range(r_Centre.x, r_Centre.y));
        noiseSettings1.ridgidNoiseSettings.weightMultiplier = Random.Range(r_WeightMultiplier.x, r_WeightMultiplier.y);
        noiseSettings1.ridgidNoiseSettings.minValue = Random.Range(r_MinimalValue.x, r_MinimalValue.y);


        planet.GeneratePlanet();

        foreach (UpdateMeshCollider meshUpdater in meshUpdaters)
        {
            meshUpdater.RecalculateBounds();
        }
        
    }

    [Header("Colour Presets")]
    public PresetsEnum presetSelector;
    public enum PresetsEnum
    {

        
        Earth = 0,

        Arctic = 1,

        Venus = 2

    };

    public bool autoRegenerate = false;
    public bool lockNoiseUpdate = true;

    
    [SerializeField]
    Preset[] presets;
    

    //add new conditions for new presets here
    public void ApplyPreset()
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (i == (int)presetSelector)
            {
                presets[i].ApplyTo(colour);
                if (autoRegenerate && !lockNoiseUpdate)
                {
                    Randomise();
                    Debug.Log("Planet Generated");
                }
                else if (!autoRegenerate && lockNoiseUpdate)
                {
                    planet.GenerateColours();
                    Debug.Log("Colours updated");
                }
            }
        }
       }

}

