using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NetworkEnemySpawn : MonoBehaviour
{
    public static string EnemyListOdd;
    public static string EnemyListEven;
    //Green Enemy Details       
    public static int NetSend_UniqueGreenAmountPerWave;
    public static int NetSend_BossGreenAmountPerWave;
    //Yellow Enemy DeiNetSend_ls      
    public static int NetSend_UniqueYellowAmountPerWave;
    public static int NetSend_BossYellowAmountPerWave;
    //Blue Enemy DetasNetSend_     
    public static int NetSend_UniqueBlueAmountPerWave;
    public static int NetSend_BossBlueAmountPerWave;
    //Red Enemy Detai NetSend_  
    public static int NetSend_UniqueRedAmountPerWave;
    public static int NetSend_BossRedAmountPerWave;
    //Enemy Package
    public static string EnemyPackage;
    public static string EnemyLocationPackage;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void SendEnemyPackage()
    {
        EnemyPackage = "";
        EnemyPackage += "g";
        EnemyPackage += NetSend_UniqueGreenAmountPerWave;
        EnemyPackage += "G";
        EnemyPackage += NetSend_BossGreenAmountPerWave;
        EnemyPackage += "y";
        EnemyPackage += NetSend_UniqueYellowAmountPerWave;
        EnemyPackage += "Y";
        EnemyPackage += NetSend_BossYellowAmountPerWave;
        EnemyPackage += "b";
        EnemyPackage += NetSend_UniqueBlueAmountPerWave;
        EnemyPackage += "B";
        EnemyPackage += NetSend_BossBlueAmountPerWave;
        EnemyPackage += "m";
        EnemyPackage += NetSend_UniqueRedAmountPerWave;
        EnemyPackage += "M";
        EnemyPackage += NetSend_BossRedAmountPerWave;
        //Reset after sent
        NetSend_UniqueGreenAmountPerWave = 0;
        NetSend_BossGreenAmountPerWave = 0;
        NetSend_UniqueYellowAmountPerWave = 0;
        NetSend_BossYellowAmountPerWave = 0;
        NetSend_UniqueBlueAmountPerWave = 0;
        NetSend_BossBlueAmountPerWave = 0;
        NetSend_UniqueRedAmountPerWave = 0;
        NetSend_BossRedAmountPerWave = 0;
    }
    public static void BuildNormalEnemyPackage()
    {
        EnemyLocationPackage += "l";
        EnemyLocationPackage += EnemySpawn.TotalTotalWaveAmount[EnemySpawn.WaveNumber];
        foreach (KeyValuePair<int, GameObject> EnemyList in EnemyAI.EnemyList)
        {
            if (EnemyAI.EnemyList.ContainsKey(EnemyList.Key) == true)
            {
                Transform tempTranny;
                tempTranny = EnemyList.Value.transform;
                if (EnemyList.Key % 2 == 0)
                {
                    EnemyListEven += "e";
                    EnemyListEven += EnemyList.Key;
                    EnemyListEven += "E";
                    EnemyListEven += tempTranny.position.x;
                    EnemyListEven += ",";
                    EnemyListEven += tempTranny.position.y;
                    EnemyListEven += ",";
                    EnemyListEven += tempTranny.position.z;

                }
                else if (EnemyList.Key % 2 != 0)
                {
                    EnemyListOdd += "e";
                    EnemyListOdd += EnemyList.Key;
                    EnemyListOdd += "E";
                    EnemyListOdd += tempTranny.position.x;
                    EnemyListOdd += ",";
                    EnemyListOdd += tempTranny.position.y;
                    EnemyListOdd += ",";
                    EnemyListOdd += tempTranny.position.z;
                }
            }
        }
        if (SetLevel.PlayerNumber == 1)
        {
            EnemyLocationPackage += EnemyListOdd + "e" + "d" + EnemyAI.DeadAmount + "," + EnemyAI.DeathList;
        }
        else if (SetLevel.PlayerNumber == 2)
        {
            EnemyLocationPackage += EnemyListEven + "e" + "d" + EnemyAI.DeadAmount + "," + EnemyAI.DeathList;
        }

    }
}
