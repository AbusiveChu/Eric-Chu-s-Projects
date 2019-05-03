using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DustMiteEnemy : MonoBehaviour
{
    //List
  
  //  public static Transform[] DustMitePos = new Transform[120];
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
    private float delayGE = 3.0f;
    private float nextUsage;
    private float nextUsageGE;
    //Objects
    //public Transform Food;

    private Transform GarlicClove;
    public AudioSource AttackSound;
    public AudioSource IdleSound;
    public AudioSource DeathSound;

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
    void Start()
    {
        
        PathChoice = EnemySpawn.randomspawn;
        //RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        OrgEnemySpeed = EnemySpeed;
    }
    // Update is called once per frame
    void Update()
    {
       // DustMitePos[EnemyID] = GetComponent<Transform>();
        //GarlicClove = GameObject.FindGameObjectWithTag ("GarlicClove").transform;
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
            ToolTip.EnemyKills++;
            if(ToolTip.ToolTipProgress > 5)
            {
                ToolTip.UltiKills++;
            }
            for (int i = 0; i < CoinAmount; i++)
            {
                Instantiate(Coin, transform.position, transform.rotation);
            }
            // Audio.Play();			 
            Instantiate(DeathParticle, transform.position, transform.rotation);
            SaveLoadGame.GreenKills = SaveLoadGame.GreenKills + 1;
         //   DustMitePos[EnemyID].position = new Vector3(0,0,0);
            Destroy(gameObject);
            TargetLookAt = null;
        }
        //
        //Movement with Status Effect
        if (GarlicEffect == true)
        {
            transform.LookAt(GarlicLoc);
            TargetLookAt = GarlicLoc;
            DisDiff = 5;
        }
        else if (GarlicEffect == false)
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

        //Enemy tries not to collide with food 

        DistanceBetweenObject = Vector3.Distance(TargetLookAt.position + RanOffset, transform.position);
        if (DistanceBetweenObject > DisDiff)
        {
            transform.position += transform.forward * EnemySpeed * Time.deltaTime;
        }
        else if (DistanceBetweenObject < DisDiff && GarlicEffect == false)
        {
            PathProgress++;
            RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);
        }

    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Bullet")
        {
            EnemyHP = EnemyHP - 1;
            Destroy(collisionInfo.gameObject);          
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
                GarlicLoc = collisionInfo.transform;
                nextUsageGE = Time.time + delayGE;
                GarlicEffect = true;
            }
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
            if (collisionInfo.collider.tag == "Blackpepper")
            {
                ToolTip.BombKills++;
            }
            
        }       
    }
}
