using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyClumpSpawner : MonoBehaviour
{
    public static bool EnemyClumpSystem;
    //Green Enemy Details
    public GameObject GreenEnemy;
    private float GreenNextSpawn;
    public static int GreenAmountPerWave;
    public static int GreenAmountInc;
    //Yellow Enemy Details
    public GameObject YellowEnemy;
    private float YellowNextSpawn;
    public static int YellowAmountPerWave;
    public static int YellowAmountInc;
    //Blue Enemy Details
    public GameObject BlueEnemy;
    private float BlueNextSpawn;
    public static int BlueAmountPerWave;
    public static int BlueAmountInc;
    //Red Enemy Details
    public GameObject RedEnemy;
    private float RedNextSpawn;
    public static int RedAmountPerWave;
    public static int RedAmountInc;
    //Waypoint Info
    //public Transform[] PathOne;
    //public Transform[] PathTwo;
    //public Transform[] PathThree;
    //public Transform[] PathFly;
    //public static Transform[] PathOneStatic = new Transform[5];
    //public static Transform[] PathTwoStatic = new Transform[5];
    //public static Transform[] PathThreeStatic = new Transform[5];
    //public static Transform[] PathFlyStatic = new Transform[5];
    //SpawnerInfo
    public GameObject FlySpawnerPos;
    public GameObject[] SpawnerPos = new GameObject[3];
    public static int randomspawn = 0;  
    //Wave Details
    public static int WaveNumber = 0;
    private float WaveTimer;
    public static float TotalWaveAmount;
    public static float TotalAmountSpawned;
    public static bool WaveOver;
    public static bool CleanUpTime;
    public static bool CheckPoint;
    public GameObject[] EnemyCount;
    public Text[] EnemyCountText;
    public static bool UpdateText;
    //Level Info
    public static int LevelRound;
    public float SpawnRangeX = 0;
    public float SpawnRangeZ = 0;
    public int asdsad;
    // Use this for initialization
    void Start()
    {
        EnemyClumpSystem = false;
        //for (int i = 0; i < PathOne.Length; i++)
        //{
        //    PathOneStatic[i] = PathOne[i];
        //}
        //for (int i = 0; i < PathTwo.Length; i++)
        //{
        //    PathTwoStatic[i] = PathTwo[i];
        //}
        //for (int i = 0; i < PathThree.Length; i++)
        //{
        //    PathThreeStatic[i] = PathThree[i];
        //}
        //for (int i = 0; i < PathFly.Length; i++)
        //{
        //    PathFlyStatic[i] = PathFly[i];
        //}
       // GreenAmountPerWave = 30;
        //YellowAmountPerWave = 10;
       // BlueAmountPerWave = 1;
       // RedAmountPerWave = 0;
       // UpdateText = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyClumpSystem == true)
        {
            asdsad = randomspawn;
            EnemyCountText[4].text = "Wave: " + WaveNumber;
            if (UpdateText == true)
            {
                UpdateCount();
                UpdateText = false;
            }

            if (randomspawn > 2)
            {
                randomspawn = 0;
            }

            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x, SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z);

            //Green Spawn  
            if (Time.time > GreenNextSpawn)
            {
                if (GreenAmountPerWave > 0)
                {
                    Instantiate(GreenEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
                    GreenAmountPerWave -= 1;
                    GreenNextSpawn = Time.time + 1;
                }
            }
            //for(int i = 0; i < 2; i++)
            //{
            //    Instantiate(GreenEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
            //    GreenAmountPerWave -= 1;
            //    Instantiate(YellowEnemy, tempVec, FlySpawnerPos.transform.rotation);
            //    YellowAmountPerWave -= 1;
            //    Instantiate(BlueEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
            //    BlueAmountPerWave -= 1;
            //    Instantiate(RedEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
            //    RedAmountPerWave -= 1;
            //}
            //Yellow Spawn
            if (YellowAmountPerWave > 0)
            {
                Instantiate(YellowEnemy, tempVec, FlySpawnerPos.transform.rotation);
                YellowAmountPerWave -= 1;
            }
            //Blue Spawn
            if (BlueAmountPerWave > 0)
            {
                Instantiate(BlueEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
                BlueAmountPerWave -= 1;
            }

            //Red Spawn
            if (RedAmountPerWave > 0)
            {
                Instantiate(RedEnemy, tempVec, SpawnerPos[randomspawn].transform.rotation);
                RedAmountPerWave -= 1;
            }
            //TotalAmount
            TotalAmountSpawned = GreenAmountPerWave + YellowAmountPerWave + BlueAmountPerWave + RedAmountPerWave;
            randomspawn += 1;
            //Stuff
            if (TotalAmountSpawned == 0 && TotalWaveAmount > 0)
            {
                CleanUpTime = true;
            }
            else
            {
                CleanUpTime = false;
            }
            if (TotalWaveAmount <= 0)
            {
                CheckPoint = true;
            }
            else
            {
                CheckPoint = false;
            }
        }
    }
    void UpdateCount()
    {
        //Green
        if (GreenAmountPerWave > 0)
        {
            EnemyCount[0].SetActive(true);
            EnemyCountText[0].text = "x " + GreenAmountPerWave.ToString();
        }
        else if (GreenAmountPerWave <= 0)
        {
            EnemyCount[0].SetActive(false);
        }
        //Yellow
        if (YellowAmountPerWave > 0)
        {
            EnemyCount[1].SetActive(true);
            EnemyCountText[1].text = "x " + YellowAmountPerWave.ToString();
        }
        else if (YellowAmountPerWave <= 0)
        {
            EnemyCount[1].SetActive(false);
        }
        //Blue
        if (BlueAmountPerWave > 0)
        {
            EnemyCount[2].SetActive(true);
            EnemyCountText[2].text = "x " + (BlueAmountPerWave + (BlueAmountPerWave * 10)).ToString();
        }
        else if (BlueAmountPerWave <= 0)
        {
            EnemyCount[2].SetActive(false);
        }
        //Red
        if (RedAmountPerWave > 0)
        {
            EnemyCount[3].SetActive(true);
            EnemyCountText[3].text = "x " + RedAmountPerWave.ToString();
        }
        else if (RedAmountPerWave <= 0)
        {
            EnemyCount[3].SetActive(false);
        }
    }
}
