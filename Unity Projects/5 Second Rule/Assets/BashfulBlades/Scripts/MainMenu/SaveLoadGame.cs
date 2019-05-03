using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;

public class SaveLoadGame : MonoBehaviour
{
    [DllImport("SLD")]
    public static extern void StartSLD();
    [DllImport("SLD")]
    public static extern void SaveGameBin(int Coins, int GK, int YK, int BK, int RK, int TotalKills, int MeatPlay, int VeggiePlay, int Cleartime, int tut);
    [DllImport("SLD")]
    public static extern void LoadGameBin();
    [DllImport("SLD")]
    public static extern void SaveGameAsci(int Coins, int GK, int YK, int BK, int RK, int TotalKills, int MeatPlay, int VeggiePlay, int Cleartime, int tut);
    [DllImport("SLD")]
    public static extern void LoadGameAsci();
    [DllImport("SLD")]
    public static extern int LoadInfo(int name);

    public static int Coins; 
    public static int GreenKills;
    public static int YellowKills;
    public static int BlueKills;
    public static int RedKills;
    public static int TotalKills;
    public static int MeatPlay;
    public static int VeggiePlay;
    public static int RoundsCleared;
    public static int tuton;

    public Text[] SaveInfoDisplay;

    //string name2 = "Coins";
    // Use this for initialization
    void Start()
    {
        //StartSLD();
      
    }

    // Update is called once per frame
    void Update()
    {      
       
        SaveInfoDisplay[0].text = " Player Accumlated Coins " + Coins.ToString();
        SaveInfoDisplay[1].text = " Green Germs Killed: " + GreenKills.ToString();
        SaveInfoDisplay[2].text = " Yellow Germs Killed: " + YellowKills.ToString() ;
        SaveInfoDisplay[3].text = " Blue/Mini Germs Killed: " + BlueKills.ToString();
        SaveInfoDisplay[4].text = " Red Germs Killed: " + RedKills.ToString();
        SaveInfoDisplay[5].text = " Total Germs Killed: " + TotalKills.ToString();
        SaveInfoDisplay[6].text = " Times Played as Meat: " + MeatPlay.ToString();
        SaveInfoDisplay[7].text = " Times Played as Veggie: " + VeggiePlay.ToString();
        SaveInfoDisplay[8].text = " Total Rounds Cleared: " + RoundsCleared.ToString();

    }   
   
}
