using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemy : MonoBehaviour
{
    //List
  //  public static List<Transform> FlyingEnemyPos = new List<Transform>();
    public int PathProgress = 0;
    public int PathChoice = 0;
    //Enemy Stats
    //private float OrgEnemySpeed;
    public float EnemyHP = 5;
    public float EnemySpeed = 10;
    //private bool HoneyEffect = false;
    //private bool GarlicEffect = false;
    //private Transform GarlicLoc;
    //private float delay = 15.0f;
    //private float delayGE = 3.0f;
    //private float nextUsage;
    //private float nextUsageGE;
    //Objects
    public ParticleSystem DeathParticle;
    private Transform PlayerGO;
    private Transform GarlicClove;
    public AudioSource AttackSound;
    public AudioSource IdleSound;
    public AudioSource DeathSound;
    //AI Variables 
    private bool Aggro = false;
    public GameObject Coin;
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
    void Start()
    {
       // FlyingEnemyPos.Add(GetComponent<Transform>());
        PlayerGO = GameObject.Find("Player 1").transform;
       // OrgEnemySpeed = EnemySpeed;        
        PathChoice = EnemySpawn.randomspawn;
        //RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        //OrgEnemySpeed = EnemySpeed;




    }
    // Update is called once per frame
    void Update()
    {
      //  IdleSound.Play();
        if (EnemyHP <= 0)
        {
            DeathSound.Play();
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;

            for (int i = 0; i < CoinAmount; i++)
            {
                Instantiate(Coin, transform.position, transform.rotation);
            }
            // Audio.Play();			 

            TargetLookAt = null;
            SaveLoadGame.YellowKills = SaveLoadGame.YellowKills + 1;
            Instantiate(DeathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        //
        //Movement with Status Effect        
        if (Aggro == false)
        {

            if (PathChoice == 0)
            {
                transform.LookAt(EnemySpawn.PathFlyOneStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathFlyOneStatic[PathProgress];
            }
            else if (PathChoice == 1)
            {
                transform.LookAt(EnemySpawn.PathFlyTwoStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathFlyTwoStatic[PathProgress];
            }
            else if (PathChoice == 2)
            {
                transform.LookAt(EnemySpawn.PathFlyThreeStatic[PathProgress].transform.position + RanOffset);
                TargetLookAt = EnemySpawn.PathFlyThreeStatic[PathProgress];
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
            DisDiff = 3;
        }
       

        //Enemy tries not to collide with food 

        DistanceBetweenObject = Vector3.Distance(TargetLookAt.position, transform.position);
        if (DistanceBetweenObject > DisDiff)
        {
            transform.position += transform.forward * EnemySpeed * Time.deltaTime;
        }
        else if (DistanceBetweenObject < DisDiff)
        {
            PathProgress++;
            RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        }   
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Bullet")
        {
            
            Aggro = true;
          
            EnemyHP = EnemyHP - 1;
            Destroy(collisionInfo.gameObject);
        }
        
        if (collisionInfo.collider.tag == "Food")
        {
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
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


