using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static Vector3 rotateBy90(int angle, float x, float z)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        switch (angle)
        {
            case 90:
                return new Vector3(z,0, -x);
            case 180:
                return new Vector3(-x,0, -z);
            case 270:
                return new Vector3(-z,0, x);
            default:
                return new Vector3(x,0, z);
        }
    }
}
