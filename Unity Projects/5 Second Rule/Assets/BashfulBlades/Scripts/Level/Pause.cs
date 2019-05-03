using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    bool once;
    public static bool paused = false;

    void Update()
    {
        if (MainMenuControls2.Multiplayer == false)
        {
            if (ControllerManager.GetButtonDown(11) == true || Input.GetKeyDown(KeyCode.P))
            {
                if (once == true)
                {
                    paused = togglePause();
                }
                once = false;
            }
            else if (ControllerManager.GetButtonDown(11) == false)
            {
                once = true;
            }
            if (paused == true)
            {
                if (ControllerManager.GetButtonDown(10) == true || Input.GetKeyDown(KeyCode.M))
                {
                    paused = togglePause();
                    //  SoundScript.StopSound(0);
                    Application.LoadLevel(0);

                }

            }
        }
    }

    void OnGUI()
    {
        if (paused)
        {
          
            
            GUI.Button(new Rect(.5f * Screen.width, .5f * Screen.height + 20, .5f * Screen.width , .5f * Screen.height / 4), "Press Start / P to resume ");
            GUI.Button(new Rect(.5f * Screen.width, .5f * Screen.height + 40, .5f * Screen.width , .5f * Screen.height / 4), "Press Back / M to return to Main Menu");
            GUI.Button(new Rect(.5f * Screen.width, .5f * Screen.height, .5f * Screen.width , .5f * Screen.height /4), "Game is paused!");
            //GUI.Button(new Rect(.5f * Screen.width, .5f * Screen.height + 20, .5f * Screen.width, .5f * Screen.height), "");
                  
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}