using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameOver : MonoBehaviour {
    public GameObject Team1UI;
    public GameObject Team2UI;
    public Text Menu;
    public string Menutext1;
    public string Menutext2;
    public Text[] Player1UI = new Text[4];
    public Text[] Player2UI = new Text[4];
    public Text[] Player3UI = new Text[4];
    public Text[] Player4UI = new Text[4];
    public static string[] sPlayer1UI = new string[4];
    public static string[] sPlayer2UI = new string[4];
    public static int[] sPlayer3UI = new int[4];
    public static int[] sPlayer4UI = new int[4];

    public Image[] Select;
    public int LeftY;
    public static float delayA = 0.5f;
    public static float NextA;
    public int Selectionv;
    bool once = false;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.None;
	}
	
	// Update is called once per frame
	void Update () {
        MainUIControls();
        if (MainMenuControls2.Multiplayer == true)
        {
            Menu.text = Menutext1;
            Player1UI[0].text = "Player 1 Name";
            Player1UI[1].text = "Character";
            Player1UI[2].text = "Total Kills";
            Player1UI[3].text = "Best for Something";

            Player2UI[0].text = "Player 2 Name";
            Player2UI[1].text = "Character";
            Player2UI[2].text = "Total Kills";
            Player2UI[3].text = "Best for Something";

            Player3UI[0].text = "Player 3 Name";
            Player3UI[1].text = "Character";
            Player3UI[2].text = "Total Kills";
            Player3UI[3].text = "Best for Something";

            Player4UI[0].text = "Player 4 Name";
            Player4UI[1].text = "Character";
            Player4UI[2].text = "Total Kills";
            Player4UI[3].text = "Best for Something";
        }
        else if(MainMenuControls2.Multiplayer == false)
        {
            Menu.text = Menutext2;
            Team1UI.SetActive(false);
            Team2UI.SetActive(false);
        }

	}
    void MainUIControls()
    {
        LeftY = ControllerManager.GetLeftJoyStickY();

        if (LeftY > ControllerMaxMIN.Left_ControllerMaxX)
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
        if (LeftY < ControllerMaxMIN.Left_ControllerBaseY && LeftY > -ControllerMaxMIN.Left_ControllerBaseY)
        {
            once = true;
        }
        if (Selectionv == 0)
        {
            Select[0].color = Color.white;
            Select[1].color = Color.grey;
           
        }

        if (Selectionv == 1)
        {
            Select[1].color = Color.white;
            Select[0].color = Color.grey;
           
        }
        if (Selectionv > 1)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 1;
        }
        if (ControllerManager.GetButtonDown(0) == true && Time.time > NextA)//A
        {
            if (Selectionv == 0)
            {
                Application.LoadLevel(0);
            }
            if (Selectionv == 1)
            {
                Application.Quit();
            }
            NextA = Time.time + delayA;
        }
    }
}
