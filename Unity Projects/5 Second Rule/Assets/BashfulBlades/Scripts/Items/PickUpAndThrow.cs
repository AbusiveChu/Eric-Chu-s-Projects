using UnityEngine;
using System.Collections;

public class PickUpAndThrow : MonoBehaviour {

    //Variables
    public Vector3 objectPos;
    public Quaternion objectRot;
    public GameObject pickObj;
   
    public bool canPickUp = true;
    public bool guiPickUp = false;
    public bool picking = false;
   
    public GameObject ReferenceObj;

    Vector3 RayCheckPos;

	void Start () 
    {
        RayCheckPos = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
        ReferenceObj = GameObject.FindWithTag("pickupref");

        //To prevent a "nothing assigned" error
        pickObj = ReferenceObj;
	}
	
	void Update () 
    {
        RaycastHit hit;
        Ray rayCheck = Camera.main.ScreenPointToRay(RayCheckPos);

        if (Physics.Raycast(rayCheck, out hit, 100.0f))
        {
            if (hit.collider.gameObject.tag == "pickup")
            {
                guiPickUp = true;
            }

            else if (hit.collider.gameObject.tag != "pickup")
            {
                guiPickUp = false;
            }
            else
            {
                guiPickUp = false;
            }
        }
        objectPos = transform.position;
        objectRot = transform.rotation;

        if ((Input.GetKeyDown(KeyCode.F) || ControllerManager.GetButtonDown(2)) && canPickUp)
        {
                          

                Ray ray = Camera.main.ScreenPointToRay(RayCheckPos);
                RaycastHit Hit;

                if (Physics.Raycast(ray, out Hit, 100.0f) && Hit.collider.gameObject.tag == "pickup")
                {
                    pickObj = hit.collider.gameObject;

                    Hit.rigidbody.useGravity = false;

                    Hit.rigidbody.isKinematic = true;

                    Hit.collider.isTrigger = true;

                    Hit.transform.parent = gameObject.transform;

                    Hit.transform.position = objectPos;

                    Hit.transform.rotation = objectRot;
                }
                
                       

        }
        if (!Input.GetKeyDown(KeyCode.F) && picking)
        {
            picking = false;

            canPickUp = false;
        }
        if (Input.GetKeyDown(KeyCode.G) && !canPickUp || ControllerManager.GetButtonDown(2) && canPickUp)
        {
            canPickUp = true;

            pickObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            pickObj.GetComponent<Rigidbody>().useGravity = true;
            pickObj.GetComponent<Rigidbody>().isKinematic = false;

            pickObj.transform.parent = null;

            pickObj.GetComponent<Collider>().isTrigger = false;

            pickObj.GetComponent<Rigidbody>().AddForce(transform.forward * 5000);

            pickObj = ReferenceObj;
        }
    }

    void OnGUI()
    {
        //GUI.Label(new Rect((Screen.width / 2.0f), Screen.height / 2.1, Screen.width / 2.0f, Screen.height /2.0), "X");
    }
}
