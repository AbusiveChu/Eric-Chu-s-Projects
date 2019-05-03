using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour 
{
    private float nextFire;
    public static float fireRate =  0.4f;
    public static bool AimSlow = false;
    public Transform shotSpawn;
    public Camera cam;
    public GameObject[] Guns;
    public GameObject[] Ultis;
    public AudioSource[] ShootSound;   
    public static Ray ray;
    public static RaycastHit hit;
    public static Vector3 hitDir;
    public static bool hitConfirm = false;   
    Vector3 RayPoint;
    public LayerMask RayCastMake;
    void Start()
    {
        RayPoint = new Vector3((Screen.width / 2), (Screen.height / 2), 0);
    }

    void Update()
    {
        //if (Cursor.lockState != CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
        if (Player.Stunned == false && EnemyShop.EnemyShopToggle == false)
        {
            if (ControllerManager.GetButtonDown(6) == true && Time.time > nextFire || Input.GetMouseButton(0) == true && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                ray = cam.ScreenPointToRay(RayPoint);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy" || hit.collider.tag == "StunEnemy" || hit.collider.tag == "StunEnemyBig")
                    {
                        AimSlow = true;
                    }
                    else
                    {
                        AimSlow = false;
                    }
                    hitDir = new Vector3((hit.point.x - shotSpawn.position.x) / (hit.distance),
                                        (hit.point.y - shotSpawn.position.y) / (hit.distance),
                                        (hit.point.z - shotSpawn.position.z) / (hit.distance));
                    hitConfirm = true;
                }
                //SINGLE SHOT
                if (SetLevel.Broco == true) // share same bullet since they both use swords
                {
                    Instantiate(Guns[0], shotSpawn.position, shotSpawn.rotation);
                    ShootSound[0].Play();
                }
                //DOUBLE SHOT
                if (SetLevel.Hambo == true) // double shot
                {
                    if (Abilities.HamboUlti == false)
                    {
                        Instantiate(Guns[1], shotSpawn.position, shotSpawn.rotation);
                    }
                    if (Abilities.HamboUlti == true)
                    {
                        Instantiate(Ultis[1], shotSpawn.position, shotSpawn.rotation);
                    }
                    ShootSound[2].Play();
                }
                //SLICE CUT
                if (SetLevel.Chicken == true)
                {
                    Instantiate(Guns[4], shotSpawn.position, shotSpawn.rotation);
                    ShootSound[1].Play();
                }
                //TRIPLE SHOT
                if (SetLevel.Carrot == true) // triple shot
                {
                    Instantiate(Guns[3], shotSpawn.position, shotSpawn.rotation);
                    ShootSound[1].Play();
                }
               
            }

            
                //ARC SHOT
            if (ControllerManager.GetButtonDown(5) == true || Input.GetMouseButton(1) == true)
                {
                    ToolTip.SecondaryShot = true;
                    nextFire = Time.time + fireRate;
                    ray = cam.ScreenPointToRay(RayPoint);
                    if (Physics.Raycast(ray, out hit))
                    {
                        hitDir = new Vector3((hit.point.x - shotSpawn.position.x) / (hit.distance),
                                             (hit.point.y - shotSpawn.position.y) / (hit.distance),
                                             (hit.point.z - shotSpawn.position.z) / (hit.distance));
                        hitConfirm = true;
                    }
                    if (SetLevel.Broco == true && Time.time > Abilities.CoolDownBroc)
                    {
                        Instantiate(Guns[2], shotSpawn.position, shotSpawn.rotation);
                        Abilities.CoolDownBroc = Time.time + Abilities.delaySkillBroc;
                        Abilities.CoolDownTimerSecond = Abilities.delaySkillBroc;
                        ShootSound[0].Play();
                    }
                    else if(SetLevel.Carrot == true && Time.time > Abilities.CoolDownCarrot)
                    {
                        Instantiate(Guns[5], shotSpawn.position, shotSpawn.rotation);
                        Abilities.CoolDownCarrot = Time.time + Abilities.delaySkillCarrot;
                        Abilities.CoolDownTimerSecond = Abilities.delaySkillCarrot;
                        ShootSound[1].Play();
                    }
                    else if(SetLevel.Chicken == true && Time.time > Abilities.CoolDownChicken)
                    {
                        
                    Instantiate(Guns[6], shotSpawn.position, shotSpawn.rotation);
                    Abilities.CoolDownChicken = Time.time + Abilities.delaySkillChicken;
                    Abilities.CoolDownTimerSecond = Abilities.delaySkillChicken;
                         ShootSound[1].Play();
                    }                   
                }
            
           
                //CARROT RAIL GUN
            if (ControllerManager.GetButtonDown(7) == true || Input.GetKeyDown(KeyCode.Z))
                {
                    nextFire = Time.time + fireRate;
                    ray = cam.ScreenPointToRay(RayPoint);
                    if (Physics.Raycast(ray, out hit))
                    {
                        hitDir = new Vector3((hit.point.x - shotSpawn.position.x) / (hit.distance),
                                             (hit.point.y - shotSpawn.position.y) / (hit.distance),
                                             (hit.point.z - shotSpawn.position.z) / (hit.distance));
                        hitConfirm = true;
                    }
                    if(SetLevel.Carrot == true && Time.time > Abilities.CoolDownCarrotU)
                    {
                        Instantiate(Ultis[0], shotSpawn.position, shotSpawn.rotation);
                    Abilities.CoolDownCarrotU = Time.time + Abilities.delaySkillCarrotU;
                    Abilities.CoolDownTimerUlti = Abilities.delaySkillCarrotU;
                        ShootSound[1].Play();
                    }
                    else if(SetLevel.Chicken == true && Time.time > Abilities.CoolDownChickenU)
                    {
                        
                        Instantiate(Ultis[2], shotSpawn.position, shotSpawn.rotation);

                    Abilities.CoolDownChickenU = Time.time + Abilities.delaySkillChickenU;
                    Abilities.CoolDownTimerUlti = Abilities.delaySkillChickenU;
                         ShootSound[1].Play();
                    }  
                   
                }
            }

          
        }
    }


