using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerGizmoScript : MonoBehaviour
{
    public float size = 1;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
        Gizmos.DrawWireSphere(transform.position, size*0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Util.rotateBy90((int)transform.eulerAngles.y, 1,1) * size);
    }
}
