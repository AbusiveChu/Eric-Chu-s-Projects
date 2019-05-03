using UnityEngine;
using System.Collections;

public class FireShot : MonoBehaviour 
{
    public ParticleSystem FireBoom;
    void OnCollisionEnter(Collision other)
    { 
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "StunEnemy" || other.gameObject.tag == "Level" || other.gameObject.tag == "Floor" || other.gameObject.tag == "Ground"|| other.gameObject.tag == "StunEnemyBig")
        {
            Instantiate(FireBoom, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
