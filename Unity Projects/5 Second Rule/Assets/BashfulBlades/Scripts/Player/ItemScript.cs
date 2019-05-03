using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class ItemScript : MonoBehaviour
{
    //Items
    public Transform ItemSpawn;
    public Transform throwTarget;
    public Transform player;
    public GameObject GarlicClove;
    public GameObject Blackpepperbombg;
    public GameObject Mousetrap;
    public GameObject Honeycomb;
    public static int BPAmount = 0;
    public static int GCAmount = 0;
    public static int HCAmount = 0;
    public static int MTAmount = 0;
    public static int BPCost = 10;
    public static int GCCost = 5;
    public static int HCCost = 5;
    public static int MTCost = 10;
    public static bool bombBeingThrown = false;
    public static bool[] ONCEITEM = new bool[5]{true,true,true,true,true};
    public static bool[] showItem = new bool[3] { false, false, false };

    public float tossAngle = 45;

    void Start()
    {
        BPAmount = 3;
        GCAmount = 3;
        HCAmount = 3;
        MTAmount = 3;
        if (SetLevel.Broco == true)
        {
            GCAmount += 2;
        }
        if (SetLevel.Carrot == true)
        {
            MTAmount += 2;
        }
        if (SetLevel.Hambo == true)
        {
            BPAmount += 2;
        }
        if (SetLevel.Chicken == true)
        {
            HCAmount += 2;
        }
        
    }
    //Pepp = 10
    //Garlic = 5
    //Mouse = 20
    // Honeycomb = 15
    // Update is called once per frame
    void Update()
    {
        if (EnemyShop.EnemyShopToggle == false)
        {
            //Pepper Bomb
            if (ControllerManager.GetButtonDown(4) == true || Input.GetKey(KeyCode.Alpha1) == true)// Y
            {
                if (ONCEITEM[0] == true)
                {
                    bombBeingThrown = true;
                    ONCEITEM[0] = false;
                }
                if (ControllerManager.GetButtonDown(1) == true || Input.GetKeyDown(KeyCode.I))//B
                {
                    bombBeingThrown = false;
                }
            }
            else
            {
                ONCEITEM[0] = true;
                if (bombBeingThrown)
                {
                    GetItemBP(Blackpepperbombg, ItemSpawn);
                    bombBeingThrown = false;
                }
            }

            //MouseTrap
            if (ControllerManager.GetButtonDown(14) == true || Input.GetKeyDown(KeyCode.Alpha4))//A
            {
                if (ONCEITEM[1] == true)
                {
                    showItem[0] = true;
                    ONCEITEM[1] = false;
                }
                if (ControllerManager.GetButtonDown(1) == true || Input.GetKeyDown(KeyCode.I))//B
                {
                    showItem[0] = false;
                }
            }
            else
            {
                ONCEITEM[1] = true;
                if (showItem[0])
                {
                    GetItemMT(Mousetrap, ItemSpawn);
                    ToolTip.TrapSetup[1] = false;
                    ToolTip.TrapSetup[2] = true;
                    ToolTip.TrapSetup[3] = false;
                    showItem[0] = false;
                }
            }

            //Honeycomb
            if (ControllerManager.GetButtonDown(15) == true || Input.GetKeyDown(KeyCode.Alpha3))//X
            {
                if (ONCEITEM[2] == true)
                {
                    showItem[1] = true;
                    ONCEITEM[2] = false;
                }
                if (ControllerManager.GetButtonDown(1) == true || Input.GetKeyDown(KeyCode.I))//B
                {
                    showItem[1] = false;
                }
            }
            else
            {
                ONCEITEM[2] = true;
                if (showItem[1])
                {
                    GetItemHC(Honeycomb, ItemSpawn);
                    ToolTip.TrapSetup[1] = true;
                    ToolTip.TrapSetup[2] = false;
                    ToolTip.TrapSetup[3] = false;
                    showItem[1] = false;
                }
            }

            //Garlic Clove
            if (ControllerManager.GetButtonDown(16) == true || Input.GetKeyDown(KeyCode.Alpha2))//B
            {
                if (ONCEITEM[3] == true)
                {
                    showItem[2] = true;
                    ONCEITEM[3] = false;
                }
                if (ControllerManager.GetButtonDown(1) == true || Input.GetKeyDown(KeyCode.I))//B
                {
                    showItem[2] = false;
                }
            }
            else
            {
                ONCEITEM[3] = true;
                if (showItem[2])
                {
                    GetItemGC(GarlicClove, ItemSpawn);
                    ToolTip.TrapSetup[1] = false;
                    ToolTip.TrapSetup[3] = true;
                    ToolTip.TrapSetup[2] = false;
                    showItem[2] = false;
                }
            }
        }
    }
    public void GetItemBP(GameObject Blackpepperbomb, Transform ItemSpawn)
    {
        
        if (BPAmount > 0)
        {
            BPAmount -= 1;
            GameObject bomb = (GameObject)Instantiate(Blackpepperbomb, player.position, ItemSpawn.rotation);
            bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            bomb.GetComponent<Rigidbody>().velocity = BombThrow.BallisticVel(player, throwTarget, tossAngle);
        }
        else if (BPAmount < 1)
        {
            if (Player.Money > BPCost)
            {
                Player.Money -= BPCost;
                GameObject bomb = (GameObject)Instantiate(Blackpepperbomb, player.position, ItemSpawn.rotation);
                bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                bomb.GetComponent<Rigidbody>().velocity = BombThrow.BallisticVel(player, throwTarget, tossAngle);
            }
            else if (Player.Money < BPCost)
            {

            }
        }
       
    }
    public void GetItemGC(GameObject GarlicClove, Transform ItemSpawn)
    {
        
        if (GCAmount > 0)
        {
            GCAmount -= 1;
            Instantiate(GarlicClove, ItemSpawn.position, ItemSpawn.rotation);
        }
        else if (GCAmount < 1)
        {
            if (Player.Money > GCCost)
            {
                Player.Money -= GCCost;
                Instantiate(GarlicClove, ItemSpawn.position, ItemSpawn.rotation);

            }
            else if (Player.Money < GCCost)
            {

            }
        }
       
    }
    public  void GetItemHC(GameObject Honeycomb, Transform ItemSpawn)
    {
        //if (Fill.canUseItem == false)
        //{
        if (HCAmount > 0)
        {
            HCAmount -= 1;
            Instantiate(Honeycomb, ItemSpawn.position, ItemSpawn.rotation);
        }
        else if (HCAmount < 1)
        {
            if (Player.Money > HCCost)
            {
                Player.Money -= HCCost;
                Instantiate(Honeycomb, ItemSpawn.position, ItemSpawn.rotation);
            }
            else if (Player.Money < HCCost)
            {

            }
        }
        //}
        //else if (Fill.canUseItem == true)
        //{
        //    if (Player.Money >= HCCost)
        //    {
        //        HCAmount += 1;
        //        Player.Money -= HCCost;
        //    }

        //}
    }
    public  void GetItemMT(GameObject Mousetrap, Transform ItemSpawn)
    {
        if (MTAmount > 0)
        {
            MTAmount -= 1;
            Instantiate(Mousetrap, ItemSpawn.position, ItemSpawn.rotation);
        }
        else if (MTAmount < 1)
        {
            if (Player.Money > MTCost -1)
            {
                Player.Money -= MTCost;
                Instantiate(Mousetrap, ItemSpawn.position, ItemSpawn.rotation);
            }
            else if (Player.Money < MTCost)
            {

            }
        }        
    }
}
