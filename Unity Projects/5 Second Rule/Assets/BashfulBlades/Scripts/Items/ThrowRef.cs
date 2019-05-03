using UnityEngine;
using System.Collections;

public class ThrowRef : MonoBehaviour 
{
    public Transform Camera;
    public Vector3 LocPos;
    float tempY = 0;
	void Update () 
    {
        if (ItemScript.bombBeingThrown)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        if (tempY != Camera.localPosition.y)
        {
            LocPos = transform.localPosition;
            LocPos.z = LocPos.z + 5*(tempY - Camera.localPosition.y);
            transform.localPosition = LocPos;
            tempY = Camera.localPosition.y;
        }
	}
}
