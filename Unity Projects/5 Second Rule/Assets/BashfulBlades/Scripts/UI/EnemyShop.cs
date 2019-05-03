using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnemyShop : MonoBehaviour {
    //Enemy Costs
    public int[] UniqueEnemyCost = new int[4];// 1 = Flying
    public int[] BossEnemyCost = new int[4];  // 2 = Blue Germ
                                              // 3 = Meat Ball
    

    //Menu Items
    public Text[] DustMite_t = new Text[3];
    public Text[] Flying_t = new   Text[3];
    public Text[] BlueGerm_t = new Text[3];
    public Text[] MeatBall_t = new Text[3];
    public Button[] DustMite_b = new Button[3];
    public Button[] Flying_b = new   Button[3];
    public Button[] BlueGerm_b = new Button[3];
    public Button[] MeatBall_b = new Button[3];
    //Grid Movement
    public static int EnemyHoriGrid;
    public static int EnemyVertGrid;
    //Grid Movement Once
    public bool[] CD = new bool[7];   
    //Controller Movement
    public int RightX, RightY;
    //ToggleBool
    public static bool EnemyShopToggle = false;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (EnemyShopToggle == true)
        {
            ButtonChecker();
            if(Input.GetKeyDown(KeyCode.W))
            {
                EnemyVertGrid = EnemyVertGrid - 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                EnemyVertGrid = EnemyVertGrid + 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                EnemyHoriGrid = EnemyHoriGrid - 10;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                EnemyHoriGrid = EnemyHoriGrid + 10;
            }
            RightX = ControllerManager.GetLeftJoyStickX();
            RightY = ControllerManager.GetLeftJoyStickY();
            if (RightX > 30000)
            {
                if (CD[0] == true)
                {
                    EnemyHoriGrid = EnemyHoriGrid + 10;
                }
                CD[0] = false;
            }
            else if (RightX < -30000)
            {
                if (CD[0] == true)
                {
                    EnemyHoriGrid = EnemyHoriGrid - 10;
                }
                CD[0] = false;
            }
            if (RightX == 0)
            {
                CD[0] = true;
            }
            //Vertical Movement
            if (RightY > 30000)
            {
                if (CD[1] == true)
                {
                    EnemyVertGrid = EnemyVertGrid - 1;
                }
                CD[1] = false;
            }
            else if (RightY < -30000)
            {
                if (CD[1] == true)
                {
                    EnemyVertGrid = EnemyVertGrid + 1;
                }
                CD[1] = false;
            }
            if (RightY > -1000 && RightY < 1000)
            {
                CD[1] = true;
            }
            //A Button
            if (ControllerManager.GetButtonDown(0) == true || Input.GetKeyDown(KeyCode.Return))
            {
                if (CD[6] == true)
                {
                    PurchaseEnemy();
                }
                CD[6] = false;
            }
            else
            {
                CD[6] = true;
            }
            //Makes sure it does not go out of range
            if (EnemyVertGrid > 3)
            {
                EnemyVertGrid = 3;
            }
            if (EnemyVertGrid < 0)
            {
                EnemyVertGrid = 0;
            }
            if (EnemyHoriGrid > 10)
            {
                EnemyHoriGrid = 10;
            }        
            if (EnemyHoriGrid < 0)
            {       
                EnemyHoriGrid = 0;
            }
        }
    }
    public void PurchaseEnemy()
    {
        // 0 = Dustmite
        // 1 = Flying
        // 2 = Blue Germ
        // 3 = Meat Ball
        //DUSTMITES       
        if (EnemyVertGrid == 0 && EnemyHoriGrid == 0) // Purchase Unique DustMite
        {
            if (Player.Money >= UniqueEnemyCost[0])
            {
                NetworkEnemySpawn.NetSend_UniqueGreenAmountPerWave++;
            }
        }
        if (EnemyVertGrid == 1 && EnemyHoriGrid == 0) // Purchase Boss DustMite
        {
            if (Player.Money >= BossEnemyCost[0])
            {
                NetworkEnemySpawn.NetSend_BossGreenAmountPerWave++;
            }
        }
       
        if (EnemyVertGrid == 2 && EnemyHoriGrid == 0) // Purchase Normal DustMite
        {
            if (Player.Money >= UniqueEnemyCost[1])
            {
                NetworkEnemySpawn.NetSend_UniqueYellowAmountPerWave++;
            }
        }
        if (EnemyVertGrid == 3 && EnemyHoriGrid == 0) // Purchase Normal DustMite
        {
            if (Player.Money >= BossEnemyCost[1])
            {
                NetworkEnemySpawn.NetSend_BossYellowAmountPerWave++;
            }
        }
        
        if (EnemyVertGrid == 0 && EnemyHoriGrid == 10) // Purchase Unique DustMite
        {
            if (Player.Money >= UniqueEnemyCost[2])
            {
                NetworkEnemySpawn.NetSend_UniqueBlueAmountPerWave++;
            }
        }
        if (EnemyVertGrid == 1 && EnemyHoriGrid == 10) // Purchase Boss DustMite
        {
            if (Player.Money >= BossEnemyCost[2])
            {
                NetworkEnemySpawn.NetSend_BossBlueAmountPerWave++;
            }
        }
       
        if (EnemyVertGrid == 2 && EnemyHoriGrid == 10) // Purchase Normal DustMite
        {
            if (Player.Money >= UniqueEnemyCost[3])
            {
                NetworkEnemySpawn.NetSend_UniqueRedAmountPerWave++;
            }
        }
        if (EnemyVertGrid == 3 && EnemyHoriGrid == 10) // Purchase Normal DustMite
        {
            if (Player.Money >= BossEnemyCost[3])
            {
                NetworkEnemySpawn.NetSend_BossRedAmountPerWave++;
            }
        }
    }
    public void ButtonChecker()
    {
        for(int i = 1; i < 3;i++)
        {
            DustMite_t[i].color = Color.black;
            Flying_t[i].color = Color.black;
            BlueGerm_t[i].color = Color.black;
            MeatBall_t[i].color = Color.black;
            DustMite_b[i].image.color = Color.white;
            Flying_b[i].image.color = Color.white;
            BlueGerm_b[i].image.color = Color.white;
            MeatBall_b[i].image.color = Color.white;
        }
        
        if (EnemyVertGrid == 0 && EnemyHoriGrid == 0) // Purchase Unique DustMite
        {
            DustMite_t[1].color = Color.white;
            DustMite_b[1].image.color = Color.black;
        }
        if (EnemyVertGrid == 1 && EnemyHoriGrid == 0) // Purchase Boss DustMite
        {
            DustMite_t[2].color = Color.white;
            DustMite_b[2].image.color = Color.black;
        }
        //FLYERS
        
        if (EnemyVertGrid == 2 && EnemyHoriGrid == 0) // Purchase Normal DustMite
        {
            Flying_t[1].color = Color.white;
            Flying_b[1].image.color = Color.black;
        }
        if (EnemyVertGrid == 3 && EnemyHoriGrid == 0) // Purchase Normal DustMite
        {
            Flying_t[2].color = Color.white;
            Flying_b[2].image.color = Color.black;
        }
        //BLUE GERMS
       
        if (EnemyVertGrid == 0 && EnemyHoriGrid == 10) // Purchase Unique DustMite
        {
            BlueGerm_t[1].color = Color.white;
            BlueGerm_b[1].image.color = Color.black;
        }
        if (EnemyVertGrid == 1 && EnemyHoriGrid == 10) // Purchase Boss DustMite
        {
            BlueGerm_t[2].color = Color.white;
            BlueGerm_b[2].image.color = Color.black;
        }
        //MEATBALL
        
        if (EnemyVertGrid == 2 && EnemyHoriGrid == 10) // Purchase Normal DustMite
        {
            MeatBall_t[1].color = Color.white;
            MeatBall_b[1].image.color = Color.black;
        }
        if (EnemyVertGrid == 3 && EnemyHoriGrid == 10) // Purchase Normal DustMite
        {
            MeatBall_t[2].color = Color.white;
            MeatBall_b[2].image.color = Color.black;
        }
    }
}
