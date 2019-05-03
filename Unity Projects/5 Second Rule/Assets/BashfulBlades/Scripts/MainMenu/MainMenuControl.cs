using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour {


  
    private int LeftX;

    public GameObject[] Before;
    public GameObject[] After;
    public int Selection;

    public int LevelLoad;
    bool once = true;
    // 0 = Load // 1 = Start Game // 2 = Options // 3 = Exit
    // Use this for initialization
  
    void Start () 
    {
        ControllerManager.Initialize(1);
        Cursor.lockState = CursorLockMode.None;
        //InputManager.SetHookTarget(InputManager.GetActiveWindow());       
       
    }

    // Update is called once per frame
    void Update()
    {
       
            LeftX = ControllerManager.GetLeftJoyStickX();

        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            if (once == true)
            {
                Selection += 1;
            }
            once = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            if (once == true)
            {

                Selection -= 1;
            }
            once = false;
        }
        if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        {
            once = true;
        }
        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA)//A
        {
            if (LevelLoad == 6)
            {
                Application.Quit();
            }
            SceneManager.LoadScene(LevelLoad);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }

        if (Selection == 0)
            {
                LevelLoad = 3;                
                Before[Selection].SetActive(false);
                After[Selection].SetActive(true);
                After[1].SetActive(false);
                After[2].SetActive(false);
                After[3].SetActive(false);

                Before[1].SetActive(true);
                Before[2].SetActive(true);
                Before[3].SetActive(true);
            }
            else if (Selection == 1)
            {
                LevelLoad = 3;
                Before[Selection].SetActive(false);
                After[Selection].SetActive(true);
                After[0].SetActive(false);
                After[2].SetActive(false);
                After[3].SetActive(false);
                Before[0].SetActive(true);

                Before[2].SetActive(true);
                Before[3].SetActive(true);
            }
            else if (Selection == 2)
            {
                LevelLoad = 4;
                Before[Selection].SetActive(false);
                After[Selection].SetActive(true);
                After[1].SetActive(false);
                After[0].SetActive(false);
                After[3].SetActive(false);
                Before[0].SetActive(true);
                Before[1].SetActive(true);

                Before[3].SetActive(true);
            }
            else if (Selection == 3)
            {
                LevelLoad = 6;
                Before[Selection].SetActive(false);
                After[Selection].SetActive(true);
                After[1].SetActive(false);
                After[2].SetActive(false);
                After[0].SetActive(false);
                Before[0].SetActive(true);
                Before[1].SetActive(true);
                Before[2].SetActive(true);

            }

            if (Selection > 3)
            {
                Selection = 0;
            }
            else if (Selection < 0)
            {
                Selection = 3;
            }
        
    }   
}
