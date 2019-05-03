using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
public class Fill : MonoBehaviour {

    [DllImport("SLD")]
    public static extern void SaveGameBin(int Coins, int GK, int YK, int BK, int RK, int TotalKills, int MeatPlay, int VeggiePlay, int Cleartime, int tut);

    public static bool isHit = false;
    public static bool canUseItem = false;
    public Image[] healthFillImage;
    public GameObject[] Icons;
    public Image timeFillImage;
    public GameObject Singleplayer;
    public GameObject Multiplayer;
    public Camera MiniMapTeam1;
    public GameObject MiniMapTeam2;
    private float health;
    private float fullHealth;
    public static float timer = 0;
    public float FullTime = 300; // 5 mins in seconds
	public Text Currency;
    public GameObject CleanUp;
    public GameObject Checkpoint;
    public GameObject PathArrows;
   // public GameObject BuyItemsNow;
	// Start Stuff
    void Awake()
    {
        if (SetLevel.SinglePlayer == true)
        {
            Singleplayer.SetActive(true);
            Multiplayer.SetActive(false);
            MiniMapTeam1.rect = new Rect(0.75f,0.71f,1,1);
            MiniMapTeam2.SetActive(false);
                
        }
        if (SetLevel.SinglePlayer == false)
        {
            Singleplayer.SetActive(false);
            Multiplayer.SetActive(true);
        }
    }
	void Start()
	{
		health = Food.PlayerHP;
		fullHealth = 100;
        timer = 0;
        Icons[0].SetActive(false);
        Icons[1].SetActive(false);
        Icons[2].SetActive(false);
        Icons[3].SetActive(false);
        if (SetLevel.Hambo == true)
        {
            Icons[0].SetActive(true); 
        }
        if (SetLevel.Broco == true)
        {
            Icons[1].SetActive(true);            
        }
        if (SetLevel.Carrot == true)
        {            
            Icons[2].SetActive(true);
        }
        if (SetLevel.Chicken == true)
        {
            Icons[3].SetActive(true);
        }
    }
	// Update is called once per frame
	void Update () 
    {      
		Currency.text = Player.Money.ToString();
        health = Food.PlayerHP;
        //Health Bar
        if(isHit)
        {
           
           health -= 1;
            if(health < 0)
            {
                health = 0;
            }
            
            isHit = false;
        }
        if (SetLevel.Hambo == true)
        {
            healthFillImage[0].fillAmount = health / (float)fullHealth;
        }
        else if (SetLevel.Broco == true)
        {
            healthFillImage[1].fillAmount = health / (float)fullHealth;
        }
        else if (SetLevel.Carrot == true)
        {
            healthFillImage[2].fillAmount = health / (float)fullHealth;
        }
        else if (SetLevel.Chicken == true)
        {
            healthFillImage[3].fillAmount = health / (float)fullHealth;
        }
       
       
        //Timer and checkpoint bar
        if (EnemySpawn.CleanUpTime == false && EnemySpawn.CheckPoint == false)
        {
            if (EnemySpawn.WaveNumber != 0)
            {
                CleanUp.SetActive(false);
                timer += Time.deltaTime;
                timeFillImage.fillAmount = timer / FullTime;
                if (timer > FullTime)
                {
                    timer = 0;
                  // Application.LoadLevel(2);
                 //   SaveLoadGame.RoundsCleared += 1;
                   // SaveGameBin(SaveLoadGame.Coins,SaveLoadGame.GreenKills, SaveLoadGame.YellowKills, SaveLoadGame.BlueKills, SaveLoadGame.RedKills, SaveLoadGame.TotalKills, SaveLoadGame.MeatPlay, SaveLoadGame.VeggiePlay, SaveLoadGame.RoundsCleared, SaveLoadGame.tuton);
                }               
            }
        }
        if (EnemySpawn.WaveNumber > 50 && SetLevel.SinglePlayer == true)
        {
            BoidMovement._boids.Clear();
            SceneManager.LoadScene(2);
        }
       if(EnemySpawn.CleanUpTime == true)
        {
            //EnemySpawn.CheckPoint = false;
            CleanUp.SetActive(true);
        }
        if(EnemySpawn.CheckPoint == false)
        {

            EnemySpawn.CheckPoint = false;
            Checkpoint.SetActive(false);
            //BuyItemsNow.SetActive(false);
            canUseItem = false;
        }
        if(EnemySpawn.CheckPoint == true)
        {
            PathArrows.SetActive(true);
            Checkpoint.SetActive(true);
           // BuyItemsNow.SetActive(true);
            CleanUp.SetActive(false);
            EnemySpawn.CleanUpTime = false;
            canUseItem = true;
        }
	
	}
}
