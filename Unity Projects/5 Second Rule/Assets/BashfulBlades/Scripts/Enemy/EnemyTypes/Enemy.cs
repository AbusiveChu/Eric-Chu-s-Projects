using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
   // public static List<Transform> EnemyList = new List<Transform>();
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
	private Transform PlayerGO;
	private Transform GarlicClove;  
    public AudioSource DeathSound;
    //Variables for Blue Enemy
    public GameObject enemy;
	public GameObject Spawner;
	public float miniSpawnC = 10;	
    public GameObject Coin;
	public ParticleSystem DeathParticle;
	public float CoinAmount;
	private float DistanceBetweenObject;
	private Transform TargetLookAt;
	private float DisDiff;
    //Enemy Type
    public int EnemyType;
    public int EnemyID;
    //This ranges from 1-12// 1-3(DustMite) 4-6 (YellowGerm) 7-9 (Blue Germ) 10-12 (Metaball)
    // First number 1,4,7,10 Stands for Normal Enemy// 2,5,8,11 Stands for Unique Enemy//3,6,9,12 Stands for BAM
    void Start ()
	{
       // EnemyList.Add(GetComponent<Transform>());
        PlayerGO = GameObject.Find("Player 1").transform;
    }
	// Update is called once per frame
	void Update ()
	{		
		//Item Effect
		if (HoneyEffect == true)
		{
			if (Time.time > nextUsage) {
				nextUsage = Time.time + delay;
				HoneyEffect = false;
				EnemySpeed = OrgEnemySpeed;
			}
		}
		if (GarlicEffect == true)
		{
			if (Time.time > nextUsageGE) {
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
				Instantiate (Coin, transform.position, transform.rotation); 
			}
            Instantiate(DeathParticle, transform.position, transform.rotation);
            SaveLoadGame.BlueKills = SaveLoadGame.BlueKills + 1;
            TargetLookAt = null;
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
        else if (GarlicEffect == false)
        {
            
            transform.LookAt(PlayerGO);
            TargetLookAt = PlayerGO;
            DisDiff = 15;
           
            DistanceBetweenObject = Vector3.Distance(TargetLookAt.position, transform.position);
            if (DistanceBetweenObject > DisDiff)
            {
                transform.position += transform.forward * EnemySpeed * Time.deltaTime;
            }
            else if (DistanceBetweenObject < DisDiff && GarlicEffect == false)
            {
                PathProgress++;
            }            
        }
    }
	void OnCollisionEnter (Collision collisionInfo)
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
       
        if (collisionInfo.collider.tag == "Mousetrap" || collisionInfo.collider.tag == "Blackpepper" || collisionInfo.collider.tag == "Ulti")
        {
            EnemyHP = 0;
        }
    }
}




















