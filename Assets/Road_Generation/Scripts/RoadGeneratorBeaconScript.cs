using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class RoadGeneratorBeaconScript : MonoBehaviour
{
    [SerializeField]public List<RoadProbability> nextRoads;

    public int minimalClearanceSide = 2; 

    public List<GameObject> GenerateRoad()
    {
        List<GameObject> results = new List<GameObject>();

        if (nextRoads.Capacity == 0)
        {
            Debug.Log("No successor road were found");  
            return results;
        }

        var _road = nextRoads[0].roadPrefab;
        
        //check if the road is outside of city boundary
        if (Math.Abs(transform.position.x) > GenerationManagerScript.range ||
            Math.Abs(transform.position.z) > GenerationManagerScript.range)
        {
            return results;
        }
        
        //check if the road CAN be placed
        var collisions = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity);
        foreach (var col in collisions)
        {
            if (col.gameObject.CompareTag("Road"))
            {
                var originBeacon = Resources.Load<GameObject>("Beacons/RoadDestructionBeacon");
                
                //Delete the generation beacon if one of them would be in the same location as the road
                GameObject beacon = null;
                foreach (var child in col.gameObject.transform.parent)
                {
                    if (((Transform)child).gameObject.CompareTag("RoadBeacon") &&
                        Vector3.Distance(((Transform)child).position, transform.parent.position) < 0.1f)
                    {
                        beacon = ((Transform)child).gameObject;
                        beacon.tag = "Untagged";
                        beacon.SetActive(false);
                    }
                }
                Instantiate(originBeacon, transform.parent.position, transform.rotation, col.gameObject.transform.parent);
                var _script = col.gameObject.transform.parent.GetComponent<RoadCoreManagerScript>();
                if(_script != null) _script.AdjustRoadCore();
                return results;
            }
        }
        
        //check if the road should be placed there
        collisions = Physics.OverlapBox(transform.position, new Vector3(2*minimalClearanceSide + 1, 1,1) * 0.45f,  Quaternion.Euler(0,90 + transform.rotation.eulerAngles.y,0));
        foreach (var col in collisions)
        {
            if (col.gameObject.CompareTag("Road"))
            {
                var beacons = new List<GameObject>();
                foreach (var child in gameObject.transform.parent.transform){
                    if(((Transform)child).gameObject.CompareTag("RoadBeacon")) beacons.Add(((Transform)child).gameObject);
                }

                if (beacons.Count <= 2){
                    var destruction = gameObject.transform.parent.GetComponentInChildren<RoadDestructionBeaconScript>();
                    destruction.DestroyRoadsTillCrossing();
                }
                else{
                    var parent = gameObject.transform.parent.GetComponent<RoadCoreManagerScript>();
                    gameObject.tag = "Untagged";
                    StartCoroutine(AdjustLater(parent));
                }

                return results;
            }
        }

        //select the road to be generated
        var decisionBoundary = GenerationManagerScript.random.NextDouble();
        var sum = 0.0;
        foreach (var road in nextRoads)
        {
            sum += road.probability;
            if (sum >= decisionBoundary)
            {
                _road = road.roadPrefab;
                break;
            }
        }

        //generate road
        var obj = Resources.Load<GameObject>(_road);
        if (obj == null)
        {
            Debug.Log("Cannot find object with name \"" + _road + "\" in Resources");
            return results;
        }
        GameObject _newRoad = Instantiate(obj, transform.position, transform.rotation);

        foreach (var child in _newRoad.transform)
        {
            if (((Transform)child).gameObject.GetComponent<RoadGeneratorBeaconScript>())
            {
                results.Add(((Transform)child).gameObject);
            }
        }

        return results;
    }

    private IEnumerator AdjustLater(RoadCoreManagerScript parent){
        if(parent != null) parent.AdjustRoadCore();
        yield return new WaitForSeconds(.00005f);
        Destroy(gameObject);
    }

    [Serializable]
    public class RoadProbability
    {
        public float probability;
        public string roadPrefab;
    }
}
