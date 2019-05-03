using UnityEngine;
using System.Collections;

public class RadiusDrawer : MonoBehaviour {
    
    //Draw radius for debugging
    void OnDrawGizmosSelected()
    {       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyAI.Offset);
        Gizmos.color = Color.red;
    }
}
