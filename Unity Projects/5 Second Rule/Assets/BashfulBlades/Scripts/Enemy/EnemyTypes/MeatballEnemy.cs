using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeatballEnemy : MonoBehaviour
{
    //List
    //public static List<Transform> MeatballPos = new List<Transform>();
    public int PathProgress = 0;
    public int PathChoice = 0;
    //Enemy Stats
    
    public float EnemyHP = 5;
    public float EnemySpeed = 10;
 
    //Objects
    //public Transform Food;
    private Transform PlayerGO;
    private Transform GarlicClove;
    //Sounds    
    public AudioSource DeathSound;
    //AI Variables 
    private bool Aggro = false;

    public GameObject Coin;
    public ParticleSystem DeathParticle;
    public float CoinAmount;
    public float DistanceBetweenObject;
    private Transform TargetLookAt;
    private float DisDiff;
    public Vector3 RanOffset = new Vector3(0,0,0);
    public float Offset = 10;
    //Enemy Type
    public int EnemyType;
    public int EnemyID;
    //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
    // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM
    void Start()
    {
       // MeatballPos.Add(GetComponent<Transform>());
            PathChoice = EnemySpawn.randomspawn;
       
        PlayerGO = GameObject.Find("Player 1").transform;

       // RanOffset = new Vector3(Random.Range(-Offset, Offset), 0, 0);

    }
    // Update is called once per frame
    void Update()
    {

        //Sound
        //When Enemy Dies
        if (EnemyHP <= 0)
        {
            DeathSound.Play();
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
            //Spawns coins upon death 
            for (int i = 0; i < CoinAmount; i++)
            {
                Instantiate(Coin, transform.position, transform.rotation);
            }
            Instantiate(DeathParticle, transform.position, transform.rotation);
            SaveLoadGame.RedKills = SaveLoadGame.RedKills + 1;
            // MeatballPos.Remove(GetComponent<Transform>());
            TargetLookAt = null;
            Destroy(gameObject);
        }

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
        if (Aggro == true)
        {
            transform.LookAt(PlayerGO);
            TargetLookAt = PlayerGO;
            DisDiff = 1;
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

        ////Enemy tries to collide with player when aggroed 

        //    transform.position += transform.forward * EnemySpeed * Time.deltaTime;


    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Bullet")
        {
            Aggro = true;
           // AttackSound.Play();
            EnemyHP = EnemyHP - 1;
            Destroy(collisionInfo.gameObject);
        }

        if (collisionInfo.collider.tag == "Mousetrap" || collisionInfo.collider.tag == "Blackpepper" || collisionInfo.collider.tag == "Ulti")
        {
            EnemyHP = 0;
        }
        if (collisionInfo.collider.tag == "Food")
        {
            EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
            Fill.isHit = true;
            Food.PlayerHP = Food.PlayerHP - 1;
            Destroy(gameObject);
        }

    }
}

