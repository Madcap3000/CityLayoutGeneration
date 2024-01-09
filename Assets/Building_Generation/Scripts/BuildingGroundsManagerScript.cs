using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroundsManagerScript : MonoBehaviour
{
    public List<buildingGroundLevel> _buildingGrounds;
    public static List<buildingGroundLevel> buildingGrounds;

    public void Awake()
    {
        buildingGrounds = _buildingGrounds;
    }

    [Serializable]
    public class buildingGroundLevel
    {
        [Header("Chance for the building ground to become bigger if there is place for it")]
        [SerializeField] public float chance;
        [SerializeField]public GameObject buildingGround;
        [Header("Possible buildings for the grounds")]
        [SerializeField]public List<GameObject> possibleBuildings = new List<GameObject>();
    }
}
