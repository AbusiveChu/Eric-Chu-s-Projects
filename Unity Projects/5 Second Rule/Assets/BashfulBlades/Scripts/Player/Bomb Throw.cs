using UnityEngine;
using System.Collections;

public class BombThrow : MonoBehaviour 
{
    public static Vector3 BallisticVel(Transform startPos, Transform target, float angle)
    {
        Vector3 dir = target.position - startPos.position;
        //float h = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }
}
