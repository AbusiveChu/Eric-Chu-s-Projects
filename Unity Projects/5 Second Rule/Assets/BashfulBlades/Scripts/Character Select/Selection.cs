using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Selection : MonoBehaviour
{

    public int LeftX;
    public int Selectionv;
    public GameObject ROTOBJECT;
    public Text[] CharacterText;
    public Vector3 rotation;
    private bool once = false;

    // Use this for initialization
    void Start()
    {
        ControllerManager.Initialize(1);
      //  InputManager.SetHookTarget(InputManager.GetActiveWindow());
        Player.Money = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        UpdateCharacterSelect();
        ControllerManager.GPConnected = ControllerManager.IsConnected();
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            Selectionv += 1;
            ROTOBJECT.transform.Rotate(0, 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Selectionv -= 1;
            ROTOBJECT.transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CharacterSelect();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(0);
        }

        LeftX = ControllerManager.GetLeftJoyStickX();

        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            if (once == true)
            {
                Selectionv += 1;
                ROTOBJECT.transform.Rotate(0, 90, 0);
            }
            once = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            if (once == true)
            {
                Selectionv -= 1;
                ROTOBJECT.transform.Rotate(0, -90, 0);
            }
            once = false;
        }
        if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
            {
                once = true;
            }
        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA)//A
        {
            CharacterSelect();
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }

        if (ControllerManager.GetButtonDown(1) == true && Time.time > SelectVert.NextA)//B
        {
            SceneManager.LoadScene(0);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }

        if (Selectionv > 3)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 3;
        }
    }
    void UpdateCharacterSelect()
    {
        //BROC LEE      
        if (Selectionv == 0)
        {
            //Name
            CharacterText[0].text = "Broc Lee";
            //Info
            CharacterText[1].text = "FOR HONOR!";
            //Primary Gun
            CharacterText[2].text = "Single Shot";
            //2nd abiltie 
            CharacterText[3].text = "Arc Shot";
            //Ulti
            CharacterText[4].text = "Shield Burst";
            //Starting Items
            CharacterText[6].text = "3 Blackpeperbomb";
            //Starting Items
            CharacterText[7].text = "3 Honeycombs";
            //Starting Items
            CharacterText[8].text = "3 Mousetrap";
            //Starting Items
            CharacterText[9].text = "5 Garlic Cloves";
        }
        // CARROT 
        if (Selectionv == 1)
        {
            //Name
            CharacterText[0].text = "Carrot Girl";
            //Info
            CharacterText[1].text = "I CAN SEE ALL HAHAHA";
            //Primary Gun
            CharacterText[2].text = "Triple Shot";
            //2nd abiltie 
            CharacterText[3].text = "Carrot Mine";
            //Ulti
            CharacterText[4].text = "Giant Flaming Shot";
            //Starting Items
            CharacterText[6].text = "3 Blackpeperbomb";
            //Starting Items
            CharacterText[7].text = "3 Honeycombs";
            //Starting Items
            CharacterText[8].text = "5 Mousetrap";
            //Starting Items
            CharacterText[9].text = "3 Garlic Cloves";
        }
        // HAMBO
        if (Selectionv == 2)
        {
            //Name
            CharacterText[0].text = "Hambo";
            //Info
            CharacterText[1].text = "I ONLY NEED MY TWO LMGS! TO PROTECT MY FOOD";
            //Primary Gun
            CharacterText[2].text = "Double Shot";
            //2nd abiltie 
            CharacterText[3].text = "Firerate ++";
            //Ulti
            CharacterText[4].text = "Explosive Shots";
            //Starting Items
            CharacterText[6].text = "5 Blackpeperbomb";
            //Starting Items
            CharacterText[7].text = "3 Honeycombs";
            //Starting Items
            CharacterText[8].text = "3 Mousetrap";
            //Starting Items
            CharacterText[9].text = "3 Garlic Cloves";
        }
        //CHICKEN
        if (Selectionv == 3)
        {
            //Name
            CharacterText[0].text = "Chicken";
            //Info
            CharacterText[1].text = "I WAS A DRAGON IN MY PAST LIFE!";
            //Primary Gun
            CharacterText[2].text = "Wind Slice";
            //2nd abiltie 
            CharacterText[3].text = "Slow Shot";
            //Ulti
            CharacterText[4].text = "Giant Explosion Shot";
            //Starting Items
            CharacterText[6].text = "3 Blackpeperbomb";
            //Starting Items
            CharacterText[7].text = "5 Honeycombs";
            //Starting Items
            CharacterText[8].text = "3 Mousetrap";
            //Starting Items
            CharacterText[9].text = "3 Garlic Cloves";
        }
    }
    void CharacterSelect()
    {
        SetLevel.Broco = false;
        SetLevel.Carrot = false;
        SetLevel.Hambo = false;
        SetLevel.Chicken = false;
        if (Selectionv == 0)
        {
            SetLevel.CharSelect = 1;
            SetLevel.Broco = true;
        }
        if (Selectionv == 1)
        {
            SetLevel.CharSelect = 3;
            SetLevel.Carrot = true;
        }
        if (Selectionv == 2)
        {
            SetLevel.CharSelect = 2;
            SetLevel.Hambo = true;
        }
        if (Selectionv == 3)
        {
            SetLevel.CharSelect = 4;
            SetLevel.Chicken = true;
        }
        if (SetLevel.SinglePlayer == true)
        {
            SceneManager.LoadScene(6);
        }
        else if(SetLevel.SinglePlayer == false)
        {
            SceneManager.LoadScene(9);
        }
    }
}

