using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
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
    //Spawn Increment Amount;
    public float GreenGermIncAmount;
    public float YellowGermIncAmount;
    public float BlueGermIncAmount;
    public float RedGermIncAmount;
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
    public static int EnemyID;
    //Green Enemy Details
    public EnemyAI Dustmite_prefab;
    public EnemyAI UniqDustmite_prefab;
    public EnemyAI BossDustmite_prefab; 
    private float GreenNextSpawn;
    public static float[] GreenAmountPerWave = new float[50];    
    public static int NetRecv_UniqueGreenAmountPerWave;
    public static int NetRecv_BossGreenAmountPerWave;
    //Yellow Enemy Details
    public EnemyAI FlyingEnemy_prefab;
    public EnemyAI UniqFlyingEnemy_prefab;
    public EnemyAI BossFlyingEnemy_prefab;    
    private float YellowNextSpawn;
    public static float[] YellowAmountPerWave = new float[50];
    public static int NetRecv_UniqueYellowAmountPerWave;
    public static int NetRecv_BossYellowAmountPerWave;
    //Blue Enemy Details
    public EnemyAI BlueGerm_prefab;
    public EnemyAI UniqBlueGerm_prefab;
    public EnemyAI BossBlueGerm_prefab;    
    private float BlueNextSpawn;
    public static float[] BlueAmountPerWave = new float[50];   
    public static int NetRecv_UniqueBlueAmountPerWave;
    public static int NetRecv_BossBlueAmountPerWave;
    //Red Enemy Details
    public EnemyAI Meatball_prefab;
    public EnemyAI UniqMeatball_prefab;
    public EnemyAI BossMeatball_prefab;    
    private float RedNextSpawn;
    public static float[] RedAmountPerWave = new float[50];  
    public static int NetRecv_UniqueRedAmountPerWave;
    public static int NetRecv_BossRedAmountPerWave;
    //Wave Details
    public static int WaveNumber = 0;
    private float WaveTimer;
    public static float[] TotalWaveAmount = new float[50];
    public static float[] TotalAmountSpawned = new float[50];
    public static float[] TotalTotalWaveAmount = new float[50];
    public static float NetworkEnemyAmount;
    public static bool WaveOver;
    public static bool CleanUpTime;
    public static bool CheckPoint;
    public GameObject[] EnemyCount;
    public Text[] EnemyCountText;
    public static bool UpdateText;
    //Level Info
    public static int LevelRound;
    public float SpawnRange = 60;
    public float SpawnRangeX = 60;
    public float SpawnRangeZ = 60;
    public float SpawnDelay = 10;
    public static float SpawnCD;
    public static int Spawncounter;
    private int prevspawn;
    //Red Circle
    public RedCircle RedCirclePrefab;
    // Use this for initialization
    void Start()
    {       
        
            for (int i = 1; i < TotalWaveAmount.Length; i++)
            {
                if (i <= 1)
                {
                    GreenAmountPerWave[i] = 20;
                    YellowAmountPerWave[i] = 20;
                    BlueAmountPerWave[i] = 20;
                    RedAmountPerWave[i] = 20;                 

                }
                else if (i > 1)
                {
                    GreenAmountPerWave[i] = GreenAmountPerWave[i - 1] + GreenGermIncAmount;
                    YellowAmountPerWave[i] = YellowAmountPerWave[i - 1] + YellowGermIncAmount;
                    BlueAmountPerWave[i] = BlueAmountPerWave[i - 1] + BlueGermIncAmount;
                    RedAmountPerWave[i] = RedAmountPerWave[i - 1] + RedGermIncAmount;
                }
                
                TotalWaveAmount[i] = GreenAmountPerWave[i] + YellowAmountPerWave[i] + (int)BlueAmountPerWave[i] + (int)RedAmountPerWave[i];
                TotalTotalWaveAmount[i] = GreenAmountPerWave[i] + YellowAmountPerWave[i] + (int)BlueAmountPerWave[i] + (int)RedAmountPerWave[i];
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
        WaveNumber = 0;
        randomspawn = Random.Range(0,3);
        UpdateText = true;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCountText[4].text = "Wave: " + WaveNumber;
        if (UpdateText == true)
        {
            EnemyID = 0;
            EnemyAI.EnemyList.Clear();
            EnemyAI.DeathList = "";
            EnemyAI.DeadAmount = 0;
            NetworkEnemyAmount = NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave + NetRecv_UniqueBlueAmountPerWave + NetRecv_BossBlueAmountPerWave + NetRecv_UniqueRedAmountPerWave + NetRecv_BossRedAmountPerWave;
            TotalWaveAmount[WaveNumber] = TotalWaveAmount[WaveNumber] + NetworkEnemyAmount;
            TotalTotalWaveAmount[WaveNumber] = TotalTotalWaveAmount[WaveNumber] + NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave + NetRecv_UniqueBlueAmountPerWave + NetRecv_BossBlueAmountPerWave + NetRecv_UniqueRedAmountPerWave + NetRecv_BossRedAmountPerWave;
            UpdateCount();
            UpdateText = false;
        }
      
        if (Spawncounter > (TotalTotalWaveAmount[WaveNumber] / 3) && WaveNumber > 0 && CheckPoint == false)
        {
            SpawnCD = Time.time + SpawnDelay;
            prevspawn = randomspawn;
            randomspawn = Random.Range(0, 3);
            while (randomspawn == prevspawn)
            {
                randomspawn = Random.Range(0, 3);
            }            
            Spawncounter = 0;
        }
        if (Time.time > SpawnCD)
        {
            SpawnNormalGerms();
           
            if (WaveNumber > 0 && CheckPoint == false)
            {
                Spawncounter++;
            }
            SpawnUniqueGerms();
            SpawnBossGerms();
        }
        //
        TotalAmountSpawned[WaveNumber] = GreenAmountPerWave[WaveNumber] + YellowAmountPerWave[WaveNumber] + (int)BlueAmountPerWave[WaveNumber] + (int)RedAmountPerWave[WaveNumber] + NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave + NetRecv_UniqueBlueAmountPerWave + NetRecv_BossBlueAmountPerWave + NetRecv_UniqueRedAmountPerWave + NetRecv_BossRedAmountPerWave;


        if (TotalAmountSpawned[WaveNumber] == 0 && TotalWaveAmount[WaveNumber] > 0)
        {
            CleanUpTime = true;
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
    void UpdateCount()
    {
        //Green
        if ((GreenAmountPerWave[WaveNumber] + NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave) > 0.9)
        {
            EnemyCount[0].SetActive(true);
            EnemyCountText[0].text = "x " + (GreenAmountPerWave[WaveNumber] + NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave).ToString();
        }
        else if ((GreenAmountPerWave[WaveNumber] + NetRecv_UniqueGreenAmountPerWave + NetRecv_BossGreenAmountPerWave) <= 0)
        {
            EnemyCount[0].SetActive(false);
        }
        //Yellow
        if ((YellowAmountPerWave[WaveNumber] + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave) > 0.9)
        {
            EnemyCount[1].SetActive(true);
            EnemyCountText[1].text = "x " + (YellowAmountPerWave[WaveNumber] + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave).ToString();
        }
        else if ((YellowAmountPerWave[WaveNumber] + NetRecv_UniqueYellowAmountPerWave + NetRecv_BossYellowAmountPerWave) <= 0)
        {
            EnemyCount[1].SetActive(false);
        }
        //Blue
        if ((BlueAmountPerWave[WaveNumber] + NetRecv_UniqueBlueAmountPerWave + NetRecv_BossBlueAmountPerWave) > 0.9)
        {
            EnemyCount[2].SetActive(true);
            EnemyCountText[2].text = "x " + ((int)BlueAmountPerWave[WaveNumber]  + NetRecv_UniqueBlueAmountPerWave + NetRecv_BossBlueAmountPerWave).ToString("0");
        }
        else if (BlueAmountPerWave[WaveNumber] <= 0)
        {
            EnemyCount[2].SetActive(false);
        }
        //Red
        if ((RedAmountPerWave[WaveNumber] + NetRecv_UniqueRedAmountPerWave + NetRecv_BossRedAmountPerWave) > 0.9)
        {
            EnemyCount[3].SetActive(true);
            EnemyCountText[3].text = "x " + ((int)RedAmountPerWave[WaveNumber] + NetRecv_UniqueRedAmountPerWave + NetRecv_BossRedAmountPerWave).ToString("0");
        }
        else if (RedAmountPerWave[WaveNumber] <= 0)
        {
            EnemyCount[3].SetActive(false);
        }
    }
    void SpawnNormalGerms()
    {
        ////Green Spawn    
        if (GreenAmountPerWave[WaveNumber] > 0.9f)
        {      
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ))); 
            float tempscale = Random.Range(1.0f, 1.5f);
            EnemyAI Dustmite = Instantiate(Dustmite_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.RedCircleID = EnemyID;
            RedCircleSp.FollowingObject = Dustmite.gameObject;
            Dustmite.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            GreenAmountPerWave[WaveNumber] -= 1;
            Dustmite.EnemyType = 1;
            Dustmite.EnemyID = EnemyID;
            EnemyID++;
            RandomSpawn();
        }
      
        ////Yellow Spawn    
        if (YellowAmountPerWave[WaveNumber] > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            EnemyAI FlyingGerm = Instantiate(FlyingEnemy_prefab, tempVec, FlySpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = FlyingGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            float tempscale = Random.Range(7.0f, 9.0f);
            FlyingGerm.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            YellowAmountPerWave[WaveNumber] -= 1;
            FlyingGerm.EnemyType = 4;
            FlyingGerm.EnemyID = EnemyID;
            EnemyID++;
            RandomSpawn();
        }

        //Blue Spawn
        if (BlueAmountPerWave[WaveNumber] > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            EnemyAI BlueGerm = Instantiate(BlueGerm_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = BlueGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            float tempscale = Random.Range(160.0f, 170.0f);
            BlueGerm.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            BlueAmountPerWave[WaveNumber] -= 1;
            BlueGerm.EnemyType = 7;
            BlueGerm.EnemyID = EnemyID;
            EnemyID++;
            RandomSpawn();
        }
        //Red Spawn       
        if (RedAmountPerWave[WaveNumber] > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            EnemyAI RedGerm = Instantiate(Meatball_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = RedGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            float tempscale = Random.Range(4.0f, 6.0f);
            RedGerm.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedAmountPerWave[WaveNumber] -= 1;
            RedGerm.EnemyType = 10;
            RedGerm.EnemyID = EnemyID;
            EnemyID++;
            RandomSpawn();
        }
    }
    void SpawnUniqueGerms()
    {
        //// Unique Green Spawn    
        if (NetRecv_UniqueGreenAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
           // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI UDustMite = Instantiate(UniqDustmite_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = UDustMite.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_UniqueGreenAmountPerWave -= 1;
            UDustMite.EnemyType = 2;
            UDustMite.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Unique Yellow Spawn    
        if (NetRecv_UniqueYellowAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            EnemyAI UFlyingGerm = Instantiate(UniqFlyingEnemy_prefab, tempVec, FlySpawnerPos[randomspawn].transform.rotation) as EnemyAI;           
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = UFlyingGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_UniqueYellowAmountPerWave -= 1;
            UFlyingGerm.EnemyType = 5;
            UFlyingGerm.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Unique Blue Spawn    
        if (NetRecv_UniqueBlueAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI UBlueGerm = Instantiate(UniqBlueGerm_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = UBlueGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_UniqueBlueAmountPerWave -= 1;
            UBlueGerm.EnemyType = 8;
            UBlueGerm.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Unique Blue Spawn    
        if (NetRecv_UniqueRedAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI URedGerm = Instantiate(UniqMeatball_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = URedGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_UniqueRedAmountPerWave -= 1;
            URedGerm.EnemyType = 11;
            URedGerm.EnemyID = EnemyID;
            EnemyID++;
        }
       
    }
    void SpawnBossGerms()
    {
        //// Bossue Green Spawn    
        if (NetRecv_BossGreenAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI BDustMite = Instantiate(BossDustmite_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);          
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = BDustMite.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_BossGreenAmountPerWave -= 1;
            BDustMite.EnemyType = 3;
            BDustMite.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Bossue Yellow Spawn    
        if (NetRecv_BossYellowAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            EnemyAI BFlyingGerm = Instantiate(BossFlyingEnemy_prefab, tempVec, FlySpawnerPos[randomspawn].transform.rotation) as EnemyAI;           
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = BFlyingGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_BossYellowAmountPerWave -= 1;
            BFlyingGerm.EnemyType = 6;
            BFlyingGerm.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Bossue Blue Spawn    
        if (NetRecv_BossBlueAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI BBlueGerm = Instantiate(BossBlueGerm_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = BBlueGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_BossBlueAmountPerWave -= 1;
            BBlueGerm.EnemyType = 9;
            BBlueGerm.EnemyID = EnemyID;
            EnemyID++;
        }
        //// Bossue Blue Spawn    
        if (NetRecv_BossRedAmountPerWave > 0.9f)
        {
            Vector3 tempVec = new Vector3(SpawnerPos[randomspawn].transform.position.x + (Random.Range(-SpawnRangeX, SpawnRangeX)), SpawnerPos[randomspawn].transform.position.y, SpawnerPos[randomspawn].transform.position.z + (Random.Range(-SpawnRangeZ, SpawnRangeZ)));
            // float tempscale = Random.Range(0.5f, 1.5f);
            EnemyAI BRedGerm = Instantiate(BossMeatball_prefab, tempVec, SpawnerPos[randomspawn].transform.rotation) as EnemyAI;
            //Dustmite_prefab.transform.localScale = new Vector3(tempscale, tempscale, tempscale);
            RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
            RedCircleSp.FollowingObject = BRedGerm.gameObject;
            RedCircleSp.RedCircleID = EnemyID;
            NetRecv_BossRedAmountPerWave -= 1;
            BRedGerm.EnemyType = 12;
            BRedGerm.EnemyID = EnemyID;
            EnemyID++;
        }
    }
    void EnemyNetworkSend()
    {

    }
    void RandomSpawn()
    {
        //randomspawn++;
        //if (randomspawn > 2)
        //{
        //    randomspawn = 0;
        //}
    }
}
