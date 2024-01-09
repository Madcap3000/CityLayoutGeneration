using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadDestructionBeaconScript : MonoBehaviour
{
    public void DestroyRoadsTillCrossing()
    {
        //find a parent road
        var collisions = Physics.OverlapBox(transform.position, Vector3.one * 0.45f, Quaternion.identity);
        GameObject origin = null;
        foreach (var col in collisions){
            if (col.gameObject.CompareTag("Road")){
                origin = col.transform.parent.gameObject;
            }
        }
        
        if(origin == null) return;

        var beacons = new List<GameObject>();
        foreach (var child in origin.transform){
            if(((Transform)child).gameObject.CompareTag("RoadBeacon")) beacons.Add(((Transform)child).gameObject);
        }

        if (beacons.Count <= 2){
            StartCoroutine(DestroyLater());
            origin.GetComponentInChildren<RoadDestructionBeaconScript>().DestroyRoadsTillCrossing();
        }
        else{
            StartCoroutine(DestroyLater());
            foreach (var beacon in beacons)
            {
                if (Vector3.Distance(beacon.transform.position, transform.parent.position) < 0.1f)
                {
                    beacon.SetActive(false);
                    beacon.tag = "Untagged";
                }
            }
            origin.GetComponent<RoadCoreManagerScript>().AdjustRoadCore();
        }
    }

    private IEnumerator DestroyLater()
    {
        transform.parent.gameObject.SetActive(false);
        Destroy(transform.parent.gameObject);
        yield return new WaitForSeconds(1f);
    }
}
