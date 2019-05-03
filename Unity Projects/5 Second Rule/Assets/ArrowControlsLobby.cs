using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ArrowControlsLobby : MonoBehaviour {
    public HostStartGame hostdastart;
    public bool startgameready = false;
    public GameObject startgamebutton;
    public LobbyPlacement[] LobbyPlacementScript = new LobbyPlacement[4];
    public GameObject[] CharacterSelectArrows = new GameObject[4];
    public GameObject[] PlayerSlotArrows = new GameObject[4];
    public int LeftX,LeftY;
    public static float delayA = 0.5f;
    public static float NextA;
    public int Selectionv;
    bool once = true,twice = true;
    public int vert, hori;
	// Use this for initialization
	void Start () {
        ControllerManager.Initialize(1);
        Cursor.lockState = CursorLockMode.None;
	}
	
	// Update is called once per frame
	void Update () {
        //X
        LeftX = ControllerManager.GetLeftJoyStickX();

        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            if (once == true)
            {
                Selectionv += 1;
                if(SetLevel.CharSelect > 0)
                {
                    hori += 10;
                }
            }
            once = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            if (once == true)
            {
                Selectionv -= 1;
                if (SetLevel.CharSelect > 0)
                {
                    hori -= 10;
                }
            }

            once = false;
        }
        if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        {
            once = true;
        }
        //Y
        LeftY = ControllerManager.GetLeftJoyStickY();

        if (LeftY > ControllerMaxMIN.Left_ControllerMaxY)
        {
            if (twice == true)
            {
              
                if (SetLevel.CharSelect > 0)
                {
                    vert += 1;
                }
            }
            twice = false;
        }
        else if (LeftY < ControllerMaxMIN.Left_ControllerMinY)
        {
            if (twice == true)
            {                
                if (SetLevel.CharSelect > 0)
                {
                    vert -= 1;
                }
            }

            twice = false;
        }
        if (LeftY < ControllerMaxMIN.Left_ControllerBaseY && LeftY > -ControllerMaxMIN.Left_ControllerBaseY)
        {
            twice = true;
        }
        //OTHER
        if (Selectionv > 3)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 3;
        }
        if(vert > 1)
        {
            vert = 0;
        }
        if(vert < 0)
        {
            vert = 1;
        }
        if(hori > 10)
        {
            hori = 0;
        }
        if(hori < 0)
        {
            hori = 10;
        }
        if (SetLevel.CharSelect == 0)
        {
            PlayerSlotArrows[0].SetActive(false);
            PlayerSlotArrows[1].SetActive(false);
            PlayerSlotArrows[2].SetActive(false);
            PlayerSlotArrows[3].SetActive(false);
            CharacterSelectArrows[0].SetActive(false);
            CharacterSelectArrows[1].SetActive(false);
            CharacterSelectArrows[2].SetActive(false);
            CharacterSelectArrows[3].SetActive(false);
            CharacterSelectArrows[Selectionv].SetActive(true);
        }
        else if(SetLevel.CharSelect > 0)
        {
            CharacterSelectArrows[0].SetActive(false);
            CharacterSelectArrows[1].SetActive(false);
            CharacterSelectArrows[2].SetActive(false);
            CharacterSelectArrows[3].SetActive(false);
            PlayerSlotArrows[0].SetActive(false);
            PlayerSlotArrows[1].SetActive(false);
            PlayerSlotArrows[2].SetActive(false);
            PlayerSlotArrows[3].SetActive(false);
            if(vert == 0 && hori == 0)
            {
                PlayerSlotArrows[0].SetActive(true);
            }
            else if (vert == 1 && hori == 0)
            {
                PlayerSlotArrows[1].SetActive(true);
            }
            else if (vert == 0 && hori == 10)
            {
                PlayerSlotArrows[3].SetActive(true);
            }
            else if (vert == 1 && hori == 10)
            {
                PlayerSlotArrows[2].SetActive(true);
            }
        }
        if(ControllerManager.GetButtonDown(0) == true&& Time.time > SelectVert.NextA)
        {
            SelectVert.NextA = Time.time + SelectVert.delayA;
        
            if (SetLevel.CharSelect == 0)
            {
                SetLevel.CharSelect = Selectionv + 1;
            }
            else if(SetLevel.CharSelect > 0)
            {
                if (vert == 0 && hori == 0)
                {
                    LobbyPlacementScript[0].MoveToSlot();
                }
                else if (vert == 1 && hori == 0)
                {
                    LobbyPlacementScript[1].MoveToSlot();
                }
                else if (vert == 0 && hori == 10)
                {
                    LobbyPlacementScript[2].MoveToSlot();
                }
                else if (vert == 1 && hori == 10)
                {
                    LobbyPlacementScript[3].MoveToSlot();
                }
                if (NetworkToggle.HostGameBool == true && SetLevel.CharSelect > 0)
                {
                    startgameready = true;
                    startgamebutton.SetActive(true);
                }
            }
        }
        if (ControllerManager.GetButtonDown(11) == true && Time.time > SelectVert.NextA && SetLevel.CharSelect > 0 && startgameready == true)
        {
            SelectVert.NextA = Time.time + SelectVert.delayA;
            hostdastart.StartGame();
        }
	}
}
