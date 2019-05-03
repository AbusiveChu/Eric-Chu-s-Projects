using UnityEngine;
using System.Collections;

public class PlaceRef : MonoBehaviour 
{

	void Update () 
    {
        if ((gameObject.tag == "Mousetrap" && ItemScript.showItem[0]) ||
            (gameObject.tag == "Honeycomb" && ItemScript.showItem[1]) ||
            (gameObject.tag == "GarlicClove" && ItemScript.showItem[2]))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
	}
}
 