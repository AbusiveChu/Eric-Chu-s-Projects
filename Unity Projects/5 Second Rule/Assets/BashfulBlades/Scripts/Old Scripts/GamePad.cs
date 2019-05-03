//using UnityEngine;
//using System.Collections;
//using System.Runtime.InteropServices;

//public class GamePad : MonoBehaviour
//{
//    //Init
//    [DllImport("ControllerPlugin")]
//    public static extern void Initialize(int controllerNum);
//    //Vib
//    [DllImport("ControllerPlugin")]
//    public static extern void Vibrate(int leftRumble, int RightRumble);
//    //Bool
//    public static bool GPConnected;
//    [DllImport("ControllerPlugin")]
//    public static extern bool IsConnected();
//    //Int
//    [DllImport("ControllerPlugin")]
//    public static extern int GetLeftJoyStickX();

//    [DllImport("ControllerPlugin")]
//    public static extern int GetLeftJoyStickY();

//    [DllImport("ControllerPlugin")]
//    public static extern int GetRightJoyStickX();

//    [DllImport("ControllerPlugin")]
//    public static extern int GetRightJoyStickY();

//    //Trigger Threshold
//    [DllImport("ControllerPlugin")]
//    public static extern int GetLeftTriggerThreshold(int buttonName);

//    [DllImport("ControllerPlugin")]
//    public static extern int GetRightTriggerThreshold(int buttonName);

//    //Bool Button Down/Release
//    [DllImport("ControllerPlugin")]
//    public static extern bool ControllerManager.GetButtonDown(int buttonName);

//    [DllImport("ControllerPlugin")]
//    public static extern bool GetButtonReleased(int buttonName);

 
//    //Bullet Info

//    public float fireRate;
//    public Rigidbody shot;
//    public Transform shotSpawn;
//    public Camera cam;
//    //Movement
//    public float speed = 20f;
//    public Rigidbody PlayerRigid;
//    public float gravityMult = 5f;
//    public GameObject target;
//    public GameObject pivot;
//    public GameObject VerticalTarget;
//    public float maxRotateSpeed = 5;
//    Vector3 offset;
//    public ItemScript Item;
//    //Used for controller;
//    private int LeftX;
//    private int LeftY;
//    private int RightX;
//    private int RightY;
//   // public float MovementSpeedRight;
//   // private float MovementSpeedNegRight;
//   // public float MovementSpeedLeft;
//   // private float MovementSpeedNegLeft;
//    // private float verticalRight;
//    // Use this for initialization
//    public Transform ItemSpawn;
//    public GameObject GarlicClove;
//    public GameObject Blackpepperbombg;
//    public GameObject Mousetrap;
//    public GameObject Honeycomb;
//    public static bool[] ONCEITEM = new bool[5];
//    public GameObject[] Guns;
//    // public int GunNumber;
//    public static float CoolDown;
//    public static float delaySkill = 5;
//    public static float CoolDownBroc;
//    public static float delaySkillBroc = 2;
//    public static float SkillTimerB = 0;
//    public static float SkillTimerH = 0;
//    public static float CoolDownHambo;
//    public static float delaySkillHambo = 15;
//    bool RightStickNotMoving = true;
//    bool once;
//    public AudioSource ShootSound;
//    public GameObject GOPat;
//    public ParticleSystem Pat;
   
//    void Start()
//    {
//        MovementSpeedNegRight = -MovementSpeedRight;
//        MovementSpeedNegLeft = -MovementSpeedLeft;
//        Initialize(1);

//        if (SetLevel.Hambo == true)
//        {
            
//            Pat.startColor = Color.red;
//        }
//        else if (SetLevel.Broco == true)
//        {
           
//            Pat.startColor = Color.green;
//        }
//        else if (SetLevel.Carrot == true)
//        {
          
//            Pat.startColor = new Color(255, 102, 0);
//        }
//        GOPat.SetActive(true);

//    }

//    // Update is called once per frame
//    void Update()
//    {

//        LeftX = GetLeftJoyStickX();
//        LeftY = GetLeftJoyStickY();
//        RightX = GetRightJoyStickX();
//        RightY = GetRightJoyStickY();

