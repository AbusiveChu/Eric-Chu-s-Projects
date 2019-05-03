using UnityEngine;
using System.Collections;

public class LookAtMe : MonoBehaviour {

    private Transform lookatthis;
	// Use this for initialization
	void Start () {
        lookatthis = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(lookatthis.transform);
        //transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y - 180.0f, transform.localRotation.z, transform.localRotation.w);
	}
}
