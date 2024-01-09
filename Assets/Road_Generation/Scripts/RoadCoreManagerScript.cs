using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCoreManagerScript : MonoBehaviour
{
    public void AdjustRoadCore()
    {
        var beacons = new List<GameObject>();
        GameObject oldCore = null;
        
        //get all needed data from parent object
        foreach (var child in transform)
        {
            if (((Transform)child).gameObject.CompareTag("RoadBeacon"))
            {
                beacons.Add(((Transform)child).gameObject);
            }
            if (((Transform)child).gameObject.CompareTag("Road"))
            {
                oldCore = (((Transform)child).gameObject);
            }
        }

        
        //calculate new road orientation
        int rotation = (int)oldCore.transform.parent.rotation.eulerAngles.y;
        bool left = false, right = false;
        foreach (var beacon in beacons){
            var _pos = beacon.transform.localPosition;
            if (_pos.z < -0.5f){
                right = true;
            }
            else if (_pos.z > 0.5f){
                left = true;
            }
        }
        
        //select the new road core
        GameObject newCore;
        switch (beacons.Count)
        {
            case 4:
                newCore = Resources.Load<GameObject>("RoadCores/RoadCore_X");
                break;
            case 3:
                newCore = Resources.Load<GameObject>("RoadCores/RoadCore_T");
                if (right && left) rotation += 90;
                else if (!right) rotation += 180;
                break;
            case 2:
                var x = Math.Abs((beacons[0].transform.position - beacons[1].transform.position).x);
                var y = Math.Abs((beacons[0].transform.position - beacons[1].transform.position).z);
                if (x > 1.5f || y > 1.5f){
                    newCore = Resources.Load<GameObject>("RoadCores/RoadCore_I");
                }
                else{
                    newCore = Resources.Load<GameObject>("RoadCores/RoadCore_L");
                    if (left) rotation += 90;
                }
                break;
            default:
                newCore = Resources.Load<GameObject>("RoadCores/RoadCore_I");
                break;
        }

        Instantiate(newCore, transform.position, Quaternion.Euler(0,rotation,0), transform);
        DestroyImmediate(oldCore);
    }
}
