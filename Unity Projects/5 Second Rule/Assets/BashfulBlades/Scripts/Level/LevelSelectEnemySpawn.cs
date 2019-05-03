using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class LevelSelectEnemySpawn : MonoBehaviour
{

    //Init
    [DllImport("dtsystem")]
    public static extern void StartStory();
    //Init
    [DllImport("dtsystem")]
    public static extern void saveEnemyRound(int RoundNumber, int GCWONE, int YCWONE, int BCWONE, int RCWONE, int GCWTWO, int YCWTWO, int BCWTWO, int RCWTWO, int GCWTHREE, int YCWTHREE, int BCWTHREE, int RCWTHREE, int GCWFOUR, int YCWFOUR, int BCWFOUR, int RCWFOUR, int GCWFIVE, int YCWFIVE, int BCWFIVE, int RCWFIVE);
    //Init
    [DllImport("dtsystem")]
    public static extern void loadEnemyRound(int RoundNumber);
    //Init
    [DllImport("dtsystem")]
    public static extern int loadEnemyWave(int Germ, int Wave);
    //Init
    [DllImport("dtsystem")]
    public static extern float loadEnemyDelay(int Germ, int Wave);
    //Waypoint Info
    public Transform[] PathOne;
    public Transform[] PathTwo;
    public Transform[] PathThree;
    public Transform[] PathFlyOne;
    public Transform[] PathFlyTwo;
    public Transform[] PathFlyThree;
    public static Transform[] PathOneStatic = new Transform[5];
    public static Transform[] PathTwoStatic = new Transform[5];
    public static Transform[] PathThreeStatic = new Transform[5];
    public static Transform[] PathFlyOneStatic = new Transform[5];
    public static Transform[] PathFlyTwoStatic = new Transform[5];
    public static Transform[] PathFlyThreeStatic = new Transform[5];
    //SpawnerInfo
    public GameObject[] FlySpawnerPos = new GameObject[3];
    public GameObject[] SpawnerPos = new GameObject[3];
    public static int randomspawn;
    public int[] EnemyID = new int[4];
    //Green Enemy Details
    public GameObject Dustmite_prefab;
    public static float[] GreenDelay = new float[6];
    private float GreenNextSpawn;
    public static int[] GreenAmountPerWave = new int[6];
    //Yellow Enemy Details
    public GameObject FlyingEnemy_prefab;
    public static float[] YellowDelay = new float[6];
    private float YellowNextSpawn;
    public static int[] YellowAmountPerWave = new int[6];
    //Blue Enemy Details
    public GameObject BlueGerm_prefab;
    public static float[] BlueDelay = new float[6];
    private float BlueNextSpawn;
    public static int[] BlueAmountPerWave = new int[6];
    //Red Enemy Details
    public GameObject Meatball_prefab;
    public static float[] RedDelay = new float[6];
    private float RedNextSpawn;
    public static int[] RedAmountPerWave = new int[6];
    //
    public float GreenCD;
    public float YellowCD;
    public float BlueCD;
    public float RedCD;
    //Wave Details
    public static int WaveNumber = 0;
    private float WaveTimer;
    public static float[] TotalWaveAmount = new float[6];
    public static float[] TotalAmountSpawned = new float[6];
    public static bool WaveOver;
    public static bool CleanUpTime;
    public static bool CheckPoint;
    public GameObject[] EnemyCount;
    public Text[] EnemyCountText;
    public static bool UpdateText;
    //Level Info
    public static int LevelRound;
    public float SpawnRange = 60;
    // Use this for initialization
    void Start()
    {
        LevelRound = 777;
        WaveNumber = 1;
        StartStory();
        //Load Spawn Count
        loadEnemyRound(LevelRound);
        for (int i = 1; i < 6; i++)
        {
            GreenAmountPerWave[i] = loadEnemyWave(0, i);
            GreenDelay[i] = loadEnemyDelay(0, i);
            YellowAmountPerWave[i] = loadEnemyWave(1, i);
            YellowDelay[i] = loadEnemyDelay(1, i);
            BlueAmountPerWave[i] = loadEnemyWave(2, i);
            BlueDelay[i] = loadEnemyDelay(2, i);
            RedAmountPerWave[i] = loadEnemyWave(3, i);
            RedDelay[i] = loadEnemyDelay(3, i);
            TotalAmountSpawned[i] = 0;
            TotalWaveAmount[i] = 0;
            TotalWaveAmount[i] = GreenAmountPerWave[i] + YellowAmountPerWave[i] + BlueAmountPerWave[i] + RedAmountPerWave[i] + (BlueAmountPerWave[i] * 10);
        }
        for (int i = 0; i < PathOne.Length; i++)
        {
            PathOneStatic[i] = PathOne[i];
            PathTwoStatic[i] = PathTwo[i];
            PathThreeStatic[i] = PathThree[i];
            PathFlyOneStatic[i] = PathFlyOne[i];
            PathFlyTwoStatic[i] = PathFlyTwo[i];
            PathFlyThreeStatic[i] = PathFlyThree[i];
        }

       // WaveNumber = 0;
        UpdateText = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyClumpSpawner.EnemyClumpSystem == false)
        {
            //EnemyCountText[4].text = "Wave: " + WaveNumber;
            if (UpdateText == true)
            {
               // UpdateCount();
                UpdateText = false;
            }

            if (randomspawn > 2)
            {
                randomspawn = 0;
            }

            if(Time.time > GreenCD)
            {
            ////Green Spawn    
            if (GreenAmountPerWave[WaveNumber] > 0)
            {
                Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRange, SpawnRange)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRange, SpawnRange)));
                Instantiate(Dustmite_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation);
                GreenAmountPerWave[WaveNumber] -= 1;
                //Dustmite_prefab.EnemyType = 1;
               // Dustmite_prefab.EnemyID = EnemyID[0];
                GreenCD = Time.time + GreenDelay[WaveNumber];
                EnemyID[0]++;
            }
        }
            if (Time.time > YellowCD)
            {
                ////Yellow Spawn    
                if (YellowAmountPerWave[WaveNumber] > 0)
                {
                    Vector3 tempVec = new Vector3(FlySpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRange, SpawnRange)), FlySpawnerPos[randomspawn].transform.position.y, FlySpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRange, SpawnRange)));
                    Instantiate(FlyingEnemy_prefab, tempVec, FlySpawnerPos[randomspawn].transform.rotation);
                    YellowAmountPerWave[WaveNumber] -= 1;
                    //FlyingEnemy_prefab.EnemyType = 4;
                    //FlyingEnemy_prefab.EnemyID = EnemyID[1];
                    YellowCD = Time.time + YellowDelay[WaveNumber];
                    EnemyID[1]++;
                }
            }
            if (Time.time > BlueCD)
            {
                //Blue Spawn
                if (BlueAmountPerWave[WaveNumber] > 0)
                {
                    Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRange, SpawnRange)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRange, SpawnRange)));
                    Instantiate(BlueGerm_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation);
                    BlueAmountPerWave[WaveNumber] -= 1;
                    //BlueGerm_prefab.EnemyType = 7;
                    BlueCD = Time.time + BlueDelay[WaveNumber];
                    //BlueGerm_prefab.EnemyID = EnemyID[2];
                    EnemyID[2]++;
                }
            }
            //Red Spawn       
            if (RedAmountPerWave[WaveNumber] > 0)
            {
                Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRange, SpawnRange)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRange, SpawnRange)));
                Instantiate(Meatball_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation);
                RedAmountPerWave[WaveNumber] -= 1;
                //Meatball_prefab.EnemyType = 10;
                //Meatball_prefab.EnemyID = EnemyID[3];
                RedCD = Time.time + RedDelay[WaveNumber];
                EnemyID[3]++;
            }
            //
            TotalAmountSpawned[WaveNumber] = GreenAmountPerWave[WaveNumber] + YellowAmountPerWave[WaveNumber] + BlueAmountPerWave[WaveNumber] + RedAmountPerWave[WaveNumber];
            randomspawn += 1;

            if (TotalAmountSpawned[WaveNumber] == 0)
            {
                GreenAmountPerWave[WaveNumber] = 60;
                YellowAmountPerWave[WaveNumber] = 30;
                BlueAmountPerWave[WaveNumber] = 15;
                RedAmountPerWave[WaveNumber] = 5;
            }
            else
            {
                CleanUpTime = false;
            }
            if (TotalWaveAmount[WaveNumber] <= 0)
            {
                CheckPoint = true;
            }
            else
            {
                CheckPoint = false;
            }
        }
    }

    //void UpdateCount()
    //{
    //    //Green
    //    if (GreenAmountPerWave[WaveNumber] > 0)
    //    {
    //        EnemyCount[0].SetActive(true);
    //        EnemyCountText[0].text = "x " + GreenAmountPerWave[WaveNumber].ToString();
    //    }
    //    else if (GreenAmountPerWave[WaveNumber] <= 0)
    //    {
    //        EnemyCount[0].SetActive(false);
    //    }
    //    //Yellow
    //    if (YellowAmountPerWave[WaveNumber] > 0)
    //    {
    //        EnemyCount[1].SetActive(true);
    //        EnemyCountText[1].text = "x " + YellowAmountPerWave[WaveNumber].ToString();
    //    }
    //    else if (YellowAmountPerWave[WaveNumber] <= 0)
    //    {
    //        EnemyCount[1].SetActive(false);
    //    }
    //    //Blue
    //    if (BlueAmountPerWave[WaveNumber] > 0)
    //    {
    //        EnemyCount[2].SetActive(true);
    //        EnemyCountText[2].text = "x " + (BlueAmountPerWave[WaveNumber] + (BlueAmountPerWave[WaveNumber] * 10)).ToString();
    //    }
    //    else if (BlueAmountPerWave[WaveNumber] <= 0)
    //    {
    //        EnemyCount[2].SetActive(false);
    //    }
    //    //Red
    //    if (RedAmountPerWave[WaveNumber] > 0)
    //    {
    //        EnemyCount[3].SetActive(true);
    //        EnemyCountText[3].text = "x " + RedAmountPerWave[WaveNumber].ToString();
    //    }
    //    else if (RedAmountPerWave[WaveNumber] <= 0)
    //    {
    //        EnemyCount[3].SetActive(false);
    //    }
    //}
}
