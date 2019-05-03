using UnityEngine;
using System.Collections;

public class MultiplayerToggle : MonoBehaviour {


    public GameObject[] Things = new GameObject[4];
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(MainMenuControls2.Multiplayer == true)
        {
            Things[0].SetActive(true);
            Things[1].SetActive(true);
            Things[2].SetActive(true);
            //Things[3].SetActive(true);
        }
        else if (MainMenuControls2.Multiplayer == false)
        {
            Things[0].SetActive(false);
           Things[1].SetActive(false);
          //  Things[2].SetActive(false);
          //  Things[3].SetActive(false);
        }
	}
}
