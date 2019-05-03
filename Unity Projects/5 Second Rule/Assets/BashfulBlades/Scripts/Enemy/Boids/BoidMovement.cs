using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BoidMovement : MonoBehaviour
{
    public static List<Rigidbody> _boids = new List<Rigidbody>();
    private FlockControl _boid_controller;
    public Vector3 acceleration;
    public int type;
    private static int counter = 0;

    void Start()
    {
        _boid_controller = GetComponentInParent<FlockControl>();
        _boids.Add(GetComponent<Rigidbody>());
        Vector3 vel = Random.insideUnitSphere;
        vel *= Random.Range(1, 10);
        GetComponent<Rigidbody>().velocity = vel;
      
     
    }


    void FixedUpdate()
    {

        
            acceleration += Separation() * _boid_controller._separation_weight;
            acceleration += Calc();
            acceleration = Vector3.ClampMagnitude(acceleration, _boid_controller._max_acceleration);

            // GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + Calc() * Time.deltaTime;
            GetComponent<Rigidbody>().AddForce(acceleration * Time.deltaTime, ForceMode.Acceleration);
           transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);
        
     

    }

    private Vector3 Calc()
    {
        
        Vector3 flockCenter = _boid_controller.Target.transform.localPosition;
        Vector3 flockVelocity = _boid_controller.flockVelocity;


        flockCenter = flockCenter - transform.localPosition;
        flockVelocity = flockVelocity - GetComponent<Rigidbody>().velocity;
        //follow = follow - transform.localPosition;

        return ((flockCenter - transform.localPosition) + flockVelocity); //follow * 2)// + randomize * _boid_controller.randomness);
    }

    Vector3 Cohesion()
    {
        Vector3 sum_vector = new Vector3();
        int count = 0;


        for (int i = 0; i < _boids.Count; i++)
        {
            float dist = Vector3.Distance(GetComponent<Rigidbody>().position, _boids[i].position);

            if (dist < _boid_controller._cohesion_radius && dist > 0) // dist > 0 prevents including this boid
            {
                sum_vector += _boids[i].position;
                count++;
            }
        }


        if (count > 0)
        {
            sum_vector /= count;
            return sum_vector - GetComponent<Rigidbody>().position;
        }

        return sum_vector;
    }


    Vector3 Alignment()
    {
        Vector3 sum_vector = new Vector3();
        int count = 0;


        for (int i = 0; i < _boids.Count; i++)
        {
            float dist = Vector3.Distance(GetComponent<Rigidbody>().position, _boids[i].position);

            if (dist < _boid_controller._cohesion_radius && dist > 0)
            {
                sum_vector += _boids[i].velocity;
                count++;

            }
        }


        if (count > 0)
        {
            sum_vector /= count;
            sum_vector = Vector3.ClampMagnitude(sum_vector, 1);
        }

        return sum_vector;
    }


    Vector3 Separation()
    {
        Vector3 sum_vector = new Vector3();
        int count = 0;


        for (int i = 0; i < _boids.Count; i++)
        {
            float dist = Vector3.Distance(GetComponent<Rigidbody>().position, _boids[i].position);

            if (dist < _boid_controller._separation_radius && dist > 0)
            {
                sum_vector += (GetComponent<Rigidbody>().position - _boids[i].position).normalized / dist;
                count++;
            }
        }


        if (count > 0)
        {
            sum_vector /= count;
        }
        return sum_vector;
    }

    ////Draw radius for debugging
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, _boid_controller._cohesion_radius);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _boid_controller._separation_radius);
    //}
}