//        if(Pause.paused == false)
//        { 
//        GPConnected = IsConnected();
//        if (GPConnected == true && Player.Stunned == false)
//        {
//            //Buttons go as follows:
//            //0	    1	2	3	4	5	6	7	8		9		10		11		12
//            //A     B   X   Y   LT  LB  RT RB  LStick RStick  Back     Start   XboxButton
//            //Shoot
//            // DPAD 13,14,15,16
//            if (ControllerManager.GetButtonDown(6) == true && Time.time > Player.nextFire)
//            {
//                if (Cursor.lockState != CursorLockMode.Locked)
//                {
//                    Cursor.lockState = CursorLockMode.Locked;
//                }
//                Player.nextFire = Time.time + fireRate;
//                Player.ray = cam.ScreenPointToRay(Input.mousePosition);
//                if (Physics.Raycast(Player.ray, out Player.hit))
//                {
//                    Player.hitDir = new Vector3((Player.hit.point.x - shotSpawn.position.x) / (Player.hit.distance),
//                                        (Player.hit.point.y - shotSpawn.position.y) / (Player.hit.distance),
//                                        (Player.hit.point.z - shotSpawn.position.z) / (Player.hit.distance));
//                    Player.hitConfirm = true;
//                }
//                if (SetLevel.Broco == true)
//                {
//                    Instantiate(Guns[0], shotSpawn.position, shotSpawn.rotation);
//                }
//                if (SetLevel.Hambo == true)
//                {
//                    Instantiate(Guns[1], shotSpawn.position, shotSpawn.rotation);
//                }
//                if (SetLevel.Carrot == true)
//                {
//                    Instantiate(Guns[3], shotSpawn.position, shotSpawn.rotation);
//                }
//                ShootSound.Play();
//            }
//            //Broco's Ability
//            if (SetLevel.Broco == true)
//            {
//                if (ControllerManager.GetButtonDown(5) == true && Time.time > CoolDownBroc)
//                {
//                    if (Cursor.lockState != CursorLockMode.Locked)
//                    {
//                        Cursor.lockState = CursorLockMode.Locked;
//                    }
//                    Player.nextFire = Time.time + fireRate;
//                    Player.ray = cam.ScreenPointToRay(Input.mousePosition);
//                    if (Physics.Raycast(Player.ray, out Player.hit))
//                    {
//                        Player.hitDir = new Vector3((Player.hit.point.x - shotSpawn.position.x) / (Player.hit.distance),
//                                            (Player.hit.point.y - shotSpawn.position.y) / (Player.hit.distance),
//                                            (Player.hit.point.z - shotSpawn.position.z) / (Player.hit.distance));
//                        Player.hitConfirm = true;
//                    }
//                    if (SetLevel.Broco == true)
//                    {
//                        Instantiate(Guns[2], shotSpawn.position, shotSpawn.rotation);
//                    }
//                    GOPat.SetActive(false);
//                    CoolDownBroc = Time.time + delaySkillBroc;
//                }
//                if (Time.time > CoolDownBroc)
//                {
//                   GOPat.SetActive(true);
//                }
//            }
//            //Hambo's Abiltiy
//            else if (SetLevel.Hambo == true)
//            {
//                if (ControllerManager.GetButtonDown(5) == true && Time.time > CoolDownHambo)
//                {
//                    CoolDown = Time.time + delaySkill;
//                    fireRate = 0.1f;
//                    CoolDownHambo = Time.time + delaySkillHambo;
//                   GOPat.SetActive(false);
//                }
//                if (Time.time > CoolDownHambo)
//                {
//                   GOPat.SetActive(true);
//                }
//                if (Time.time > CoolDown)
//                {
//                    fireRate = 0.2f;
//                }
//            }


//            //Place Blackpepper Trap
//            if (ControllerManager.GetButtonDown(3) == true)// Y
//            {
//                if (ONCEITEM[0] == true)
//                {
//                    ItemScript.GetItemBP(Blackpepperbombg, ItemSpawn);
//                    ONCEITEM[0] = false;
//                }
//            }
//            else if (ControllerManager.GetButtonDown(3) == false)
//            {
//                ONCEITEM[0] = true;
//            }
//            if (ControllerManager.GetButtonDown(0) == true)//A
//            {
//                if (ONCEITEM[1] == true)
//                {
//                    ItemScript.GetItemMT(Mousetrap, ItemSpawn);
//                    ONCEITEM[1] = false;
//                }
//            }
//            else if (ControllerManager.GetButtonDown(0) == false)
//            {
//                ONCEITEM[1] = true;
//            }

//            if (ControllerManager.GetButtonDown(2) == true)//X
//            {
//                if (ONCEITEM[2] == true)
//                {
//                    ItemScript.GetItemHC(Honeycomb, ItemSpawn);
//                    ONCEITEM[2] = false;
//                }
//            }
//            else if (ControllerManager.GetButtonDown(2) == false)
//            {
//                ONCEITEM[2] = true;
//            }
//            //
//            if (ControllerManager.GetButtonDown(1) == true)//B
//            {
//                if (ONCEITEM[3] == true)
//                {
//                    ItemScript.GetItemGC(GarlicClove, ItemSpawn);
//                    ONCEITEM[3] = false;
//                }

