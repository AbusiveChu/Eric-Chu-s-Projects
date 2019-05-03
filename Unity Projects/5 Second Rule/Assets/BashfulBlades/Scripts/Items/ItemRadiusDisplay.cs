using UnityEngine;
using System.Collections;

public class ItemRadiusDisplay : MonoBehaviour {

    public GameObject Radius;
    public GameObject Value;
    // Use this for initialization
    void Start()
    {

        Radius.SetActive(false);
        Value.SetActive(false);
    }

        // Update is called once per frame
        void Update () {
	if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Radius.SetActive(true);
            Value.SetActive(true);
        }
    else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            Radius.SetActive(false);
            Value.SetActive(false);
        }
	}
}
