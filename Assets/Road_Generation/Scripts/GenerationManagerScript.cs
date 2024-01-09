using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenerationManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]public static Random random;
    [SerializeField]public int randomSeed = 1;
    [SerializeField]public int n = 10;
    public static int range;
    
    public List<GameObject> activeGenerationBeacon;
    public List<GameObject> activeBuildingGenerationBeacon;
    void Awake()
    {
        range = n;
        activeGenerationBeacon = new List<GameObject>();
        GenerationManagerScript.random = new Random(randomSeed);
        foreach (var road in GameObject.FindGameObjectsWithTag("Road"))
        {
            foreach (var child in road.transform)
            {
                if ( ((Transform)child).gameObject.GetComponentInChildren<RoadGeneratorBeaconScript>())
                {
                    activeGenerationBeacon.Add(((Transform)child).gameObject);
                }
            }
        }
        
        StartCoroutine(GenerationRoadStep());
    }

    IEnumerator GenerationRoadStep()
    {
        if (n > 0 && activeGenerationBeacon.Count != 0)
        {
            var list = new List<GameObject>();
            foreach (var beacon in activeGenerationBeacon)
            {
                list.AddRange(beacon.GetComponent<RoadGeneratorBeaconScript>().GenerateRoad());
                yield return new WaitForSeconds(.0025f);
            }
            activeGenerationBeacon = list;
        }
        yield return new WaitForSeconds(.01f);
        if (n > 0 && activeGenerationBeacon.Count != 0) StartCoroutine(GenerationRoadStep());
        else StartCoroutine(GenerationBuildingStep());
    }

    IEnumerator GenerationBuildingStep()
    {
        activeBuildingGenerationBeacon = new List<GameObject>();
        activeBuildingGenerationBeacon.AddRange(GameObject.FindGameObjectsWithTag("BuildingBeacon"));

        if (activeBuildingGenerationBeacon.Count != 0)
        {
            foreach (var beacon in activeBuildingGenerationBeacon)
            {
                beacon.GetComponent<BuildingGeneratorBeaconScript>().generateBuildingGrounds();
                yield return new WaitForSeconds(.00025f);
            }
            
            yield return new WaitForSeconds(0.0025f);
            StartCoroutine(GenerationBuildingStep());
        }
    }
}
