using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowGizmoScript : MonoBehaviour
{
    // Start is called before the first frame update

    public float size = 1;
    private Vector3 location = new Vector3(0.5f, 0.5f, 0);
    public static Mesh triangle = null; 

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (triangle == null)
        {
            Vector3 p0 = (new Vector3(0, 0, 0)) * size;
            Vector3 p1 = (new Vector3(-1, 0, 1)) * size;
            ;
            Vector3 p2 = (new Vector3(-1, 0, -1)) * size;
            ;
            Vector3 p3 = (new Vector3(0, 1, 0)) * size;
            ;
            Vector3 p4 = (new Vector3(-1, 1, 1)) * size;
            ;
            Vector3 p5 = (new Vector3(-1, 1, -1)) * size;
            ;

            triangle = new Mesh();
            triangle.vertices = new Vector3[] { p0, p1, p2, p3, p4, p5 };
            triangle.triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 1,
                3, 4, 1,
                3, 2, 5,
                2, 1, 5,
                4, 5, 1,
                3, 5, 4
            };

            triangle.RecalculateNormals();
            triangle.RecalculateBounds();
            triangle.Optimize();
        }

        Gizmos.DrawMesh(triangle, transform.position, transform.rotation);
    }
}
