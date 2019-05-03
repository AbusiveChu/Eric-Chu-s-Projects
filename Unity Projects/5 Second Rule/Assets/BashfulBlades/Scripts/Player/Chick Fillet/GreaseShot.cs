using UnityEngine;
using System.Collections;

public class GreaseShot : MonoBehaviour
{
    public ParticleSystem burst;

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Enemy" || other.collider.tag == "StunEnemy" || other.collider.tag == "StunEnemyBig")
        {
            Instantiate(burst, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
