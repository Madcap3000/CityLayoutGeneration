using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroundScript : MonoBehaviour
{
    public void generateBuilding(List<GameObject> buildings){
        if(buildings.Count == 0) return;
        foreach (var child in transform)
        {
            var childObj = ((Transform)child).gameObject;
            if (childObj.CompareTag("Buildings"))
            {
                var res = (int)Math.Floor(GenerationManagerScript.random.NextDouble() * buildings.Count);

                Instantiate(buildings[res], childObj.transform.position, childObj.transform.rotation, transform);
                childObj.SetActive(false);
                return;
            }
        }
    }
}
