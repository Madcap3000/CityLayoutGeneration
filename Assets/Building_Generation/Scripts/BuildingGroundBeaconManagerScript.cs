using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGroundBeaconManagerScript : MonoBehaviour
{
    public void GenerateBuildingBeacons()
    {
        //get the building ground size using the collider
        var colSize = gameObject.GetComponentInChildren<BoxCollider>().size;

        //get building generation beacon 
        var beacon = Resources.Load<GameObject>("Beacons/BuildingGeneratorBeacon");
        if (beacon == null)
        {
            Debug.Log("Could BuildingGeneratorBeacon in the Resources/Beacons");
            return;
        }

        //save all possible locations for the beacons around the building
        List<BeaconPositionAndRotation> beaconPositions = new List<BeaconPositionAndRotation>();
        for (int i = 0; i < colSize.x; i++){
            beaconPositions.Add(new BeaconPositionAndRotation(new Vector3(i, 0, -1), 90,180));
        }
        for (int i = 0; i < colSize.z; i++){
            beaconPositions.Add(new BeaconPositionAndRotation(new Vector3(colSize.x, 0, i), 0,90));
        }
        for (int i = 0; i < colSize.x; i++){
            beaconPositions.Add(new BeaconPositionAndRotation(new Vector3(colSize.x - i - 1, 0, colSize.z), 270,0));
        }
        for (int i = 0; i < colSize.z; i++){
            beaconPositions.Add(new BeaconPositionAndRotation(new Vector3(-1, 0, colSize.z - i - 1), 180,270));
        }

        //generate beacons on the premiss of the building ground starting from the left and from the left
        var rightBoundary = 0;
        for (; rightBoundary < beaconPositions.Count; rightBoundary++)
        {
            var pos = transform.position + Util.rotateBy90((int)transform.eulerAngles.y,
                beaconPositions[rightBoundary].pos.x, beaconPositions[rightBoundary].pos.z);
            var angle = correctAngle(beaconPositions[rightBoundary].rotationFromRight + transform.eulerAngles.y);
            var collider = Physics.OverlapBox(pos, Vector3.one * 0.45f,  Quaternion.identity);
            
            if(collider.Length != 0) continue;

            //this is an corner case(both figuratively and literally) by checking if we are flash with the road 
            if (rightBoundary == 0 || rightBoundary == colSize.x || rightBoundary == colSize.x + colSize.z
                || rightBoundary == 2 * colSize.x + colSize.z)
            {
                var cornerPos = pos + Util.rotateBy90((int)transform.eulerAngles.y + beaconPositions[rightBoundary].rotationFromRight,0,-1);
                collider = Physics.OverlapBox(cornerPos, Vector3.one * 0.45f,  Quaternion.identity);
                if (collider.Length == 0) break;
            }
            
            Instantiate(beacon, pos,Quaternion.Euler(0, angle, 0), transform);
            break;
        }
        for (int i = beaconPositions.Count - 1; i>=rightBoundary; i--)
        {
            var pos = transform.position + Util.rotateBy90((int)transform.eulerAngles.y,
                beaconPositions[i].pos.x, beaconPositions[i].pos.z);
            var angle = correctAngle(beaconPositions[i].rotationFromLeft + transform.eulerAngles.y);
            var collider = Physics.OverlapBox(pos, Vector3.one * 0.45f,  Quaternion.identity);
            
            
            
            if(collider.Length != 0) continue;

            //this is an corner case(both figuratively and literally) by checking if we are flash with the road 
            if (i == colSize.x - 1 || i == colSize.x + colSize.z - 1
                || i == 2 * colSize.x + colSize.z - 1 || i == 2 * colSize.x + 2 * colSize.z - 1)
            {
                var cornerPos = pos + Util.rotateBy90((int)transform.eulerAngles.y + beaconPositions[i].rotationFromLeft,-1,0);
                collider = Physics.OverlapBox(cornerPos, Vector3.one * 0.45f,  Quaternion.identity);
                if (collider.Length == 0) break;
            }

            Instantiate(beacon, pos, Quaternion.Euler(0, angle, 0), transform);
            break;
        }
    }

    private float correctAngle(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }
    
    private class BeaconPositionAndRotation
    {
        public Vector3 pos;
        public int rotationFromRight;
        public int rotationFromLeft;

        public BeaconPositionAndRotation(Vector3 pos, int rotationFromRight, int rotationFromLeft)
        {
            this.pos = pos;
            this.rotationFromRight = rotationFromRight;
            this.rotationFromLeft = rotationFromLeft;
        }
    }
}