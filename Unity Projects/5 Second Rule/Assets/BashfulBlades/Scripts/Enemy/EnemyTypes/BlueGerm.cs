using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueGerm : MonoBehaviour {
    //List
 //  public static List<Transform> BlueGermPos = new List<Transform>();
    public int PathProgress = 0;
    public int PathChoice = 0;
    //Enemy Stats
    private float OrgEnemySpeed;
    public float EnemyHP = 5;
    public float EnemySpeed = 10;
    private bool HoneyEffect = false;
    private bool GarlicEffect = false;
    private Transform GarlicLoc;
    private float delay = 15.0f;
    private float delayGE = 5.0f;
    private float nextUsage;
    private float nextUsageGE;
    //Objects
    //public Transform Food;
    private Transform PlayerGO;
    private Transform GarlicClove;
    public AudioSource AttackSound;   
    public AudioSource DeathSound;
    //Variables for Blue Enemy    
    public GameObject MiniBlue;
    public float miniSpawnC = 10;
    //AI Variables 
    private bool Aggro = false; 
    public GameObject Coin;
    public ParticleSystem DeathParticle;
    public float CoinAmount;
    public float DistanceBetweenObject;
    private Transform TargetLookAt;
    private float DisDiff;
    public Vector3 RanOffset;
    public float Offset = 10;
    //Enemy Type
    public int EnemyType;
    public int EnemyID;
    //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
    // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM
    // Use this for initialization
    void Start () {
        //BlueGermPos.Add(GetComponent<Transform>());
        PathChoice = EnemySpawn.randomspawn; 
        PlayerGO = GameObject.Find("Player 1").transform;
        OrgEnemySpeed = EnemySpeed;    
    }

    // Update is called once per frame
    void Update() {
        
        //Item Effect
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
        //Sound
        //When Enemy Dies
        if (EnemyHP <= 0)
        {
            DeathSound.Play();
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;

            for (int i = 0; i < CoinAmount; i++)
            {
                Instantiate(Coin, transform.position, transform.rotation);
            }
            TargetLookAt = null;
            //Spawn Mini Blue
            for (int i = 0; i < miniSpawnC; i++)
            {
                Vector3 tempVec = new Vector3(Random.Range(-5, 5) + transform.position.x, transform.position.y, (transform.position.z));
                Instantiate(MiniBlue, tempVec, MiniBlue.transform.rotation);
            }
            Instantiate(DeathParticle, transform.position, transform.rotation);
            SaveLoadGame.BlueKills = SaveLoadGame.BlueKills + 1;

            Destroy(gameObject);
        }

    
        
        //
        //Movement with Status Effect
        if (GarlicEffect == true)
        {
            transform.LookAt(GarlicLoc);
            TargetLookAt = GarlicLoc;
            DisDiff = 5;
        }
        else if(Aggro == false && GarlicEffect == false)
        {
            if (PathChoice == 0)
            {
                transform.LookAt(EnemySpawn.PathOneStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathOneStatic[PathProgress];
            }
            else if (PathChoice == 1)
            {
                transform.LookAt(EnemySpawn.PathTwoStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathTwoStatic[PathProgress];
            }
            else if (PathChoice == 2)
            {
                transform.LookAt(EnemySpawn.PathThreeStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathThreeStatic[PathProgress];
            }

            if (PathProgress > 4)
            {
                DisDiff = 0;
            }
            if (PathProgress < 4)
            {
                DisDiff = Offset;
            }



        }
        else if (Aggro == true)
           {
                    transform.LookAt(PlayerGO);
                    TargetLookAt = PlayerGO;
                    DisDiff = 10;
                }
        //Enemy tries not to collide with food 
        
            DistanceBetweenObject = Vector3.Distance(TargetLookAt.position, transform.position);
            if (DistanceBetweenObject > DisDiff)
            {
                transform.position += transform.forward * EnemySpeed * Time.deltaTime;
            }
            else if (DistanceBetweenObject < DisDiff && GarlicEffect == false)
            {
                PathProgress++;
            //RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        }
        
        
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Bullet")
        {
         
            if (Aggro == false)
            {
                AttackSound.Play();
            }
            Aggro = true;
            EnemyHP = EnemyHP - 1;
            Destroy(collisionInfo.gameObject);
        }
        if (collisionInfo.collider.tag == "Honeycomb")
        {
            if (HoneyEffect == false)
            {
                nextUsage = Time.time + delay;
                HoneyEffect = true;
                EnemySpeed = 3;
            }
        }
        if (collisionInfo.collider.tag == "GarlicClove")
        {
            if (GarlicEffect == false)
            {
                GarlicLoc = collisionInfo.transform;
                nextUsageGE = Time.time + delayGE;
                GarlicEffect = true;
            }
        }
        if (collisionInfo.collider.tag == "Food")
        {
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 11;
            Fill.isHit = true;
            Food.PlayerHP = Food.PlayerHP - 1;
            Destroy(gameObject);           
        }
        if (collisionInfo.collider.tag == "Mousetrap" || collisionInfo.collider.tag == "Blackpepper" || collisionInfo.collider.tag == "Ulti")
        {
            EnemyHP = 0;
        }
    }
}
