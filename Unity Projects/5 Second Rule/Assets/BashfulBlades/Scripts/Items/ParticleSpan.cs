using UnityEngine;
using System.Collections;

public class ParticleSpan : MonoBehaviour {
    public float lifespan;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, lifespan);
	}
}
