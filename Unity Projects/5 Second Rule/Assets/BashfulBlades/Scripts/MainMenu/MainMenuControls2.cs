using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControls2 : MonoBehaviour {

    public GameObject[] Arrows;
    public int LeftY;
    public int Selectionv;
    bool once = false;
    public static bool Multiplayer;
	// Use this for initialization
	void Start () {
        ControllerManager.Initialize(1);
   //     InputManager.SetHookTarget(InputManager.GetActiveWindow());
        Cursor.lockState = CursorLockMode.None;
	}
	
	// Update is called once per frame
	void Update () {

        //Controls for Controller
        LeftY = ControllerManager.GetLeftJoyStickY();
        if (LeftY > ControllerMaxMIN.Left_ControllerMaxY)
        {
            if (once == true)
            {
                Selectionv -= 1;
            }
            once = false;
        }
        else if (LeftY < ControllerMaxMIN.Left_ControllerMinY)
        {
            if (once == true)
            {

                Selectionv += 1;
            }
            once = false;
        }
        if (Input.GetKeyDown(KeyCode.W))// && Time.time > SelectVert.NextA)
        {
            Selectionv -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S))// && Time.time > SelectVert.NextA)
        {
            Selectionv += 1;
        }
        if (LeftY < ControllerMaxMIN.Left_ControllerBaseX && LeftY > -ControllerMaxMIN.Left_ControllerBaseX)
        {
            once = true;
        }
        //Selection and what to display   
        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].SetActive(false);
        }
        if (Selectionv == 0)
        {           
            Arrows[0].SetActive(true);
        }
        if (Selectionv == 1)
        {           
            Arrows[1].SetActive(true);
        }
        if (Selectionv == 2)
        {            
            Arrows[2].SetActive(true);
        }
          
        if (Selectionv > 2)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 2;
        }

        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.Return))//A
        {
            if (Selectionv == 0)//Character Select
            {
                Multiplayer = false;
                SetLevel.SinglePlayer = true;
                SceneManager.LoadScene(3);               
            }
            if (Selectionv == 1)//Multiplayer
            {
                Multiplayer = true;
                SetLevel.SinglePlayer = false;
                SceneManager.LoadScene(8);
            }
            if (Selectionv == 2)//Option
            {
                Application.Quit();
            }
                
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }   
	}
}
