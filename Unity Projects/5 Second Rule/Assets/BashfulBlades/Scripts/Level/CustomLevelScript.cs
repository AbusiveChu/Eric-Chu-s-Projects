using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomLevelScript : MonoBehaviour
{

    //Wave Title Boxes
    public Text[] WaveTitleBox = new Text[5];
    //Wave One Box Info
    public Text[] WaveOneCount = new Text[5];
    //Wave Two Box Info
    public Text[] WaveTwoCount = new Text[5];
    //Wave Three Box Info
    public Text[] WaveThreeCount = new Text[5];
    //Wave Four Box Info
    public Text[] WaveFourCount = new Text[5];
    //Wave Five Box Info
    public Text[] WaveFiveCount = new Text[5];
    //Enemy Image Boxes
    public GameObject[] EnemyImage = new GameObject[5];
    //Level Image Boxes
    public GameObject[] LevelImage = new GameObject[5];
    //Custom Level Image Boxes
    public Button[] WaveButtons = new Button[20];

    //int 10,20,30,40,50
    public static int HorizontalGrid;
    //int 1,2,3,4
    public static int VerticalGrid;
    public int x, y;


    //Layout works like a list forcing player to go through wave 1 germ 1-4 then wave 2 germ 1-4 and the end they pick the Level Layout

    //Germ 1 Max (Basic Unit) 120
    public int[] GermOneAmount = new int[5];
    public int GermOneMax;
    //Germ 2 Max (Basic Unit) 30
    public int[] GermTwoAmount = new int[5];
    public int GermTwoMax;
    //Germ 3 Max (Basic Unit) 30
    public int[] GermThreeAmount = new int[5];
    public int GermThreeMax;
    //Germ 4 Max (Basic Unit) 30
    public int[] GermFourAmount = new int[5];
    public int GermFourMax;

    //FALSE = MINUS // TRUE = ADD
    public bool ToggleAddMinus = false;
    private int IncDecAmount;
    //Controller Movement
    public int LeftX,LeftY;
    //Cooldown for Stuff
    public static bool[] CD = new bool[7];   
    //Colors for Buttons and there states
    public Color DefaultTextColor = Color.grey;
    public Color PressedTextColor = Color.white;
    public Color DefaultButtonColor = Color.grey;
    public Color PressedButtonColor = Color.black;


    //Controlls DPAD UP(13) DOWN(14) LEFT(15) RIGHT(16)

    // Use this for initialization
    void Start()
    {
     //   InputManager.SetHookTarget(InputManager.GetActiveWindow());
        ControllerManager.Initialize(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || ControllerManager.GetButtonDown(1) == true && Time.time > SelectVert.NextA)//B
        {
            SceneManager.LoadScene(6);
            SelectVert.NextA = Time.time + SelectVert.delayA;
        }
        //Keep Track of Grid (Debugging)
        x = HorizontalGrid;
        y = VerticalGrid;
        //Grab Controller Input
        LeftX = ControllerManager.GetLeftJoyStickX();
        LeftY = ControllerManager.GetLeftJoyStickY();
        //UpdateTextboxes
        TextUpdate();
        //Horizontal Movement
        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            if (CD[0] == true)
            {
                HorizontalGrid = HorizontalGrid + 10;
            }
            CD[0] = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            if (CD[0] == true)
            {
                HorizontalGrid = HorizontalGrid - 10;
            }
            CD[0] = false;
        }
        if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        {
            CD[0] = true;
        }
        //Vertical Movement
        if (LeftY > ControllerMaxMIN.Left_ControllerMaxY)
        {
            if (CD[1] == true)
            {
                VerticalGrid = VerticalGrid - 1;
            }
            CD[1] = false;
        }
        else if (LeftY < ControllerMaxMIN.Left_ControllerMinY)
        {
            if (CD[1] == true)
            {
                VerticalGrid = VerticalGrid + 1;
            }
            CD[1] = false;
        }
        if (LeftY < ControllerMaxMIN.Left_ControllerBaseY && LeftY > -ControllerMaxMIN.Left_ControllerBaseY)
        {
            CD[1] = true;
        }

        //FALSE = MINUS // TRUE = ADD
        //L1 AND R1
        if (ControllerManager.GetButtonDown(5) == true || Input.GetKeyDown(KeyCode.A))
        {
            if (CD[2] == true)
            {
                IncDecAmount = 1;
                ToggleAddMinus = false;
                WaveUpdate();                  
            }
            CD[2] = false;
        }
        else
        {
            CD[2] = true;
        }
        //FALSE = MINUS // TRUE = ADD
        //RIGHT-DPAD / D
        if (ControllerManager.GetButtonDown(7) == true || Input.GetKeyDown(KeyCode.D))
        {
            if (CD[3] == true)
            {
                IncDecAmount = 1;
                ToggleAddMinus = true;
                WaveUpdate();               
            }
            CD[3] = false;
        }
       else
        {
           CD[3] = true;
        }
        //FALSE = MINUS // TRUE = ADD
        //L1 AND R1
        if (ControllerManager.GetButtonDown(4) == true || Input.GetKeyDown(KeyCode.Q))
        {
            if (CD[4] == true)
            {
                IncDecAmount = 5;
                ToggleAddMinus = false;
                WaveUpdate();
            }
            CD[4] = false;
        }
        else
        {
            CD[4] = true;
        }
        //FALSE = MINUS // TRUE = ADD
        //RIGHT-DPAD / D
        if (ControllerManager.GetButtonDown(6) == true || Input.GetKeyDown(KeyCode.E))
        {
            if (CD[5] == true)
            {
                IncDecAmount = 5;
                ToggleAddMinus = true;
                WaveUpdate();
            }
            CD[5] = false;
        }
        else
        {
            CD[5] = true;
        }
        //A Button
        if (ControllerManager.GetButtonDown(0) == true || Input.GetKeyDown(KeyCode.Return))
        {
            if (CD[6] == true)
            {
                EnemySpawn.saveEnemyRound(9001, GermOneAmount[0], GermTwoAmount[0], GermThreeAmount[0], GermFourAmount[0],
                                                GermOneAmount[1], GermTwoAmount[1], GermThreeAmount[1], GermFourAmount[1],
                                                GermOneAmount[2], GermTwoAmount[2], GermThreeAmount[2], GermFourAmount[2],
                                                GermOneAmount[3], GermTwoAmount[3], GermThreeAmount[3], GermFourAmount[3],
                                                GermOneAmount[4], GermTwoAmount[4], GermThreeAmount[4], GermFourAmount[4]);
                EnemySpawn.LevelRound = 9001;
                SceneManager.LoadScene(5);
            }
            CD[6] = false;
        }
        else
        {
            CD[6] = true;
        }
        //Y Button
        if (ControllerManager.GetButtonDown(3) == true || Input.GetKeyDown(KeyCode.Y))
        {
            if (CD[6] == true)
            {
                IncDecAmount = 120;
                ToggleAddMinus = true;
                WaveUpdate();
            }
            CD[6] = false;
        }
        else
        {
            CD[6] = true;
        }
        //Makes sure it does not go out of range
        if (VerticalGrid > 4)
        {
            VerticalGrid = 4;
        }
        if (VerticalGrid < 0)
        {
            VerticalGrid = 0;
        }
        if (HorizontalGrid > 40)
        {  
           HorizontalGrid = 40;
        }  
        if (HorizontalGrid < 0)
        {  
           HorizontalGrid = 0;
        }
        WaveColorCheck();
        WaveAmountCheck();
    }

    void TextUpdate()
    {
        //Wave One Germ Amount
        WaveOneCount[0].text = GermOneAmount[0].ToString() + "/" + GermOneMax.ToString();      
        WaveOneCount[1].text = GermTwoAmount[0].ToString() + "/" + GermTwoMax.ToString();
        WaveOneCount[2].text = GermThreeAmount[0].ToString() + "/" + GermThreeMax.ToString();
        WaveOneCount[3].text = GermFourAmount[0].ToString() + "/" + GermFourMax.ToString();
        //Wave One Germ Amount
        WaveTwoCount[0].text = GermOneAmount[1].ToString() + "/" + GermOneMax.ToString();
        WaveTwoCount[1].text = GermTwoAmount[1].ToString() + "/" + GermTwoMax.ToString();
        WaveTwoCount[2].text = GermThreeAmount[1].ToString() + "/" + GermThreeMax.ToString();
        WaveTwoCount[3].text = GermFourAmount[1].ToString() + "/" + GermFourMax.ToString();
        //Wave One Germ Amount
        WaveThreeCount[0].text = GermOneAmount[2].ToString() + "/" + GermOneMax.ToString();
        WaveThreeCount[1].text = GermTwoAmount[2].ToString() + "/" + GermTwoMax.ToString();
        WaveThreeCount[2].text = GermThreeAmount[2].ToString() + "/" + GermThreeMax.ToString();
        WaveThreeCount[3].text = GermFourAmount[2].ToString() + "/" + GermFourMax.ToString();
        //Wave One Germ Amount
        WaveFourCount[0].text = GermOneAmount[3].ToString() + "/" + GermOneMax.ToString();
        WaveFourCount[1].text = GermTwoAmount[3].ToString() + "/" + GermTwoMax.ToString();
        WaveFourCount[2].text = GermThreeAmount[3].ToString() + "/" + GermThreeMax.ToString();
        WaveFourCount[3].text = GermFourAmount[3].ToString() + "/" + GermFourMax.ToString();
        //Wave One Germ Amount
        WaveFiveCount[0].text = GermOneAmount[4].ToString() + "/" + GermOneMax.ToString();
        WaveFiveCount[1].text = GermTwoAmount[4].ToString() + "/" + GermTwoMax.ToString();
        WaveFiveCount[2].text = GermThreeAmount[4].ToString() + "/" + GermThreeMax.ToString();
        WaveFiveCount[3].text = GermFourAmount[4].ToString() + "/" + GermFourMax.ToString();
    }
    void WaveUpdate()
    {
        //WAVE ONE
        if(VerticalGrid == 0 && HorizontalGrid ==0)
        {
            if(ToggleAddMinus == false)
            {
                GermOneAmount[0] = GermOneAmount[0] - IncDecAmount;
            }
            else
            {
                GermOneAmount[0] = GermOneAmount[0] + IncDecAmount;
            }
        }
        if (VerticalGrid == 1 && HorizontalGrid == 0)
        {
            if (ToggleAddMinus == false)
            {
                GermTwoAmount[0] = GermTwoAmount[0] - IncDecAmount;
            }
            else
            {
                GermTwoAmount[0] = GermTwoAmount[0] + IncDecAmount;
            }
        }
        if (VerticalGrid == 2 && HorizontalGrid == 0)
        {
            if (ToggleAddMinus == false)
            {
                GermThreeAmount[0] = GermThreeAmount[0] - IncDecAmount;
            }
            else
            {
                GermThreeAmount[0] = GermThreeAmount[0] + IncDecAmount;
            }
        }
        if (VerticalGrid == 3 && HorizontalGrid == 0)
        {
            if (ToggleAddMinus == false)
            {
                GermFourAmount[0] = GermFourAmount[0] - IncDecAmount;
            }
            else
            {
                GermFourAmount[0] = GermFourAmount[0] + IncDecAmount;
            }
        }
        //WAVE TWO
        if (VerticalGrid == 0 && HorizontalGrid == 10)
        {
            if (ToggleAddMinus == false)
            {
                GermOneAmount[1] = GermOneAmount[1] - IncDecAmount;
            }
            else
            {
                GermOneAmount[1] = GermOneAmount[1] + IncDecAmount;
            }
        }
        if (VerticalGrid == 1 && HorizontalGrid == 10)
        {
            if (ToggleAddMinus == false)
            {
                GermTwoAmount[1] = GermTwoAmount[1] - IncDecAmount;
            }
            else
            {
                GermTwoAmount[1] = GermTwoAmount[1] + IncDecAmount;
            }
        }
        if (VerticalGrid == 2 && HorizontalGrid == 10)
        {
            if (ToggleAddMinus == false)
            {
                GermThreeAmount[1] = GermThreeAmount[1] - IncDecAmount;
            }
            else
            {
                GermThreeAmount[1] = GermThreeAmount[1] + IncDecAmount;
            }
        }
        if (VerticalGrid == 3 && HorizontalGrid == 10)
        {
            if (ToggleAddMinus == false)
            {
                GermFourAmount[1] = GermFourAmount[1] - IncDecAmount;
            }
            else
            {
                GermFourAmount[1] = GermFourAmount[1] + IncDecAmount;
            }
        }
        //WAVE THREE
        if (VerticalGrid == 0 && HorizontalGrid == 20)
        {
            if (ToggleAddMinus == false)
            {
                GermOneAmount[2] = GermOneAmount[2] - IncDecAmount;
            }
            else
            {
                GermOneAmount[2] = GermOneAmount[2] + IncDecAmount;
            }
        }
        if (VerticalGrid == 1 && HorizontalGrid == 20)
        {
            if (ToggleAddMinus == false)
            {
                GermTwoAmount[2] = GermTwoAmount[2] - IncDecAmount;
            }
            else
            {
                GermTwoAmount[2] = GermTwoAmount[2] + IncDecAmount;
            }
        }
        if (VerticalGrid == 2 && HorizontalGrid == 20)
        {
            if (ToggleAddMinus == false)
            {
                GermThreeAmount[2] = GermThreeAmount[2] - IncDecAmount;
            }
            else
            {
                GermThreeAmount[2] = GermThreeAmount[2] + IncDecAmount;
            }
        }
        if (VerticalGrid == 3 && HorizontalGrid == 20)
        {
            if (ToggleAddMinus == false)
            {
                GermFourAmount[2] = GermFourAmount[2] - IncDecAmount;
            }
            else
            {
                GermFourAmount[2] = GermFourAmount[2] + IncDecAmount;
            }
        }
        //WAVE FOUR
        if (VerticalGrid == 0 && HorizontalGrid == 30)
        {
            if (ToggleAddMinus == false)
            {
                GermOneAmount[3] = GermOneAmount[3] - IncDecAmount;
            }
            else
            {
                GermOneAmount[3] = GermOneAmount[3] + IncDecAmount;
            }
        }
        if (VerticalGrid == 1 && HorizontalGrid == 30)
        {
            if (ToggleAddMinus == false)
            {
                GermTwoAmount[3] = GermTwoAmount[3] - IncDecAmount;
            }
            else
            {
                GermTwoAmount[3] = GermTwoAmount[3] + IncDecAmount;
            }
        }
        if (VerticalGrid == 2 && HorizontalGrid == 30)
        {
            if (ToggleAddMinus == false)
            {
                GermThreeAmount[3] = GermThreeAmount[3] - IncDecAmount;
            }
            else
            {
                GermThreeAmount[3] = GermThreeAmount[3] + IncDecAmount;
            }
        }
        if (VerticalGrid == 3 && HorizontalGrid == 30)
        {
            if (ToggleAddMinus == false)
            {
                GermFourAmount[3] = GermFourAmount[3] - IncDecAmount; 
            }
            else
            {
                GermFourAmount[3] = GermFourAmount[3] + IncDecAmount;
            }
        }
        //WAVE FIVE
        if (VerticalGrid == 0 && HorizontalGrid == 40)
        {
            if (ToggleAddMinus == false)
            {
                GermOneAmount[4] = GermOneAmount[4] - IncDecAmount;
            }
            else
            {
                GermOneAmount[4] = GermOneAmount[4] + IncDecAmount;
            }
        }
        if (VerticalGrid == 1 && HorizontalGrid == 40)
        {
            if (ToggleAddMinus == false)
            {
                GermTwoAmount[4] = GermTwoAmount[4] - IncDecAmount;
            }
            else
            {
                GermTwoAmount[4] = GermTwoAmount[4] + IncDecAmount;
            }
        }
        if (VerticalGrid == 2 && HorizontalGrid == 40)
        {
            if (ToggleAddMinus == false)
            {
                GermThreeAmount[4] = GermThreeAmount[4] - IncDecAmount;
            }
            else
            {
                GermThreeAmount[4] = GermThreeAmount[4] + IncDecAmount;
            }
        }
        if (VerticalGrid == 3 && HorizontalGrid == 40)
        {
            if (ToggleAddMinus == false)
            {
                GermFourAmount[4] = GermFourAmount[4] - IncDecAmount;
            }
            else
            {
                GermFourAmount[4] = GermFourAmount[4] + IncDecAmount;
            }
        }
    }   
    void WaveAmountCheck()
    {
        for(int i = 0; i < 5; i++)
        {
            if(GermOneAmount[i] < 0)
            {
                GermOneAmount[i] = 0;
            }
            if (GermTwoAmount[i] < 0)
            {       
                GermTwoAmount[i] = 0;
            }
            if (GermThreeAmount[i] < 0)
            {       
                GermThreeAmount[i] = 0;
            }
            if (GermFourAmount[i] < 0)
            {       
                GermFourAmount[i] = 0;
            }
            //
            if (GermOneAmount[i] > GermOneMax)
            {
                GermOneAmount[i] = GermOneMax;
            }
            if (GermTwoAmount[i] > GermTwoMax)
            {
                GermTwoAmount[i] = GermTwoMax;
            }
            if (GermThreeAmount[i] > GermThreeMax)
            {
                GermThreeAmount[i] = GermThreeMax;
            }
            if (GermFourAmount[i] > GermFourMax)
            {
                GermFourAmount[i] = GermFourMax;
            }


        }
    }
    void WaveColorCheck()
    {
        for(int i = 0; i < 4;i++)
        {
            WaveOneCount[i].color =  Color.black;
            WaveTwoCount[i].color =  Color.black;
            WaveThreeCount[i].color =Color.black;
            WaveFourCount[i].color = Color.black;
            WaveFiveCount[i].color = Color.black;
        }
        for (int i = 0; i < 20; i++)
        {
            WaveButtons[i].image.color = Color.white;
        }
        //Wave One
        if (VerticalGrid == 0 && HorizontalGrid == 0)
        {
            WaveOneCount[0].color = Color.white;
            WaveButtons[0].image.color = Color.black;
        }
        if (VerticalGrid == 1 && HorizontalGrid == 0)
        {
            WaveOneCount[1].color = Color.white;
            WaveButtons[1].image.color = Color.black;
        }
        if (VerticalGrid == 2 && HorizontalGrid == 0)
        {
            WaveOneCount[2].color = Color.white;
            WaveButtons[2].image.color = Color.black;
        }
        if (VerticalGrid == 3 && HorizontalGrid == 0)
        {
            WaveOneCount[3].color = Color.white;
            WaveButtons[3].image.color = Color.black;
        }
        //Wave Two
        if (VerticalGrid == 0 && HorizontalGrid == 10)
        {
            WaveTwoCount[0].color = Color.white;
            WaveButtons[4].image.color = Color.black;
        }
        if (VerticalGrid == 1 && HorizontalGrid == 10)
        {
            WaveTwoCount[1].color = Color.white;
            WaveButtons[5].image.color = Color.black;
        }
        if (VerticalGrid == 2 && HorizontalGrid == 10)
        {
            WaveTwoCount[2].color = Color.white;
            WaveButtons[6].image.color = Color.black;
        }
        if (VerticalGrid == 3 && HorizontalGrid == 10)
        {
            WaveTwoCount[3].color = Color.white;
            WaveButtons[7].image.color = Color.black;
        }
        //Wave Three
        if (VerticalGrid == 0 && HorizontalGrid == 20)
        {
            WaveThreeCount[0].color = Color.white;
            WaveButtons[8].image.color = Color.black;
        }
        if (VerticalGrid == 1 && HorizontalGrid == 20)
        {
            WaveThreeCount[1].color = Color.white;
            WaveButtons[9].image.color = Color.black;
        }
        if (VerticalGrid == 2 && HorizontalGrid == 20)
        {
            WaveThreeCount[2].color = Color.white;
            WaveButtons[10].image.color = Color.black;
        }
        if (VerticalGrid == 3 && HorizontalGrid == 20)
        {
            WaveThreeCount[3].color = Color.white;
            WaveButtons[11].image.color = Color.black;
        }
        //Wave Four
        if (VerticalGrid == 0 && HorizontalGrid == 30)
        {
            WaveFourCount[0].color = Color.white;
            WaveButtons[12].image.color = Color.black;
        }
        if (VerticalGrid == 1 && HorizontalGrid == 30)
        {
            WaveFourCount[1].color = Color.white;
            WaveButtons[13].image.color = Color.black;
        }
        if (VerticalGrid == 2 && HorizontalGrid == 30)
        {
            WaveFourCount[2].color = Color.white;
            WaveButtons[14].image.color = Color.black;
        }
        if (VerticalGrid == 3 && HorizontalGrid == 30)
        {
            WaveFourCount[3].color = Color.white;
            WaveButtons[15].image.color = Color.black;
        }
        //Wave Five
        if (VerticalGrid == 0 && HorizontalGrid == 40)
        {
            WaveFiveCount[0].color = Color.white;
            WaveButtons[16].image.color = Color.black;
        }
        if (VerticalGrid == 1 && HorizontalGrid == 40)
        {
            WaveFiveCount[1].color = Color.white;
            WaveButtons[17].image.color = Color.black;
        }
        if (VerticalGrid == 2 && HorizontalGrid == 40)
        {
            WaveFiveCount[2].color = Color.white;
            WaveButtons[18].image.color = Color.black;
        }
        if (VerticalGrid == 3 && HorizontalGrid == 40)
        {
            WaveFiveCount[3].color = Color.white;
            WaveButtons[19].image.color = Color.black;
        }
    }
}

