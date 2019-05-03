using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyAI : MonoBehaviour
{
    //Enemy List
    public static Dictionary<int, GameObject> EnemyList = new Dictionary<int, GameObject>();
    public static string DeathList = "";
    public static int DeadAmount;
    public RedCircle RedCirclePrefab;
    //Enemy Path Progression
    public int PathProgress = 0;
    public int PathChoice = 0;
    //Enemy Stats
    private int OrgEnemySpeed;
    public int EnemyHP = 5;
    public int EnemySpeed = 10;
    public int CoinDrop;
    public int MiniBlueAmount;
    public Rigidbody EnemyBody;
    //Targets  
    public Transform Player1;
    public Transform Player2;
    //Sounds
    public AudioSource AttackSound;
    public AudioSource IdleSound;
    public AudioSource DeathSound;
    //AI
    private bool Aggro = false;
    //Coin Spawn
    public GameObject Coin;
    public GameObject DeathParticles;
    //Distance Tracker
    public float DistanceBetweenObjects;
    public Vector3 TargetLookAt;
    private int DisDiff = 20;
    public Vector3 RandomOffset = new Vector3(0, 0, 0);
    public static int Offset;
    //Food Effects
    private bool HoneyEffect = false;
    private bool GarlicEffect = false;
    private Vector3 GarlicLoc;
    private float delay = 15.0f;
    private float delayGE = 3.0f;
    private float nextUsage;
    private float nextUsageGE;
    //Objects
    //public Transform Food;
    public Transform EnemyTransform;


    //Enemy Type
    public int EnemyType;
    public int EnemyID;
    public EnemyAI[] MiniBlue = new EnemyAI[3];
    //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
    // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM

    // Use this for initialization
    void Start()
    {
        EnemyTransform = GetComponent<Transform>();
        DetermineHPSpeed();
        Offset = DisDiff / 2;
        EnemyList.Add(EnemyID, gameObject);
        PathChoice = EnemySpawn.randomspawn;
        OrgEnemySpeed = EnemySpeed;
        Player1 = GameObject.Find("Player 1").transform;
        //  Player2 = GameObject.Find("Player 2").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHP <= 0)
        {
            EnemyDeath();
        }
        Player1 = GameObject.Find("Player 1").transform;
        if (HoneyEffect == true)
        {
            if (Time.time > nextUsage)
            {
                nextUsage = Time.time + delay;
                HoneyEffect = false;
                EnemySpeed = OrgEnemySpeed;
            }
        }
        if (GarlicEffect == true)
        {
            if (Time.time > nextUsageGE)
            {
                nextUsageGE = Time.time + delayGE;
                GarlicEffect = false;
            }
        }

        if (EnemyType >= 13)
        {
            Aggro = true;
        }

        EnemyPath();

    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Bullet")
        {
            Aggro = true;
            EnemyHP = EnemyHP - 1;
            Destroy(collisionInfo.gameObject);
        }

        if (collisionInfo.collider.tag == "Mousetrap" || collisionInfo.collider.tag == "Blackpepper" || collisionInfo.collider.tag == "Ulti")
        {
            EnemyHP = 0;
            if (collisionInfo.collider.tag == "Blackpepper")
            {
                ToolTip.BombKills++;
            }
        }

        if (collisionInfo.collider.tag == "Food")
        {
            if (EnemyType < 13)
            {
                EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
            }
            Fill.isHit = true;
            if (EnemyType < 13)
            {
                Fill.isHit = true;
                Food.PlayerHP = Food.PlayerHP - 1;
                //Debug.Log(Food.PlayerHP);
            }

            Destroy(gameObject);
        }
        if (collisionInfo.collider.tag == "Honeycomb")
        {
            if (HoneyEffect == false)
            {
                nextUsage = Time.time + delay;
                HoneyEffect = true;
                EnemySpeed = 5;
            }
        }
        if (collisionInfo.collider.tag == "GarlicClove")
        {
            if (GarlicEffect == false)
            {
                GarlicLoc = collisionInfo.transform.position;
                nextUsageGE = Time.time + delayGE;
                GarlicEffect = true;
            }
        }
    }
    void EnemyDeath()
    {
        EnemyList.Remove(EnemyID);
        DeadAmount++;
        DeathList += EnemyID + ",";
        //Debug.Log(DeathList);
        DeathSound.Play();
        if (EnemyType < 13)
        {
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
        }
        //Spawns coins upon death 
        for (int i = 0; i < CoinDrop; i++)
        {
            Instantiate(Coin, transform.position, transform.rotation);
        }
        Instantiate(DeathParticles, transform.position, transform.rotation);
        if (EnemyType == 7 || EnemyType == 8 || EnemyType == 9)
        {

            if (EnemyType == 7)
            {
                for (int i = 0; i < MiniBlueAmount; i++)
                {
                    Vector3 tempVec = new Vector3(Random.Range(-20, 20) + transform.position.x, transform.position.y,  transform.position.z);
                    EnemyAI NMiniBlue = Instantiate(MiniBlue[0], tempVec, transform.rotation) as EnemyAI;
                    NMiniBlue.EnemyID = EnemySpawn.EnemyID;
                    NMiniBlue.EnemyType = 13;
                    NMiniBlue.PathProgress = PathProgress;
                    NMiniBlue.PathChoice = PathChoice;
                    RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
                    RedCircleSp.RedCircleID = EnemySpawn.EnemyID;
                    RedCircleSp.FollowingObject = NMiniBlue.gameObject;
                    EnemySpawn.EnemyID++;
                }
            }
            else if (EnemyType == 8)
            {
                for (int i = 0; i < MiniBlueAmount; i++)
                {
                    Vector3 tempVec = new Vector3(Random.Range(-20, 20) + transform.position.x, transform.position.y,  transform.position.z);
                    EnemyAI UMiniBlue = Instantiate(MiniBlue[1], tempVec, transform.rotation) as EnemyAI;
                    UMiniBlue.EnemyID = EnemySpawn.EnemyID;
                    UMiniBlue.EnemyType = 13;
                    UMiniBlue.PathProgress = PathProgress;
                    UMiniBlue.PathChoice = PathChoice;
                    RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
                    RedCircleSp.RedCircleID = EnemySpawn.EnemyID;
                    RedCircleSp.FollowingObject = UMiniBlue.gameObject;
                    EnemySpawn.EnemyID++;
                }
            }
            else if (EnemyType == 9)
            {
                for (int i = 0; i < MiniBlueAmount; i++)
                {
                    Vector3 tempVec = new Vector3(Random.Range(-20, 20) + transform.position.x, transform.position.y, transform.position.z);
                    EnemyAI BMiniBlue = Instantiate(MiniBlue[0], tempVec, transform.rotation) as EnemyAI;
                    BMiniBlue.EnemyID = EnemySpawn.EnemyID;
                    BMiniBlue.EnemyType = 13;
                    BMiniBlue.PathProgress = PathProgress;
                    BMiniBlue.PathChoice = PathChoice;
                    RedCircle RedCircleSp = Instantiate(RedCirclePrefab, transform.position, transform.rotation) as RedCircle;
                    RedCircleSp.RedCircleID = EnemySpawn.EnemyID;
                    RedCircleSp.FollowingObject = BMiniBlue.gameObject;
                    EnemySpawn.EnemyID++;
                }
            }
        }
        ToolTip.EnemyKills++;
        if (ToolTip.ToolTipProgress > 5)
        {
            ToolTip.UltiKills++;
        }
        Destroy(gameObject);

    }

    public void EnemyPath()
    {
        //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
        // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM
        //Movement with Status Effect
        if (GarlicEffect == true)
        {
            transform.LookAt(GarlicLoc);
            TargetLookAt = GarlicLoc;
            DisDiff = 5;
        }
        else if (GarlicEffect == false && Aggro == false)
        {
            if (EnemyType == 4 || EnemyType == 5 || EnemyType == 6)
            {
                if (PathChoice == 0)
                {
                    transform.LookAt(EnemySpawn.PathFlyOneStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathFlyOneStatic[PathProgress].transform.position + RandomOffset;
                }
                else if (PathChoice == 1)
                {
                    transform.LookAt(EnemySpawn.PathFlyTwoStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathFlyTwoStatic[PathProgress].transform.position + RandomOffset;
                }
                else if (PathChoice == 2)
                {
                    transform.LookAt(EnemySpawn.PathFlyThreeStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathFlyThreeStatic[PathProgress].transform.position + RandomOffset;
                }
            }
            else
            {
                if (PathChoice == 0)
                {
                    transform.LookAt(EnemySpawn.PathOneStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathOneStatic[PathProgress].transform.position + RandomOffset;
                }
                else if (PathChoice == 1)
                {
                    transform.LookAt(EnemySpawn.PathTwoStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathTwoStatic[PathProgress].transform.position + RandomOffset;
                }
                else if (PathChoice == 2)
                {
                    transform.LookAt(EnemySpawn.PathThreeStatic[PathProgress].transform.position + RandomOffset);
                    TargetLookAt = EnemySpawn.PathThreeStatic[PathProgress].transform.position + RandomOffset;
                }
            }
        }
        else if (Aggro == true)
        {
            transform.LookAt(Player1);
            TargetLookAt = Player1.transform.position;
            DisDiff = 10;
        }
        if (PathProgress > 4)
        {
            DisDiff = 0;
        }
        if (PathProgress < 4)
        {
            DisDiff = Offset;
        }


        //Enemy tries not to collide with food 

        DistanceBetweenObjects = Vector3.Distance(TargetLookAt, transform.position);
        if (DistanceBetweenObjects > DisDiff)
        {
            transform.position += transform.forward * EnemySpeed * Time.deltaTime;
        }
        else if (DistanceBetweenObjects < DisDiff)
        {

            PathProgress++;
            if (PathProgress > 4)
            {
                PathProgress = 4;
            }
            RandomOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        }
    }

    public void DetermineHPSpeed()
    {
        //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
        // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM

        //Normal Dustmite
        if (EnemyType == 1)
        {
            EnemyHP = 1;
            EnemySpeed = 30;
            CoinDrop = 1;
        }
        //Unique Dustmite
        else if (EnemyType == 2)
        {
            EnemyHP = 3;
            EnemySpeed = 30;
            CoinDrop = 3;
        }
        //Boss Dustmite
        else if (EnemyType == 3)
        {
            EnemyHP = 5;
            EnemySpeed = 35;
            CoinDrop = 5;
        }
        //Normal Flying
        else if (EnemyType == 4)
        {
            EnemyHP = 3;
            EnemySpeed = 25;
            CoinDrop = 3;
        }
        //Unique Flying
        else if (EnemyType == 5)
        {
            EnemyHP = 5;
            EnemySpeed = 30;
            CoinDrop = 3;
        }
        //Boss Flying
        else if (EnemyType == 6)
        {
            EnemyHP = 10;
            EnemySpeed = 25;
            CoinDrop = 10;
        }
        //Normal Blue Germ
        else if (EnemyType == 7)
        {
            MiniBlueAmount = 10;
            EnemyHP = 5;
            EnemySpeed = 30;
            CoinDrop = 4;
        }
        // Unique Blue Germ
        else if (EnemyType == 8)
        {
            MiniBlueAmount = 10;
            EnemyHP = 10;
            EnemySpeed = 30;
            CoinDrop = 4;
        }
        //Boss Blue Germ
        else if (EnemyType == 9)
        {
            MiniBlueAmount = 20;
            EnemyHP = 10;
            EnemySpeed = 30;
            CoinDrop = 4;
        }
        //Normal Meatball
        else if (EnemyType == 10)
        {
            EnemyHP = 15;
            EnemySpeed = 30;
            CoinDrop = 5;
        }
        //Unique Meatball
        else if (EnemyType == 11)
        {
            EnemyHP = 20;
            EnemySpeed = 40;
            CoinDrop = 5;
        }
        //Boss Meatball
        else if (EnemyType == 12)
        {
            EnemyHP = 20;
            EnemySpeed = 45;
            CoinDrop = 5;
        }
        //Normal Mini Blue
        else if (EnemyType == 13)
        {
            EnemyHP = 1;
            EnemySpeed = 30;
            CoinDrop = 0;
        }
        //Unique Mini Blue
        else if (EnemyType == 14)
        {
            EnemyHP = 1;
            EnemySpeed = 30;
            CoinDrop = 0;
        }
        //Boss Mini Blue
        else if (EnemyType == 15)
        {
            EnemyHP = 2;
            EnemySpeed = 30;
            CoinDrop = 0;
        }

    }
}
