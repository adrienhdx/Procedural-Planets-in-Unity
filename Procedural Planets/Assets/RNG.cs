using UnityEngine;
using UnityEditor.Presets;


//
// DISCLAIMER
//
//This is really messy but working code, feel free to upgrade and modify
//Credit for all scripts except this one, CustomRNGEditor.cs and Rotator.cs goes to Sebastian Lague and libnoise.net
//I'm not an experienced developer so don't throw your keyboard out of the window if some code sucks, I'm 15.
//Have fun, thx for downloading.
//

public class RNG : MonoBehaviour
{
    [HideInInspector()]
    public Planet planet;
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
    public Vector2 simpleStrengthMinMax = new Vector2(0.01f, 0.1f);
    public Vector2 simpleBaseRoughnessMinMax = new Vector2(0.7f, 2f);
    public Vector2 simpleRoughnessMinMax = new Vector2(2.2f, 3.2f);
    public Vector2 simpleCentreMinMax = new Vector2(0, 20);


    [Header("Ridgid Noise Attributes")]
    public Vector2 ridgidStrengthMinMax = new Vector2(0.6f, 1f);
    public Vector2 ridgidBaseRoughnessMinMax = new Vector2(0.1f, 4.5f);
    public Vector2 ridgidRoughnessMinMax = new Vector2(0, 1);
    public Vector2 ridgidPersistenceMinMax = new Vector2(.3f, .8f);
    public Vector2 ridgidCentreMinMax = new Vector2(0, 20);
    public Vector2 ridgidMinimalValueMinMax = new Vector2(0, 2);
    public Vector2 ridgidWeightMultiplierMinMax = new Vector2(0.3f, 2.5f);



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
        noiseSettings0.simpleNoiseSettings.strength = Random.Range(simpleStrengthMinMax.x, simpleStrengthMinMax.y);
        noiseSettings0.simpleNoiseSettings.baseRoughness = Random.Range(simpleBaseRoughnessMinMax.x, simpleBaseRoughnessMinMax.y);
        noiseSettings0.simpleNoiseSettings.roughness = Random.Range(simpleRoughnessMinMax.x, simpleRoughnessMinMax.y);
        noiseSettings0.simpleNoiseSettings.centre = new Vector3(Random.Range(simpleCentreMinMax.x, simpleCentreMinMax.y), Random.Range(simpleCentreMinMax.x, simpleCentreMinMax.y), Random.Range(simpleCentreMinMax.x, simpleCentreMinMax.y));


        //Ridgid noise RNG (highly unoptimised)
        noiseSettings1.ridgidNoiseSettings.strength = Random.Range(ridgidStrengthMinMax.x, ridgidStrengthMinMax.y);
        noiseSettings1.ridgidNoiseSettings.baseRoughness = Random.Range(ridgidBaseRoughnessMinMax.x, ridgidBaseRoughnessMinMax.y);
        noiseSettings1.ridgidNoiseSettings.roughness = Random.Range(ridgidRoughnessMinMax.x, ridgidRoughnessMinMax.y);
        noiseSettings1.ridgidNoiseSettings.persistence = Random.Range(ridgidPersistenceMinMax.x, ridgidPersistenceMinMax.y);
        noiseSettings1.ridgidNoiseSettings.centre = new Vector3(Random.Range(ridgidCentreMinMax.x, ridgidCentreMinMax.y), Random.Range(ridgidCentreMinMax.x, ridgidCentreMinMax.y), Random.Range(ridgidCentreMinMax.x, ridgidCentreMinMax.y));
        noiseSettings1.ridgidNoiseSettings.weightMultiplier = Random.Range(ridgidWeightMultiplierMinMax.x, ridgidWeightMultiplierMinMax.y);
        noiseSettings1.ridgidNoiseSettings.minValue = Random.Range(ridgidMinimalValueMinMax.x, ridgidMinimalValueMinMax.y);


        planet.GeneratePlanet();
        
    }

    [Header("Colour Presets")]
    public PresetsEnum presetsReference;
    public enum PresetsEnum
    {

        Earth = 0,

        Arctic = 1

    };

    public bool autoRegenerateOnPresetModification = false;
    public bool lockNoiseUpdate = true;

    //add new presets here
    [SerializeField]
    Preset[] presets;
    

    //add new conditions for new presets here
    public void ApplyPreset()
    {
        for (int i = 0; i < presets.Length; i++)
        {
            if (i == (int)presetsReference)
            {
                presets[i].ApplyTo(colour);
                if (autoRegenerateOnPresetModification && lockNoiseUpdate == false)
                {
                    Randomise();
                    Debug.Log("Planet Generated");
                }
                else if (autoRegenerateOnPresetModification && lockNoiseUpdate)
                {
                    planet.GenerateColours();
                    Debug.Log("Colours updated");
                }
            }
        }
       
    }
}
