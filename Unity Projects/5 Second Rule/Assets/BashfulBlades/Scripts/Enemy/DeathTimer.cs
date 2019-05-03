using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour {
    public int lifespan;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, lifespan);
	}
}
