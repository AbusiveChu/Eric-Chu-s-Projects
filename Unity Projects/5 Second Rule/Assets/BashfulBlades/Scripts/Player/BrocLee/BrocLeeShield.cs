using UnityEngine;
using System.Collections;

public class BrocLeeShield : MonoBehaviour {

    public Transform Shield;
    public bool once;
	// Use this for initialization
	void Start () {
        once = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (once == true)
        {
            if (Shield.transform.localScale.x < 76)
            {
                Shield.transform.localScale += new Vector3(2, 2, 2);
            }
        }
        if(Shield.transform.localScale.x > 75)
        {
            once = false;
            
        }
        if (once == false)
        {
            Shield.transform.localScale -= new Vector3(2, 2, 2);
            if (Shield.transform.localScale.x < 1)
            {
                Destroy(gameObject);
            }
        }
    }
   
}
