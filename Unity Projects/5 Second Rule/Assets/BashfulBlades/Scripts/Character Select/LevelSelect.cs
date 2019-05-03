using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour 
{
    //Init
    public Text Info;
    public GameObject[] Arrows;
    public int RightX;
    public int Selectionv;
    bool once = false;

    // Use this for initialization
    void Start () {
        ControllerManager.Initialize(1);
       // InputManager.SetHookTarget(InputManager.GetActiveWindow());
        Cursor.lockState = CursorLockMode.None;        
    }
    // Update is called once per frame
    void Update()
    {
        //Controls for Controller
        RightX = ControllerManager.GetLeftJoyStickX();
        if (RightX > 30000)
        {
            if (once == true)
            {
                Selectionv -= 1;
            }
            once = false;
        }
        else if (RightX < -30000)
        {
            if (once == true)
            {

                Selectionv += 1;
            }
            once = false;
        }
                if (Input.GetKeyDown(KeyCode.A))// && Time.time > SelectVert.NextA)
        {
            Selectionv += 1;
        }
        if (Input.GetKeyDown(KeyCode.D))// && Time.time > SelectVert.NextA)
        {
            Selectionv -= 1;
        }
        if (RightX == 0)
        {
            once = true;
        }
        //Selection and what to display   
        Arrows[0].SetActive(false);
        Arrows[1].SetActive(false);
        Arrows[2].SetActive(false);
        Arrows[3].SetActive(false);
        if (Selectionv == 0)
        {
            Info.text = "Level 1 - The Falling";
            Arrows[0].SetActive(true);         
        }
        if (Selectionv == 1)
        {
            Info.text = "Level 2 - The Falling AGAIN";       
            Arrows[1].SetActive(true);
           
        }
        if (Selectionv == 2)
        {
            Info.text = "Level 3 - The Chef keeps dropping me wtf";     
            Arrows[2].SetActive(true);
           
        }
        if (Selectionv == 3)
        {
            Info.text = "Level 4 - This is awful I am so not clean anymore";           
            Arrows[3].SetActive(true);
        }
        if (Selectionv == 4)
        {
            Info.text = "Level 5 - How did you even get here";
        }
        if (Selectionv > 3)
        {
            Selectionv = 0;
        }
        if (Selectionv < 0)
        {
            Selectionv = 3;
        }

        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.Return))//A
        {
            if (Selectionv == 0)//Level 1
            {
                SceneManager.LoadScene(5);
                EnemySpawn.LevelRound = 10;
            }

            if (Selectionv == 1)//Level 2
            {
                EnemySpawn.LevelRound = 20;
                SceneManager.LoadScene(5);
            }
            if (Selectionv == 2)//Level 3
            {
                EnemySpawn.LevelRound = 30;
                SceneManager.LoadScene(5);
            }
            if (Selectionv == 3)//Level 4
            {
                EnemySpawn.LevelRound = 40;
                SceneManager.LoadScene(5);
            }
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }
        if (ControllerManager.GetButtonDown(3) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.Y))//Y
        {
            SceneManager.LoadScene(7);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }      
        if (Input.GetKeyDown(KeyCode.Backspace)  || ControllerManager.GetButtonDown(1) == true && Time.time > SelectVert.NextA)//B
        {
            SceneManager.LoadScene(3);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }
        
    }
}
