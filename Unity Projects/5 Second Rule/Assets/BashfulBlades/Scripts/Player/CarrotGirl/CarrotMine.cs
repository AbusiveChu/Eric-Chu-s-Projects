using UnityEngine;
using System.Collections;

public class CarrotMine : MonoBehaviour {

    public Rigidbody Bullet;
    public float Bulletspeed = 20f;
    public float Lifespan = 5;
    public GameObject BulletShards;
    public int BulletType;
    private Vector3 BulletDir;
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
       // Instantiate(BulletShards, transform.position, transform.rotation);
    }

    void FixedUpdate()
    {
        //print(BulletScript.hitDir);

        Bullet.AddForce(BulletDir * Bulletspeed, ForceMode.Impulse);


        //Bullet.AddForce(transform.forward *  Bulletspeed ,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Floor" || collisionInfo.collider.tag == "default" || collisionInfo.collider.tag == "Ground")
        {
            Instantiate(BulletShards, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy" || collisionInfo.collider.tag == "StunEnemyBig")
        {
            Instantiate(BulletShards, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}