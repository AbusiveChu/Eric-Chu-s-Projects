using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockControl : MonoBehaviour
{
    public BoidMovement _BoidMovement_prefab;
    public  int FlockSize = 10;

    //
    public float _cohesion_radius = 30;
    public float _cohesion_weight = 30;
    public float _alignment_weight = 1000;
    public float _separation_radius = 20;
    public float _separation_weight = 50;
    public float _max_acceleration = 20;
    //
    public int randomness;
    public GameObject Target;
    public Vector3 flockCenter;
    public Vector3 flockVelocity;
    //
 
    public BoidMovement[] Boid;
    public static bool Check = false;

    void Start()
    {

       // Target = GameObject.FindGameObjectWithTag("Respawn");
        Boid = new BoidMovement[FlockSize];
        for (int i = 0; i < FlockSize; i++)
        {
            BoidMovement BoidMovement = Instantiate(_BoidMovement_prefab, transform.position, transform.rotation) as BoidMovement;
            BoidMovement.transform.parent = transform;

            
            Boid[i] = BoidMovement;
        }
    }
    void Update()
    {
        //Target = GameObject.FindGameObjectWithTag("Respawn");
        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;



        foreach (BoidMovement BoidMovement in Boid)
        {
            theCenter = theCenter + BoidMovement.transform.localPosition;
            theVelocity = theVelocity + BoidMovement.GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (FlockSize);
        flockVelocity = theVelocity / (FlockSize);

    
    }
}
