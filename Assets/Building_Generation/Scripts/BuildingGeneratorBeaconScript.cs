using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGeneratorBeaconScript : MonoBehaviour
{
    public void generateBuildingGrounds()
    {
        //Check if inside bounds
        if (Math.Abs(gameObject.transform.position.x) > GenerationManagerScript.range ||
            Math.Abs(gameObject.transform.position.z) > GenerationManagerScript.range)
        {
            Destroy(gameObject);
            return;
        }
        
        //Find the list of buildings to place
        int level = 0;
        int l = 0;
        List<GameObject> list, olderlist = new List<GameObject>();
        for (; level < BuildingGroundsManagerScript.buildingGrounds.Count; level++){
            list = new List<GameObject>();
            var ground = BuildingGroundsManagerScript.buildingGrounds[level].buildingGround;
            if(PlaceBuildingGrounds(ground)) list.Add(ground);
            l++;
            if (list.Count != 0){
                olderlist = list;
                if(GenerationManagerScript.random.NextDouble() > BuildingGroundsManagerScript.buildingGrounds[level].chance) break;
                continue;
            }
            break;
        }
        level--;

        //Destroy beacon if no building could be placed
        if (olderlist.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        
        //TODO: select building grounds randomly from the olderList
        var grounds = olderlist[0];

        //place the building
        var pos = transform.position;
        var angle = transform.eulerAngles.y;
        var placed = Instantiate(grounds, pos, Quaternion.Euler(0,angle,0));
        
        //generate new beacons
        placed.GetComponent<BuildingGroundBeaconManagerScript>().GenerateBuildingBeacons();
        
        //TODO: fix the building grounds replacement
        //Debug.Log("level: " + level + " " + placed.transform.position);
        //placed.GetComponentInChildren<BuildingGroundScript>().generateBuilding(BuildingGroundsManagerScript.buildingGrounds[level].possibleBuildings);
        Destroy(gameObject);
    }

    private bool PlaceBuildingGrounds(GameObject buildingGround)
    {
        var colSize = buildingGround.GetComponentInChildren<BoxCollider>().size;
        var pos = transform.position + Util.rotateBy90((int)transform.eulerAngles.y,
            colSize.x * 0.5f - 0.5f, colSize.z * 0.5f - 0.5f);
        var angle = transform.eulerAngles.y;
        var collider = Physics.OverlapBox(pos, colSize * 0.45f,  Quaternion.Euler(0,angle,0));

        return collider.Length == 0;
    }
}
