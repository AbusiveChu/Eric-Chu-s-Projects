using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelectNew : MonoBehaviour
{
    public GameObject CameraRot;
    public Camera MainCamera;
    public int SetLevel;
    //Init
    public Text Info;
    public GameObject[] Arrows;
    public int LeftX;
    public int Selectionv;
    bool once = false;


    // Use this for initialization
    void Start()
    {
        ControllerManager.Initialize(1);
     //   InputManager.SetHookTarget(InputManager.GetActiveWindow());
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        MainCamera.transform.RotateAround(CameraRot.transform.position, new Vector3(0, 1, 0), 0.3f);

        ////Controls for Controller
        //LeftX = ControllerManager.GetLeftJoyStickX();
        //if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        //{
        //    if (once == true)
        //    {
        //        Selectionv += 1;
        //    }
        //    once = false;
        //}
        //else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        //{
        //    if (once == true)
        //    {

        //        Selectionv -= 1;
        //    }
        //    once = false;
        //}
        //if (Input.GetKeyDown(KeyCode.A))// && Time.time > SelectVert.NextA)
        //{
        //    Selectionv -= 1;
        //}
        //if (Input.GetKeyDown(KeyCode.D))// && Time.time > SelectVert.NextA)
        //{
        //    Selectionv += 1;
        //}
        //if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        //{
        //    once = true;
        //}
        //Selection and what to display   
        //for (int i = 0; i < Arrows.Length; i++)
        //{
        //    Arrows[i].SetActive(false);
        //}
        //if (Selectionv == 0)
        //{
        //    Info.text = "Very Easy";
        //    Arrows[0].SetActive(true);
        //}
     
       

        if (ControllerManager.GetButtonDown(0) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.Return))//A
        {
            //if (Selectionv == 0)//Level 1
            //{
            //    SceneManager.LoadScene(5);
            //    EnemySpawn.LevelRound = 10;
            //}

            //if (Selectionv == 1)//Level 2
            //{
            //    EnemySpawn.LevelRound = 20;
            //    SceneManager.LoadScene(5);
            //}
            //if (Selectionv == 2)//Level 3
            //{
            //    EnemySpawn.LevelRound = 30;
            //    SceneManager.LoadScene(5);
            //}
            //if (Selectionv == 3)//Level 4
            //{
            //    EnemySpawn.LevelRound = 40;
            //    SceneManager.LoadScene(5);
            //}
            //if (Selectionv == 4)//Level 5
            //{
                EnemySpawn.LevelRound = 50;
                SceneManager.LoadScene(5);
            //}
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }
        //if (ControllerManager.GetButtonDown(3) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.Y))//Y
        //{
        //    SceneManager.LoadScene(7);
        //    SelectVert.NextA = Time.time + SelectVert.delayA;
        //}
        //if (ControllerManager.GetButtonDown(4) == true && Time.time > SelectVert.NextA || Input.GetKeyDown(KeyCode.X))//Y
        //{
        //    SceneManager.LoadScene(8);
        //    SelectVert.NextA = Time.time + SelectVert.delayA;
        //}
        if (Input.GetKeyDown(KeyCode.Backspace) || ControllerManager.GetButtonDown(1) == true && Time.time > SelectVert.NextA)//B
        {
            SceneManager.LoadScene(3);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }

    }
}
