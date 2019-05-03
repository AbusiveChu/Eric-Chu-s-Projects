using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NetworkToggle : MonoBehaviour {
    
    public static bool HostGameBool;
    public GameObject HostGameObject;
    public GameObject JoinGameObject;
    public GameObject NetworkToggleScreen;
    public Text JoinGameText;
    public Text HostGameText;
    public Image b_JoinGameText;
    public Image b_HostGameText;
    public int LeftX;
    public bool once;
    public int Selectionv;
    public static bool joingameisago = false;

	// Use this for initialization
	void Start () {
        JoinGameObject.SetActive(false);
        HostGameObject.SetActive(false);
        SetLevel.SinglePlayer = false;
        ControllerManager.Initialize(1);
        
        Cursor.lockState = CursorLockMode.None;
    }
	
	// Update is called once per frame
	void Update () {
        //Controls for Controller
        LeftX = ControllerManager.GetLeftJoyStickX();
        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            if (once == true)
            {
                Selectionv += 1;
            }
            once = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            if (once == true)
            {

                Selectionv -= 1;
            }
            once = false;
        }       
        if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        {
            once = true;
        }
        
        if(ControllerManager.IsConnected() == true)
        {
            HostGameText.text = " Host Game";
            JoinGameText.text = " Join Game";
        }
        if(Selectionv == 0)
        {
            b_HostGameText.color = Color.white;
            b_JoinGameText.color = Color.black;
        }
        else if(Selectionv == 1)
        {
            b_JoinGameText.color = Color.white;
            b_HostGameText.color = Color.black;
        }
        if(Selectionv > 1)
        {
            Selectionv = 0;
        }
        if(Selectionv < 0)
        {
            Selectionv = 1;
        }
        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA)
        {
            SelectVert.NextA = Time.time + SelectVert.delayA;
            if (Selectionv == 0)
            {
                HostGameBool = true;
                JoinGameObject.SetActive(false);
                HostGameObject.SetActive(true);
                NetworkToggleScreen.SetActive(false);
            }
            else if (Selectionv == 1)
            {
                HostGameBool = false;
                JoinGameObject.SetActive(true);
                HostGameObject.SetActive(false);
                NetworkToggleScreen.SetActive(false);
                joingameisago = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))//Host Game
        {
           HostGameBool = true;
            JoinGameObject.SetActive(false);
            HostGameObject.SetActive(true);
            NetworkToggleScreen.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))//
        {
            HostGameBool = false;
            JoinGameObject.SetActive(true);
            HostGameObject.SetActive(false);
            NetworkToggleScreen.SetActive(false);
        }
    }
}
