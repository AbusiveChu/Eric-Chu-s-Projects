using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Events;
public class Actions : MonoBehaviour
{
    public SetPlayerIcons setplay;
    public UnityEvent SabEvent;
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject StartingPosPlayerOne;
    public GameObject StartingPosPlayerTwo;
    public GameObject PathArrows;
    private bool once;
    public static int Ready = 0;
    bool invokeonce = true;
    // Use this for initialization
    void Start()
    {
        PlayerOne.transform.position = StartingPosPlayerOne.transform.position;
        PlayerTwo.transform.position = StartingPosPlayerTwo.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemySpawn.CheckPoint == false)
        {
            invokeonce = true;
        }
        else if (EnemySpawn.CheckPoint == true)
        {
            if (invokeonce == true)
            {
                SabEvent.Invoke();
                invokeonce = false;
            }
           
            if (SetPlayerIcons.PlayerReady[1] == true && SetPlayerIcons.PlayerReady[2] == true && SetPlayerIcons.PlayerReady[3] == true && SetPlayerIcons.PlayerReady[4] == true)
            {
                PlayerOne.transform.position = StartingPosPlayerOne.transform.position;
                //PlayerTwo.transform.position = StartingPosPlayerTwo.transform.position;
                //NetworkEnemySpawn.GetEnemyPackage();

                EnemySpawn.CleanUpTime = false;
                EnemySpawn.CheckPoint = false;
                EnemySpawn.UpdateText = true;
                EnemySpawn.WaveNumber += 1;
                EnemySpawn.SpawnCD = Time.time + 3;
                EnemySpawn.Spawncounter = 0;
                PathArrows.SetActive(false);
                Ready = 0;
                setplay.PlayerReadyIcon[1].SetActive(false);
                setplay.PlayerReadyIcon[2].SetActive(false);
                setplay.PlayerReadyIcon[3].SetActive(false);
                setplay.PlayerReadyIcon[4].SetActive(false);
                SetPlayerIcons.PlayerReady[1] = false;
                SetPlayerIcons.PlayerReady[2] = false;
                SetPlayerIcons.PlayerReady[3] = false;
                SetPlayerIcons.PlayerReady[4] = false;

            }
        }
        if (EnemyShop.EnemyShopToggle == false)
        {
            if (ControllerManager.GetButtonDown(2) == true || Input.GetKeyDown(KeyCode.X))
            {

                if (EnemySpawn.CheckPoint == true)
                {
                    if (SetLevel.SinglePlayer == true)
                    {
                        PlayerOne.transform.position = StartingPosPlayerOne.transform.position;
                        //PlayerTwo.transform.position = StartingPosPlayerTwo.transform.position;
                        //NetworkEnemySpawn.GetEnemyPackage();

                        EnemySpawn.CleanUpTime = false;
                        EnemySpawn.CheckPoint = false;
                        EnemySpawn.UpdateText = true;
                        EnemySpawn.WaveNumber += 1;
                        EnemySpawn.SpawnCD = Time.time + 3;
                        EnemySpawn.Spawncounter = 0;
                        PathArrows.SetActive(false);
                    }
                    else if (SetLevel.SinglePlayer == false)
                    {
                        Ready = 1;

                    }
                }
                else
                {
                    //EnemySpawn.WaveNumber += 1;
                    PlayerOne.transform.position = StartingPosPlayerOne.transform.position;
                }
            }
        }
        if (ControllerManager.GetButtonDown(3) == true || Input.GetKeyDown(KeyCode.U))
        {
            if (once == false)
            {
                EnemyShop.EnemyShopToggle = !EnemyShop.EnemyShopToggle;
                once = true;
            }
        }
        else if (ControllerManager.GetButtonDown(3) == false || Input.GetKeyUp(KeyCode.U))
        {
            if (once == true)
            {
                once = false;
            }
        }

    }
}
