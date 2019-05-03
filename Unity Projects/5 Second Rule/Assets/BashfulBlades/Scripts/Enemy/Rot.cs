using UnityEngine;
using System.Collections;

public class Rot : MonoBehaviour {
    public float speed;
    public Transform RedRot;

    void Start()
    {


    }
	
	// Update is called once per frame
	void Update () {


        // rotation.eulerAngles = new Vector3(0, 30, 0);
        RedRot.transform.Rotate(Time.time * speed, 0, 0);
    }
}
