using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class SetPlayerIcons : MonoBehaviour
{
    public GameObject[] Player1Icon = new GameObject[5];
    public GameObject[] Player2Icon = new GameObject[5];
    public GameObject[] Player3Icon = new GameObject[5];
    public GameObject[] Player4Icon = new GameObject[5];
    public GameObject[] PlayerReadyIcon = new GameObject[5];
    public static int Player3Character = 0;
    public static int Player4Character = 0;
    public bool DoOnce;

    public static float Team1HP;
    public static float Team2HP;
    public Image Team1HPImg;
    public Image Team2HPImg;
    public static bool[] PlayerReady = new bool[5];

    public int recvStart = 0;
    public int recvIndex = 0;
    public int parseIndex = 0;
    public string TCPRecv;
    bool RecvParseDone = true;

    List<string> recvParse = new List<string>();

    private int tempplayernum;
    // Use this for initialization
    void Start()
    {
        DoOnce = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkUpdate.numberOfPlayers == 3)
        {
            PlayerReadyIcon[4].SetActive(false);
            PlayerReady[4] = true;
        }
        if (NetworkUpdate.numberOfPlayers == 2)
        {
            PlayerReadyIcon[3].SetActive(false);
            PlayerReady[3] = true;
            PlayerReadyIcon[4].SetActive(false);
            PlayerReady[4] = true;
        }
        Team1HP = Food.PlayerHP;
        Team1HPImg.fillAmount = Team1HP / 100;
        Team2HPImg.fillAmount = Team2HP / 100;

        if (DoOnce == false)
        {
            Player1Icon[SetLevel.CharSelect].SetActive(true);
            Player2Icon[PlayerTwo.PlayerTwoChar].SetActive(true);

            for (int i = 0; i < 3; i++)
            {
                if (LobbyNetworking.OccupiedSlots[i] == 4)
                {
                    Player4Character = LobbyNetworking.Characters[i];
                }
            }


            for (int i = 0; i < 3; i++)
            {
                if (LobbyNetworking.OccupiedSlots[i] == 3)
                {
                    Player3Character = LobbyNetworking.Characters[i];
                }
            }

            Player3Icon[Player3Character].SetActive(true);
            Player4Icon[Player4Character].SetActive(true);
        }
        if(EnemySpawn.CheckPoint == true)
        {
            ParseTCP();
        }
    }
    void ParseEnemyValues()
    {
        recvIndex = TCPRecv.IndexOf('g', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_UniqueGreenAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('G', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_BossGreenAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('y', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_UniqueYellowAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('Y', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_BossYellowAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('b', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_UniqueBlueAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('B', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_BossBlueAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('m', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_UniqueRedAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
        recvIndex = TCPRecv.IndexOf('M', recvStart);
        if (recvIndex >= 0)
        {
            recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
            EnemySpawn.NetRecv_BossRedAmountPerWave = Convert.ToInt32(recvParse[parseIndex]);
            parseIndex++;
            recvStart = recvIndex + 1;
        }
    }
    public void ParseTCP()
    {
        TCPRecv = Marshal.PtrToStringAnsi(NetworkManager.GetRecvT());

        for (int i = 0; i < NetworkUpdate.numberOfPlayers; i++)
        {
            recvIndex = TCPRecv.IndexOf('P', recvStart);
            if (recvIndex >= 0)
            {
                recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
                tempplayernum = Convert.ToInt32(recvParse[parseIndex]);

                recvStart = recvIndex + 1;
                parseIndex++;

            }
            recvIndex = TCPRecv.IndexOf('R', recvStart);
            if (recvIndex >= 0)
            {
                int tempready;
                recvParse.Add(TCPRecv.Substring(recvStart, recvIndex - recvStart).Trim());
                tempready = Convert.ToInt32(recvParse[parseIndex]);

                if (tempready == 1)
                {
                    SetPlayerIcons.PlayerReady[tempplayernum] = true;
                    SetReady(tempplayernum);
                }
                recvStart = recvIndex + 1;
                parseIndex++;
            }
            if ((LobbyPlacement.locallyOccupied == 3 || LobbyPlacement.locallyOccupied == 4 && tempplayernum == 1 || tempplayernum == 2) ||
                (LobbyPlacement.locallyOccupied == 1 || LobbyPlacement.locallyOccupied == 2 && tempplayernum == 3 || tempplayernum == 4))
            {
                ParseEnemyValues();
            }
            recvIndex = TCPRecv.IndexOf("+", recvStart);
            if (recvIndex >= 0)
            {
                recvStart = recvIndex + 1;
            }
        }
        recvStart = 0;
        parseIndex = 0;
        recvParse.Clear();
    }

    public void SetReady(int i)
    {
        if (PlayerReady[i] == true)
        {
            PlayerReadyIcon[i].SetActive(true);
        }
    }
}
