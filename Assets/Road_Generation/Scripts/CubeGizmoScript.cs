using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeGizmoScript : MonoBehaviour
{
    
    public float size = 1;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawCube(Vector3.zero + transform.position, Vector3.one * size);
    }
}
