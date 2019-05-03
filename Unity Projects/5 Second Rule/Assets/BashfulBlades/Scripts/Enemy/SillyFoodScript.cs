using UnityEngine;
using System.Collections;

public class SillyFoodScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy")
        {
            Destroy(collisionInfo.gameObject);
        }

    }
}
