using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;


public class LobbyNetworking : MonoBehaviour
{
    public static string lobbyPack = "";

    public static string sendPack = "";

    public static int[] OccupiedSlots = new int[3];
    public static string[] names = new string[3];
    public static int[] Characters = new int[3];
    public int recvStart = 0;
    public int recvIndex = 0;
    public int parseIndex = 0;

    public LobbyPlacement[] slotButtons;

    List<string> recvParse = new List<string>();

    bool ParseDone = true;

    public LobbyEvent slotRecv;

    public void OnSlotClick(LobbyPlacement slot)
    {
        //slotButtons.IndexOf(slot);
        for (int i = 0; i < slotButtons.Length; i++)
        {
            if ((slotButtons[i] != slot && !slotButtons[i].Occupied) || (i != LobbyPlacement.locallyOccupied && slotButtons[i].Occupied))
            {
                print(i);
                slotButtons[i].ClearSlot();
            }
        }
    }

    void Update()
    {
        SendPack();
        RecvPack();
        //Debug.Log("LOOP UPDATE");
        //Debug.Log(OccupiedSlots[0]);
    }

    void RecvPack()
    {
        lobbyPack = Marshal.PtrToStringAnsi(NetworkManager.GetRecvT());
      
        if (lobbyPack != null)
        {
            Debug.Log(lobbyPack);
            if (ParseDone)
            {
                ParseDone = false;

                for (int i = 0; i < 3; i++)
                {


                    //LOBBY NUMBER
                    recvIndex = lobbyPack.IndexOf(",", recvStart);
                    if (recvIndex >= 0)
                    {
                        recvParse.Add(lobbyPack.Substring(recvStart, recvIndex - recvStart).Trim());
                        OccupiedSlots[i] = Convert.ToInt32(recvParse[parseIndex]);
                    
                        parseIndex++;
                        recvStart = recvIndex + 1;
                    }
                    else
                    {
                        ParseDone = true;
                        break;
                    }
                    if (!(OccupiedSlots[i] == LobbyPlacement.locallyOccupied))
                    {
                        //NAME
                        recvIndex = lobbyPack.IndexOf(",", recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(lobbyPack.Substring(recvStart, recvIndex - recvStart).Trim());
                            names[i] = recvParse[parseIndex];

                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        //Character selection
                        recvIndex = lobbyPack.IndexOf(",", recvStart);
                        if (recvIndex >= 0)
                        {
                            recvParse.Add(lobbyPack.Substring(recvStart, recvIndex - recvStart).Trim());
                            Characters[i] = Convert.ToInt32(recvParse[parseIndex]);
                           
                            parseIndex++;
                            recvStart = recvIndex + 1;
                        }

                        if (slotRecv != null)
                        {
                            
                            slotRecv.Invoke(OccupiedSlots[i], names[i], Characters[i]);
                        }
                    }
                    recvIndex = lobbyPack.IndexOf("+", recvStart);
                    if (recvIndex >= 0)
                    {
                        recvStart = recvIndex + 1;
                        if (recvStart > lobbyPack.Length - 1)
                        {
                            
                            break;
                        }
                    }
                    //Check for the start of the game
                    recvIndex = lobbyPack.IndexOf("~");
                    if (recvIndex >= 0)
                    {
                        recvParse.Add(lobbyPack.Substring(recvStart, recvIndex - recvStart).Trim());
                        NetworkUpdate.numberOfPlayers = Convert.ToInt32(recvParse[parseIndex]);
                        Debug.Log(NetworkUpdate.numberOfPlayers);
                      
                        parseIndex++;
                        recvStart = recvIndex + 1;

                        recvIndex = lobbyPack.IndexOf("G");
                        if (recvIndex >= 0)
                        {
                            SceneManager.LoadScene(5);
                        }
                    }
                    else
                    {
                       
                        continue;
                    }
                    //Set lobby names and positions

                }
            }
            recvStart = 0;
            parseIndex = 0;
            recvParse.Clear();
            ParseDone = true;
        }
    }


    void SendPack()
    {
        sendPack = "";
    
        if (!HostStartGame.GameStart && LobbyPlacement.changePlaces)
        {
            sendPack += LobbyPlacement.locallyOccupied;
            sendPack += ",";
            sendPack += ChangeName.Name;
            sendPack += ",";
            sendPack += SetLevel.CharSelect;
            sendPack += ",";
           
            NetworkManager.SendPackT(sendPack);
        
        }
 
    }
}