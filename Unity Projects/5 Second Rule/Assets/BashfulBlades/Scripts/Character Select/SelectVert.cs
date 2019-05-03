using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class SelectVert : MonoBehaviour {

    public Image[] Select;
    public int LeftY;
    public static float delayA = 0.5f;
    public static float NextA;
    public int Selectionv;
    bool once = false;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
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
            Select[2].color = Color.grey;
            Select[3].color = Color.grey;
        }
       
            if (Selectionv == 1)
            {
                Select[1].color = Color.white;
                Select[0].color = Color.grey;
                Select[2].color = Color.grey;
                Select[3].color = Color.grey;
            }
            if (Selectionv == 2)
            {
                Select[2].color = Color.white;
                Select[1].color = Color.grey;
                Select[0].color = Color.grey;
                Select[3].color = Color.grey;
            }
            if (Selectionv == 3)
            {
                Select[3].color = Color.white;
                Select[1].color = Color.grey;
                Select[2].color = Color.grey;
                Select[0].color = Color.grey;
            }
       
     
        if (Selectionv > 3)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 3;
        }
        if (ControllerManager.GetButtonDown(0) == true && Time.time > NextA)//A
        {
            if (Selectionv == 0)
            {
                Application.LoadLevel(0);
            }
            if (Selectionv == 1)
            {
                Application.LoadLevel(3);
            }
            if (Selectionv == 2)
            {
                Application.LoadLevel(6);
            }
            if (Selectionv == 3)
            {
                Application.Quit();
            }
            NextA = Time.time + delayA;
        }
      
    }
}
