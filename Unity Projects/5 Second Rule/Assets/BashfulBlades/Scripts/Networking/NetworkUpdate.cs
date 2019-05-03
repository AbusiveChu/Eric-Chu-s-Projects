using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class NetworkUpdate : MonoBehaviour
{
    public SetPlayerIcons setplayeric;
    public GameObject RedCircleTeam2;
    public Transform Player1;
    public Vector3 Player1Pos;
    public Transform Player2;
    public Vector3 Orientation;
    public int[] ButtonInput = new int[7];
    public static int[] RecvButtons = new int[7];
    private int tempEnemyID;
    private GameObject tempEnemyGO;
    private int tempplayernum;
    public static int numberOfPlayers = 4;
    int allyEnemyCount = 0;
    int otherEnemyCount = 0;
    int playerParseID = 0;
    int deadNum = 0;
    //Enemy Array
    public static int Index = 0;
    public string PackageTCP = "";
    public string Package = "";
    public string Recv;

    List<string> recvParse = new List<string>();

    bool NetworkPlayer = false;
    private bool RecvParseDone = true;

    public int recvStart = 0;
    public int recvIndex = 0;
    public int parseIndex = 0;

    static public int playerNum;

    Vector3 Player1PositionLast = new Vector3(0, 0, 0);
    Vector3 Player1Velocity;
    Vector3 Player1VelocityLast = new Vector3(0, 0, 0);
    Vector3 Player1Acceleration;

    Vector3 Player2Velocity;

    void Awake()
    {
        playerNum = LobbyPlacement.locallyOccupied;
        RecvParseDone = true;
        // Debug.Log(playerNum);
    }
    void FixedUpdate()
    {
       
        RecvPackage();
        SendPackage();
    }

    void RecvPackage()
    {
        Recv = Marshal.PtrToStringAnsi(NetworkManager.GetRecvU());
        Debug.Log(Recv);

        if (RecvParseDone == true)
        {
            Debug.Log("RECVPARSE DONE ENTER");
            RecvParseDone = false;
         
            Debug.Log(numberOfPlayers);
            for (int j = 0; j < numberOfPlayers; j++)
            {
                Debug.Log("FOR LOOP ENTERED!");
                recvIndex = Recv.IndexOf('p', recvStart);
                if (recvIndex >= 0)
                {
                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                    playerParseID = Convert.ToInt32(recvParse[parseIndex]);
                    NetworkPlayer = true;
                    parseIndex++;
                    recvStart = recvIndex + 1;
                    Debug.Log("player num assigned");
                }
                else
                {
                    Debug.Log("LOOP HAS RETURNED!!");
                    RecvParseDone = true;
                    break;
                }

                //Check to exclude the local packet
                if (playerNum != playerParseID)
                {
                    Debug.Log("Playernum != PlayerparseID");
                    //Check for Ally player number
                    if (((playerNum == 1 || playerNum == 2) && (playerParseID == 1 || playerParseID == 2))
                        || ((playerNum == 3 || playerNum == 4) && (playerParseID == 3 || playerParseID == 4)))
                    {
                        //Fill in info for "Player 2"

                        recvIndex = Recv.IndexOf('@', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                            //Convert.ToInt32(recvParse[recvIndex]);
                            //////food hp here here///////
                            //SetPlayerIcons.Team1HP = Convert.ToInt32(recvParse[parseIndex]);
                            Debug.Log("I have set Team 1 HP");
                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        for (int i = 0; i < 2; i++)
                        {
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            else
                            {
                                Debug.Log("position fail");
                            }
                        }

                        recvIndex = Recv.IndexOf('v', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        print(recvParse[parseIndex - 3]);
                        print(recvParse[parseIndex - 2]);
                        print(recvParse[parseIndex - 1]);
                        ///Set player 2 position
                        PlayerTwo.PlayerTwoPosRecv = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                        Debug.Log("THISIS PLAYER TWO POS: " + PlayerTwo.PlayerTwoPosRecv);
                        //Player2.position = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));

                        for (int i = 0; i < 2; i++)
                        {
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            else
                            {
                                Debug.Log("velocity fail");
                            }
                        }
                        recvIndex = Recv.IndexOf('o', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        //Set player 2 velocity
                        PlayerTwo.PlayerTwoVelRecv = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                        Debug.Log("THIS IS PLAYER TWO VELOC");
                        for (int i = 0; i < 2; i++)
                        {
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            else
                            {
                                Debug.Log("orientation fail");
                            }
                        }

                        recvIndex = Recv.IndexOf('c', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }
                        //change player2 orientation here
                        //new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));

                        //Check for button presses
                        for (int i = 0; i < 7; i++)
                        {
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                RecvButtons[i] = Convert.ToInt32(recvParse[parseIndex]);
                                Debug.Log("Buttons");
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            else
                            {
                                Debug.Log("controller fail");
                            }
                        }
                        recvIndex = Recv.IndexOf('l', recvStart);
                        if (recvIndex >= 0)
                        {
                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        else
                        {
                            continue;
                        }

                        recvIndex = Recv.IndexOf('e', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                            allyEnemyCount = Convert.ToInt32(recvParse[parseIndex]);
                            Debug.Log("Enemy Count");
                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        for (int i = 0; i < allyEnemyCount; i++)
                        {
                            recvIndex = Recv.IndexOf('E', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                //Get the enemy ID
                                tempEnemyID = Convert.ToInt32(recvParse[parseIndex]);
                                Debug.Log("Got an Enemies HP");
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            recvIndex = Recv.IndexOf('e', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            //given position of the enemy at the given ID use the following to add the position vec3
                            //new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                            EnemyAI.EnemyList[tempEnemyID].transform.position = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                        }
                        //The list of dead units
                        recvIndex = Recv.IndexOf('d', recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                            //set number of dead enmies
                            deadNum = Convert.ToInt32(recvParse[parseIndex]);
                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }
                        for (int i = 0; i < deadNum; i++)
                        {
                            recvIndex = Recv.IndexOf(',', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                //Check for the dead enemy's ID and kill it if nessisary
                                int tempdead = Convert.ToInt32(recvParse[parseIndex]);
                                Destroy(EnemyAI.EnemyList[tempdead]);
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                        }
                    }

                    //Other team information
                    else
                    {
                        //Get information for other team player 1
                        if (playerParseID == 1 || playerParseID == 3)
                        {
                            recvIndex = Recv.IndexOf('@', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                //SetPlayerIcons.Team2HP = Convert.ToInt32(recvParse[parseIndex]);
                                //Convert.ToInt32(recvParse[parseIndex]);

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }

                                else
                                {
                                    Debug.Log("position fail");
                                }
                            }

                            recvIndex = Recv.IndexOf('v', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            //Set the otherTeamPlayer1 pos here
                            //new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));

                            //Get other team's enemy list for other player 1
                            recvIndex = Recv.IndexOf('l', recvStart);
                            if (recvIndex >= 0)
                            {
                                //parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            recvIndex = Recv.IndexOf('e', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                otherEnemyCount = Convert.ToInt32(recvParse[parseIndex]);

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            for (int i = 0; i < otherEnemyCount; i++)
                            {
                                recvIndex = Recv.IndexOf('E', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                    //Get the enemy ID
                                    tempEnemyID = Convert.ToInt32(recvParse[parseIndex]);
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf('e', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                //given position of the enemy at the given ID use the following to add the position vec3
                                EnemyAI.EnemyList[tempEnemyID].transform.position = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                            }
                            //The list of dead units
                            recvIndex = Recv.IndexOf('d', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                //set number of dead enmies
                                deadNum = Convert.ToInt32(recvParse[parseIndex]);
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            for (int i = 0; i < deadNum; i++)
                            {
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    //Check for the dead enemy's ID and kill it if nessisary
                                    //How to destroy object based on ID
                                    int tempdead = Convert.ToInt32(recvParse[parseIndex]);
                                    Destroy(EnemyAI.EnemyList[tempdead]);
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                            }
                        }

                        //Get information for other team player 2
                        else if (playerParseID == 2 || playerParseID == 4)
                        {
                            recvIndex = Recv.IndexOf('@', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                //Enemy food hp here
                                //SetPlayerIcons.Team2HP = Convert.ToInt32(recvParse[parseIndex]);

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }

                                else
                                {
                                    Debug.Log("position fail");
                                }
                            }

                            recvIndex = Recv.IndexOf('v', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            //Set the otherTeamPlayer2 pos here
                            //new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));

                            //Get other team's enemy list for other player 2
                            recvIndex = Recv.IndexOf('l', recvStart);
                            if (recvIndex >= 0)
                            {
                                //parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            recvIndex = Recv.IndexOf('e', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                otherEnemyCount = Convert.ToInt32(recvParse[parseIndex]);

                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            for (int i = 0; i < otherEnemyCount; i++)
                            {

                                recvIndex = Recv.IndexOf('E', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());

                                    //Get the enemy ID   
                                    tempEnemyID = Convert.ToInt32(recvParse[parseIndex]);
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                recvIndex = Recv.IndexOf('e', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                                EnemyAI.EnemyList[tempEnemyID].transform.position = new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));

                                //given position of the enemy at the given ID use the following to add the position vec3
                                //new Vector3(Convert.ToSingle(recvParse[parseIndex - 3]), Convert.ToSingle(recvParse[parseIndex - 2]), Convert.ToSingle(recvParse[parseIndex - 1]));
                                // }
                            }

                            //The list of dead units
                            recvIndex = Recv.IndexOf('d', recvStart);
                            if (recvIndex >= 0)
                            {
                                recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                //set number of dead enmies
                                deadNum = Convert.ToInt32(recvParse[parseIndex]);
                                parseIndex++;
                                recvStart = recvIndex + 1;
                            }

                            for (int i = 0; i < deadNum; i++)
                            {
                                recvIndex = Recv.IndexOf(',', recvStart);
                                if (recvIndex >= 0)
                                {
                                    recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                                    //Check for the dead enemy's ID and kill it if nessisary
                                    int tempdead = Convert.ToInt32(recvParse[parseIndex]);
                                    Destroy(EnemyAI.EnemyList[tempdead]);
                                    parseIndex++;
                                    recvStart = recvIndex + 1;
                                }
                            }
                        }
                    }
                }
                recvIndex = Recv.IndexOf("+", recvStart);
                if (recvIndex >= 0)
                {
                    //recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
                    //
                    //parseIndex++;
                    recvStart = recvIndex + 1;
                }

                ////////////////////////////////////////////////////////////////////////////////////
                //////Here as a Placeholder but should be done in TCP instead when you NEED to//////
                ////////////////////////////////////////////////////////////////////////////////////

                ////  g = Unique Green Guy
                //EnemySpawn.NetRecv_UniqueGreenAmountPerWave = w.einindex
                ////  G = Boss Green Guy
                //EnemySpawn.NetRecv_BossGreenAmountPerWave = w.einindex
                ////
                ////  y = Unique Yellow Guy
                //EnemySpawn.NetRecv_UniqueYellowAmountPerWave = w.einindex
                ////  Y = Boss Yellow Guy
                //EnemySpawn.NetRecv_BossYellowAmountPerWave = w.einindex
                ////
                ////  b = Unique Blue Guy
                //EnemySpawn.NetRecv_UniqueBlueAmountPerWave = w.einindex
                ////  B = Boss Blue Guy
                //EnemySpawn.NetRecv_BossBlueAmountPerWave = w.einindex
                ////
                ////  m = Unique Red Guy
                //EnemySpawn.NetRecv_UniqueRedAmountPerWave = w.einindex
                ////  M = Boss Red Guy
                //EnemySpawn.NetRecv_BossRedAmountPerWave = w.einindex

                
            }
            recvStart = 0;
            parseIndex = 0;
            recvParse.Clear();
            RecvParseDone = true;
            Debug.Log("ParseDONE");
        }
    }

    void Start()
    {
        Player1 = GameObject.Find("Player 1").transform;
        Player2 = GameObject.Find("Player 2").transform;
    }

    void Update()
    {
        //Calculate local velocity
        Player1Velocity = (Player1.position - Player1PositionLast) / Time.deltaTime;
        Player1PositionLast = Player1.position;

        //Get Orientation
        Orientation = Shooting.hitDir;
    }


    //void ParseEnemies()
    //{
    //    recvIndex = Recv.IndexOf('P', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvStart++;

    //        tempplayernum = Convert.ToInt32(recvParse[recvIndex]);
    //        if (LobbyPlacement.locallyOccupied == 3 || LobbyPlacement.locallyOccupied == 4 && tempplayernum == 1 || tempplayernum == 2)
    //        {
    //            ParseEnemyValues();
    //        }
    //        if (LobbyPlacement.locallyOccupied == 1 || LobbyPlacement.locallyOccupied == 2 && tempplayernum == 3 || tempplayernum == 4)
    //        {
    //            ParseEnemyValues();
    //        }
    //    }
    //    recvIndex = Recv.IndexOf('R', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvStart++;
    //        int tempready;
    //        tempready = Convert.ToInt32(recvParse[recvIndex]);

    //        if (tempready == 1)
    //        {
    //            SetPlayerIcons.PlayerReady[tempplayernum] = true;
    //            setplayeric.SetReady(tempplayernum);
    //        }
    //    }

    //}
    //void ParseEnemyValues()
    //{
    //    recvIndex = Recv.IndexOf('g', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
            
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('G', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('y', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('Y', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('b', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('B', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('m', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //    recvIndex = Recv.IndexOf('M', recvStart);
    //    if (recvIndex >= 0)
    //    {
    //        recvParse.Add(Recv.Substring(recvStart, recvIndex - recvStart).Trim());
    //        //get the value at recvParse[parseIndex] and convert it
    //        parseIndex++;
    //        recvStart = recvIndex + 1;
    //    }
    //}
    string ButtonCheck()
    {
        string tempPack = "";
        if (ControllerManager.GetButtonDown(4))
            ButtonInput[0] = 1;
        else
            ButtonInput[0] = 0;

        tempPack += ButtonInput[0].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(5))
            ButtonInput[1] = 1;
        else
            ButtonInput[1] = 0;

        tempPack += ButtonInput[1].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(6))
            ButtonInput[2] = 1;
        else
            ButtonInput[2] = 0;

        tempPack += ButtonInput[2].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(7))
            ButtonInput[3] = 1;
        else
            ButtonInput[3] = 0;

        tempPack += ButtonInput[3].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(14))
            ButtonInput[4] = 1;
        else
            ButtonInput[4] = 0;

        tempPack += ButtonInput[4].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(15))
            ButtonInput[5] = 1;
        else
            ButtonInput[5] = 0;

        tempPack += ButtonInput[5].ToString();
        tempPack += ",";

        if (ControllerManager.GetButtonDown(16))
            ButtonInput[6] = 1;
        else
            ButtonInput[6] = 0;

        tempPack += ButtonInput[6].ToString();
        tempPack += ",";

        return tempPack;
    }

    void SendPackage()
    {
        Package = "";

        Package += LobbyPlacement.locallyOccupied;
        Package += "p";
        Package += SetPlayerIcons.Team1HP;
        //Player Position
        Package += "@";
        Package += Player1.transform.position.x + 0.023596;
        Package += ",";
        Package += Player1.transform.position.y - 3.62;
        Package += ",";
        Package += Player1.transform.position.z + 0.24189;

        //Send Velocity
        Package += "v";
        Package += Player1Velocity.x;
        Package += ",";
        Package += Player1Velocity.y;
        Package += ",";
        Package += Player1Velocity.z;

        //Orientation Based on HitDir
        Package += "o";
        Package += Orientation.x;
        Package += ",";
        Package += Orientation.y;
        Package += ",";
        Package += Orientation.z;

        Package += "c";

        Package += ButtonCheck();

        if (!EnemySpawn.CheckPoint && EnemySpawn.WaveNumber > 0)
        {
            NetworkEnemySpawn.BuildNormalEnemyPackage();
            Package += NetworkEnemySpawn.EnemyLocationPackage;
        }
        //Debug.Log(System.Text.ASCIIEncoding.Unicode.GetByteCount(Package)/2);
        //Debug.Log(Package);
        NetworkManager.SendPackU(Package);
        NetworkEnemySpawn.EnemyLocationPackage = "";
        NetworkEnemySpawn.EnemyPackage = "";
        NetworkEnemySpawn.EnemyListEven = "";
        NetworkEnemySpawn.EnemyListOdd = "";
    }
    public void SendPackageTCP()
    {
        PackageTCP += LobbyPlacement.locallyOccupied;
        PackageTCP += "P";
        PackageTCP += Actions.Ready;
        PackageTCP += "R";
        
        //Send what enemies were bought in shop
        NetworkEnemySpawn.SendEnemyPackage();
        PackageTCP += NetworkEnemySpawn.EnemyPackage;
        // Debug.Log(PackageTCP);
        NetworkManager.SendPackT(PackageTCP);
        PackageTCP = "";
    }
}