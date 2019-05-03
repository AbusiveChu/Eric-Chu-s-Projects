using UnityEngine;
using System.Collections;

public class RailGun : MonoBehaviour {
    //Carrot Girl's Ulti Stuff
    public Rigidbody Bullet;
    public float Bulletspeed = 20f;
    public float Lifespan = 8;
    public SphereCollider RailgunSphere;
    private Vector3 BulletDir;
    public int BulletType;
    void Start()
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
    void Update()
    {
        Destroy(gameObject, Lifespan);
    }

    void FixedUpdate()
    {
        //print(BulletScript.hitDir);

        Bullet.AddForce(BulletDir * Bulletspeed, ForceMode.Impulse);


        //Bullet.AddForce(transform.forward *  Bulletspeed ,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        //// "Enemy" || hit.collider.tag == "StunEnemy" || hit.collider.tag == "StunEnemyBig"
        //if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy" || collisionInfo.collider.tag == "StunEnemyBig")
        //{
        //    Physics.IgnoreCollision(RailgunSphere, collisionInfo.collider);
        //}
    }
}
