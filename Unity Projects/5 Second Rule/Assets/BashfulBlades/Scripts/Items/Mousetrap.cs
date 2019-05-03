using UnityEngine;
using System.Collections;

public class Mousetrap : MonoBehaviour 
{
	public BoxCollider TrapCollider;
	public bool TrapReady;
	
    public ParticleSystem Kaboom;
    private bool once = false;

	void OnCollisionEnter (Collision collisionInfo)
	{
		if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy")
		{
            TrapCollider.size = new Vector3(12f, 0.2563867f, 12f);
            if (once == false)
            {
                Instantiate(Kaboom, transform.position, transform.rotation);
            }
            once = true;
            Destroy(gameObject, 1.0f);
		}
	}
}
