using UnityEngine;
using System.Collections;

public class BulletMov : MonoBehaviour {

    public Rigidbody Bullet;
    public float Bulletspeed = 20f;
    public float Lifespan = 5;
    public int BulletType;
    private Vector3 BulletDir;
	void Start () 
    {
        if (BulletType == 2)
        {
           //BulletDir = Network.RECVHITDIR;
        }
        else
        {
            BulletDir = Shooting.hitDir;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        Destroy(gameObject, Lifespan);
	}

    void FixedUpdate()
    {
        //print(BulletScript.hitDir);
        Bullet.AddForce(BulletDir * Bulletspeed, ForceMode.Impulse);
        
        //Bullet.AddForce(transform.forward *  Bulletspeed ,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Level" || other.gameObject.tag == "Floor" || other.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
