using UnityEngine;
using System.Collections;

public class Blackpepper : MonoBehaviour {
	//public SphereCollider BombCollider;
	//public Vector3 TrapSize = new Vector3(4.7f,3.0f,0.06f);
	public bool BombReady;
	public float BombDelay = 5f;
	private float nextUsage;
    public ParticleSystem Kaboom;
    private bool once = false;
    public bool TrapReady;
    
   
    // Use this for initialization
    void Start () {
		nextUsage = Time.time + BombDelay;
	}
	//0.6
	// Update is called once per frame
	void Update () {

        if (Time.time > nextUsage)
        {
            if (once == false)
            {
               
                Instantiate(Kaboom, transform.position, transform.rotation);
            }
            Destroy(gameObject);
            once = true;
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (gameObject.tag == "Ulti")
        {
            if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy" || collisionInfo.gameObject.tag == "Level" || collisionInfo.gameObject.tag == "Floor" || collisionInfo.gameObject.tag == "Ground")
            {
                if (once == false)
                {
                   
                    Instantiate(Kaboom, transform.position, transform.rotation);               
                
                }
                Destroy(gameObject);
                once = true;
            }
        }
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy")
        {
            if (once == false)
            {
               
                Instantiate(Kaboom, transform.position, transform.rotation);
            }
            Destroy(gameObject);
            once = true;
        }
    }
}
