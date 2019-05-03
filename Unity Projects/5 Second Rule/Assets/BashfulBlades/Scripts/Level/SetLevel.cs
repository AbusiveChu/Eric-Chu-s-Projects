using UnityEngine;
using System.Collections;

public class SetLevel : MonoBehaviour
{
    //Character Info = false
    public static bool Broco = false;
    public static bool Hambo = false;
    public static bool Carrot = false;
    public static bool Chicken = false;
    public static int CharSelect = 0;
    private bool once;
    private bool twice = true;
    private bool toggler = true;
    public GameObject CarrotGO;
    public GameObject HamboGO;
    public GameObject BrocoGO;
    public GameObject ChickenGO;
    public GameObject[] Player1FoodBase;
    public GameObject[] Player2FoodBase;

    public TextMesh WarningText;
    public TextMesh DangerText;
    public GameObject[] WarningPat;
    public GameObject[] DangerPat;

    public static bool SinglePlayer = true;
    public static int PlayerNumber = 1;
    // Use this for initialization
    void Awake()
    {
        //Carrot
        CarrotGO.SetActive(false);
        Player1FoodBase[2].SetActive(false);
        //Ham
        HamboGO.SetActive(false);
        Player1FoodBase[1].SetActive(false);
        //Broccoli
        BrocoGO.SetActive(false);
        Player1FoodBase[0].SetActive(false);
        //Chicken
        ChickenGO.SetActive(false);
        Player1FoodBase[3].SetActive(false);
        //Player 2
        Player2FoodBase[3].SetActive(false);
        //Carrot       
        Player2FoodBase[2].SetActive(false);
        //Ham       
        Player2FoodBase[1].SetActive(false);
        //Broccoli       
        Player2FoodBase[0].SetActive(false);
        //Chicken        
        Player2FoodBase[3].SetActive(false);

        if (Hambo == false && Carrot == false && Broco == false && Chicken == false)
        {
            SinglePlayer = true;
          Hambo = true;
            EnemySpawn.LevelRound = 666;
            PlayerNumber = 1;
        }

        if (Broco == true)
        {
            CharSelect = 1;
            BrocoGO.SetActive(true);
            Player1FoodBase[0].SetActive(true);
        }
        else if (Hambo == true)
        {
            CharSelect = 2;
            HamboGO.SetActive(true);
            Player1FoodBase[1].SetActive(true);
        }
        else if (Chicken == true)
        {
            CharSelect = 4;
            ChickenGO.SetActive(true);
            Player1FoodBase[3].SetActive(true);
        }

        else if (Carrot == true)
        {
            CharSelect = 3;
            CarrotGO.SetActive(true);
            Player1FoodBase[2].SetActive(true);
        }
       
    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift) || ControllerManager.GetButtonDown(2) == true && once == true)
        //{
        //    toggler = !toggler;
        //    once = false;
        //}
        //else if (ControllerManager.GetButtonDown(2) == false)
        //{
        //    once = true;
        //}


        //if (EnemySpawn.CheckPoint == false)
        //{
        //    WarningText.text = "";
        //    DangerText.text = "";
        //    if (twice == true)
        //    {
        //        toggler = false;
        //        twice = false;
        //    }
        //}       
        //else if (EnemySpawn.CheckPoint == true)
        //{
        //    twice = true;
        //    WarningText.text = "Warning Zone";
        //    DangerText.text = "Danger Zone";
        //    toggler = true;
        //}
        //WarningPat[0].SetActive(toggler);
        //DangerPat[0].SetActive(toggler);
        //WarningPat[1].SetActive(toggler);
        //DangerPat[1].SetActive(toggler);
        //WarningPat[2].SetActive(toggler);
        //DangerPat[2].SetActive(toggler);
        if (SetLevel.SinglePlayer == false)
        {
            if (PlayerTwo.PlayerTwoChar == 1)
            {
                Player2FoodBase[0].SetActive(true);
            }
            else if (PlayerTwo.PlayerTwoChar == 2)
            {
                Player2FoodBase[1].SetActive(true);
            }
            else if (PlayerTwo.PlayerTwoChar == 2)
            {
                Player2FoodBase[2].SetActive(true);
            }
            else if (PlayerTwo.PlayerTwoChar == 2)
            {
                Player2FoodBase[3].SetActive(true);
            }
        }

    }
}