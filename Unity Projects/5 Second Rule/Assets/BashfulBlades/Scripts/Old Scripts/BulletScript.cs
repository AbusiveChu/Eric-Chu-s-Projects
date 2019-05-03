using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	//Gun
	private float nextFire;
	public float fireRate;
	public Rigidbody shot;	
	public Transform shotSpawn; 
	//Misc
	//public AudioSource audio;



    public Camera cam;
    private Ray ray;
    private RaycastHit hit;

    public static Vector3 hitDir;
    public static bool hitConfirm = false;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        //Gun check
        //       if(Player.Stunned == true)
        //       {

        //       }
        //	else if (Input.GetMouseButtonDown(0) && Time.time > nextFire && Player.Stunned == false)
        //	{

        //           if (Cursor.lockState != CursorLockMode.Locked)
        //           {
        //               Cursor.lockState = CursorLockMode.Locked;
        //           }
        //		// Vector3 ShotM;
        //		// ShotM = (gameObject.transform.rotation.)
        //		nextFire = Time.time + fireRate;

        //           ////Eric's Shit ----------------
        //           ray = cam.ScreenPointToRay(Input.mousePosition);
        //           if (Physics.Raycast(ray, out hit))
        //           {
        //               hitDir = new Vector3((hit.point.x - shotSpawn.position.x)/(hit.distance),
        //                                   (hit.point.y - shotSpawn.position.y) / (hit.distance),
        //                                   (hit.point.z - shotSpawn.position.z)/(hit.distance));
        //               hitConfirm = true;

        //           }
        //           //// ---------------------------
        //		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //		audio.Play();
        //	}    
        //}
    }
}
