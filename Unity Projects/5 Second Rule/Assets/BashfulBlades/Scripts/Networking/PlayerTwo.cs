using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerTwo : MonoBehaviour
{
    //Player Two Info
    public Transform PlayerTwoPos;
    public Transform PlayerTwoShootSpawn;
    public Transform PlayerTwoItemSpawn;
    public Vector3 PlayerTwoHitDir;
    public static Vector3 PlayerTwoAccRecv;
    public static Vector3 PlayerTwoVelRecv;
    public static Vector3 PlayerTwoPosRecv;
    public static int PlayerTwoChar = 0;
    public GameObject[] PlayerTwoModel = new GameObject[4];
    private bool characterselectonce = true;
    //Player Two Items
    public GameObject GarlicClove;
    public GameObject Blackpepperbombg;
    public GameObject Mousetrap;
    public GameObject Honeycomb;
    //lerp stuff
    private float lerpTime = 1.0f;
    private float currentLerpTime;
    float dist;
    bool timer = true;
    float t, u = 0.0f;
    Vector3 prePos;
    Vector3 tempPos;
    Vector3 testPos;
    Vector3 testVecP = new Vector3(0, 0, 0);
    Vector3 testVecV = new Vector3(0, 0, 0);
    Vector3 testVecA = new Vector3(0, 0, 0);

    //Player Two Button Pressed
    public static int ButtonPressed;
    void Start()
    {
        if (characterselectonce == true)
        {
            CharacterLoad();
            characterselectonce = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(NetworkUpdate.numberOfPlayers);
       // transform.position = PlayerTwoPosRecv;
        //First part of dead reckoning
        if (testVecP == PlayerTwoPosRecv && testVecV == PlayerTwoVelRecv && testVecA == PlayerTwoAccRecv && timer)
        {
            ////////Estimated position/////////
            prePos = PlayerTwoPos.position + (PlayerTwoVelRecv * Time.deltaTime);

            ///////Set the position////////
            gameObject.transform.position = prePos;
        }

        else
        {
            ////gameObject.transform.position = PlayerTwoPosRecv;
            if (timer)
            {
                //current position 
                testPos = PlayerTwoPos.position;

                timer = false;
            }

            if (u < 1.0f)
            {

                //if (Vector3.Distance(testPos, prePos) < 3)
                // {
                //     testPos = PlayerTwoPos.position;
                //     u = 0.0f;
                // }
                ////////Smooth Interpolation to new postion///////
                prePos = PlayerTwoPosRecv + (PlayerTwoVelRecv * Time.deltaTime);
                u += Time.deltaTime;
                float perc = u / lerpTime;
                //tempPos = 
                perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
                ///////Set the position///////
                gameObject.transform.position = Vector3.Lerp(testPos, prePos, perc);
            }
            else
            {
                timer = true;
                u = 0.0f;
                ///////Set the position////////
                // gameObject.transform.position = PlayerTwoPosRecv;
            }


            testVecP = PlayerTwoPosRecv;
            testVecV = PlayerTwoVelRecv;
            testVecA = PlayerTwoAccRecv;
        }

        if (NetworkUpdate.RecvButtons[0] == 1)
        {
            Instantiate(Blackpepperbombg, PlayerTwoItemSpawn.position, PlayerTwoItemSpawn.rotation);
        }
        if (NetworkUpdate.RecvButtons[4] == 1)
        {
            Instantiate(Mousetrap, PlayerTwoItemSpawn.position, PlayerTwoItemSpawn.rotation);
        }
        if (NetworkUpdate.RecvButtons[5] == 1)
        {
            Instantiate(Honeycomb, PlayerTwoItemSpawn.position, PlayerTwoItemSpawn.rotation);
        }
        if (NetworkUpdate.RecvButtons[6] == 1)
        {
            Instantiate(GarlicClove, PlayerTwoItemSpawn.position, PlayerTwoItemSpawn.rotation);
        }
        
    }

    void CharacterLoad()
    {
        if (LobbyPlacement.locallyOccupied == 1) // Broco
        {
           for(int i = 0; i < 3; i++)
           {
               if(LobbyNetworking.OccupiedSlots[i] == 2)
               {
                   PlayerTwoChar = LobbyNetworking.Characters[i];
               }
           }
        }
        if (LobbyPlacement.locallyOccupied == 2) // Broco
        {
            for (int i = 0; i < 3; i++)
            {
                if (LobbyNetworking.OccupiedSlots[i] == 1)
                {
                    PlayerTwoChar = LobbyNetworking.Characters[i];
                }
            }
        }
        if (LobbyPlacement.locallyOccupied == 3) // Broco
        {
            for (int i = 0; i < 3; i++)
            {
                if (LobbyNetworking.OccupiedSlots[i] == 4)
                {
                    PlayerTwoChar = LobbyNetworking.Characters[i];
                }
            }
        }
        if (LobbyPlacement.locallyOccupied == 4) // Broco
        {
            for (int i = 0; i < 3; i++)
            {
                if (LobbyNetworking.OccupiedSlots[i] == 3)
                {
                    PlayerTwoChar = LobbyNetworking.Characters[i];
                }
            }
        }
        if(PlayerTwoChar == 1) // Broco
        {
            PlayerTwoModel[0].SetActive(true);
        }
        else if(PlayerTwoChar == 2) // Hambo
        {
            PlayerTwoModel[1].SetActive(true);
        }
        else if (PlayerTwoChar == 4) // Chicken
        {
            PlayerTwoModel[2].SetActive(true);
        }
        else if (PlayerTwoChar == 3) // Carrot
        {
            PlayerTwoModel[3].SetActive(true);
        } 
    }
}
