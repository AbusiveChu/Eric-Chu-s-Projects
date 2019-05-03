//using UnityEngine;
//using System.Collections;



//public class Keyboard : MonoBehaviour
//{
//    //Movement
//    public float speed = 20f;
//    public float jumpForce = 10f;
//    public float gravityMult = 5f;
//    public bool isGrounded;

//    private Ray ray;
//    public Rigidbody PlayerRigid;
//    //Gun
//    private float nextFire;
//    public float fireRate;
//    public Rigidbody shot;
//    public Transform shotSpawn;
//    //Misc
//    public AudioSource ShotSound;
//    public GameObject GarlicClove;
//    public GameObject Blackpepperbombg;
//    public GameObject Mousetrap;
//    public GameObject Honeycomb;
//    public Transform ItemSpawn;
//    public GameObject[] Guns;

//    public Camera cam;
//    // private Ray ray;
//    private RaycastHit hit;

//    public GameObject GOPat;
//    public ParticleSystem Pat;
//    public static Vector3 hitDir;
//    public static bool hitConfirm = false;
//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Pause.paused == false)
//        {
//            if (GamePad.GPConnected == false)
//            {
//                if (Input.GetMouseButtonDown(0) && Time.time > nextFire && Player.Stunned == false)
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
//                        Instantiate(Guns[0], shotSpawn.position, shotSpawn.rotation);
//                    }
//                    if (SetLevel.Hambo == true)
//                    {
//                        Instantiate(Guns[1], shotSpawn.position, shotSpawn.rotation);
//                    }
//                    if (SetLevel.Carrot == true)
//                    {
//                        Instantiate(Guns[3], shotSpawn.position, shotSpawn.rotation);
//                    }
//                    ShotSound.Play();
//                }
//            }
//            //Movement checks
//            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
//            {
//                PlayerRigid.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
//            }

//            if (Input.GetKeyDown(KeyCode.Y))
//            {
//                if (EnemySpawn.CheckPoint == true)
//                {
//                    EnemySpawn.CleanUpTime = false;
//                    EnemySpawn.CheckPoint = false;
//                    EnemySpawn.UpdateText = true;
//                    EnemySpawn.WaveNumber += 1;
//                    // Food.PlayerHP += 1;
//                }
//            }
//            //Place Blackpepper Trap
//            if (Input.GetKeyDown(KeyCode.Alpha1))// Y
//            {
//                if (GamePad.ONCEITEM[0] == true)
//                {
//                    ItemScript.GetItemBP(Blackpepperbombg, ItemSpawn);
//                    GamePad.ONCEITEM[0] = false;
//                }
//            }
//            else if (Input.GetKeyUp(KeyCode.Alpha1))
//            {
//                GamePad.ONCEITEM[0] = true;
//            }
//            if (Input.GetKeyDown(KeyCode.Alpha2))
//            {
//                if (GamePad.ONCEITEM[1] == true)
//                {
//                    ItemScript.GetItemMT(Mousetrap, ItemSpawn);
//                    GamePad.ONCEITEM[1] = false;
//                }
//            }
//            else if (Input.GetKeyUp(KeyCode.Alpha2))
//            {
//                GamePad.ONCEITEM[1] = true;
//            }

//            if (Input.GetKeyDown(KeyCode.Alpha3))
//            {
//                if (GamePad.ONCEITEM[2] == true)
//                {
//                    ItemScript.GetItemHC(Honeycomb, ItemSpawn);
//                    GamePad.ONCEITEM[2] = false;
//                }
//            }
//            else if (Input.GetKeyUp(KeyCode.Alpha3))
//            {
//                GamePad.ONCEITEM[2] = true;
//            }
//            //
//            if (Input.GetKeyDown(KeyCode.Alpha4))
//            {
//                if (GamePad.ONCEITEM[3] == true)
//                {
//                    ItemScript.GetItemGC(GarlicClove, ItemSpawn);
//                    GamePad.ONCEITEM[3] = false;
//                }

//            }
//            else if (Input.GetKeyUp(KeyCode.Alpha4))
//            {
//                GamePad.ONCEITEM[3] = true;
//            }
//        }
//        //Broco's Ability
//        if (SetLevel.Broco == true)
//        {
//            if (Input.GetMouseButtonDown(1) && Time.time > GamePad.CoolDownBroc)
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
//                    Instantiate(Guns[2], shotSpawn.position, shotSpawn.rotation);
//                }
//                GOPat.SetActive(false);
//                GamePad.CoolDownBroc = Time.time + GamePad.delaySkillBroc;
//            }
//            if (Time.time > GamePad.CoolDownBroc)
//            {
//                GOPat.SetActive(true);
//            }
//        }
//        //Hambo's Abiltiy
//        else if (SetLevel.Hambo == true)
//        {
//            if (Input.GetMouseButtonDown(1) && Time.time > GamePad.CoolDownHambo)
//            {
//                GamePad.CoolDown = Time.time + GamePad.delaySkill;
//                fireRate = 0.1f;
//                GamePad.CoolDownHambo = Time.time + GamePad.delaySkillHambo;
//                GOPat.SetActive(false);
//            }
//            if (Time.time > GamePad.CoolDownHambo)
//            {
//                GOPat.SetActive(true);
//            }
//            if (Time.time > GamePad.CoolDown)
//            {
//                fireRate = 0.2f;
//            }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (Pause.paused == false)
//        {
//            if (GamePad.GPConnected == false)
//            {
//                if (Player.Stunned == true)
//                {
//                    if (Time.time > Player.StunDur)
//                    {
//                        Player.Stunned = false;
//                    }
//                }
//                else if (Player.Stunned == false)
//                {
//                    float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
//                    transform.Translate(horizontal, 0, 0);
//                    float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
//                    transform.Translate(0, 0, vertical);
//                    PlayerRigid.AddForce(Physics.gravity * gravityMult, ForceMode.Acceleration);
//                }
//            }
//        }
//    }
//}