//            }
//            else if (ControllerManager.GetButtonDown(1) == false)
//            {
//                ONCEITEM[3] = true;
//            }


//            RightStickMovement();
//            LeftMovementStick();




//            //Checkpointcheck
//            if (ControllerManager.GetButtonDown(13) == true)
//            {

//                if (EnemySpawn.CheckPoint == true)
//                {
//                    EnemySpawn.CleanUpTime = false;
//                    EnemySpawn.CheckPoint = false;
//                    EnemySpawn.UpdateText = true;
//                    EnemySpawn.WaveNumber += 1;
//                   // Food.PlayerHP += 1;
//                }


//            }     



//        }

//            //float desiredAngleY = VerticalTarget.transform.eulerAngles.x;
//            //float desiredAngleX = target.transform.eulerAngles.y;

//            //Quaternion rotation = Quaternion.Euler(desiredAngleY, desiredAngleX, 0);


//            //transform.position = target.transform.position - (rotation * offset);
//            //transform.LookAt(pivot.transform);
//        }
//    }


//    void RightStickMovement()
//    {
//        float verticalRight = 0;
//        float horizontalRight = 0;

//        if (RightX < 1000 && RightX > -1000 && RightY < 1000 && RightY > -1000)
//        {
//            RightStickNotMoving = true;
//        }

//        if (RightX > 1000)
//        {
//            //RIGHT AIM
//            if (RightX / 10000 >= maxRotateSpeed)
//            {
//                horizontalRight = maxRotateSpeed;
//            }
//            else
//            {
//                horizontalRight = RightX / 10000;
//            }
//        }
//        else if (RightX < -1000)
//        {
//            if (RightX / 10000 <= -maxRotateSpeed)
//            {
//                horizontalRight = -maxRotateSpeed;
//            }
//            else
//            {
//                horizontalRight = RightX / 10000;
//            }
//        }
//        if (RightY > 1000)
//        {
//            if (RightY / 15000 >= maxRotateSpeed)
//            {
//                verticalRight = -maxRotateSpeed;
//            }
//            else
//            {
//                verticalRight = -RightY / 15000;
//            }
//            if (VerticalTarget.transform.eulerAngles.x < Player.YMinLimit)
//            {
//                VerticalTarget.transform.eulerAngles.Set(Player.YMinLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
//            }
//            else if (VerticalTarget.transform.eulerAngles.x > Player.YMaxLimit)
//            {
//                VerticalTarget.transform.eulerAngles.Set(Player.YMaxLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
//            }
//        }
//        else if (RightY < -1000)
//        {
//            if (RightY / 15000 >= maxRotateSpeed)
//            {
//                verticalRight = -maxRotateSpeed;
//            }
//            else
//            {
//                verticalRight = -RightY / 15000;
//            }

//            if (VerticalTarget.transform.eulerAngles.x < Player.YMinLimit)
//            {
//                VerticalTarget.transform.eulerAngles.Set(Player.YMinLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
//            }
//            else if (VerticalTarget.transform.eulerAngles.x > Player.YMaxLimit)
//            {
//                VerticalTarget.transform.eulerAngles.Set(Player.YMaxLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
//            }

//        }
//        target.transform.Rotate(0, horizontalRight, 0);
//        VerticalTarget.transform.Rotate(verticalRight, 0, 0);
//    }
//    void LeftMovementStick()
//    {
//        float verticalLeft = 0;
//        float horizontalLeft = 0;
//        ////RIGHT MOVEMENT
//        //float horizontalLeft = GetLeftJoyStickX() * speed * Time.deltaTime;
//        //transform.Translate(horizontalLeft, 0, 0);
//        //float verticalLeft = GetLeftJoyStickY() * speed * Time.deltaTime;
//        //transform.Translate(0, 0, verticalLeft);
//        //PlayerRigid.AddForce(Physics.gravity * gravityMult, ForceMode.Acceleration);
//        if (LeftX > 2000)
//        {
//            horizontalLeft = speed * Time.deltaTime;
//        }
//        else if (LeftX < -2000)
//        {
//            horizontalLeft = -speed * Time.deltaTime;
//        }
//        if (LeftY > 2000)
//        {
//            verticalLeft = speed * Time.deltaTime;
//        }
//        else if (LeftY < -2000)
//        {
//            verticalLeft = -speed * Time.deltaTime;

//        }

//        transform.Translate(horizontalLeft, 0, verticalLeft);
//        PlayerRigid.AddForce(Physics.gravity * gravityMult, ForceMode.Acceleration);
//    }
//}
