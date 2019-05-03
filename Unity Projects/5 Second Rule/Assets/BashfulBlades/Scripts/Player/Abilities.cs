using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class Abilities : MonoBehaviour {

    //Broc Lee's Skill Cooldown
    public static float CoolDownBroc;
    public static float delaySkillBroc = 2;
    public static float CoolDownBrocU;
    public static float delaySkillBrocU = 20;
    //Carrot Girl's Skill Cooldown
    public static float CoolDownCarrot;
    public static float delaySkillCarrot = 2;
    public static float CoolDownCarrotU;
    public static float delaySkillCarrotU = 20;
    //Hambo's Skill Cooldown
    public static float CoolDownHambo;
    public static float delaySkillHambo = 10;
    public static float CoolDownHamboU;
    public static float delaySkillHamboU = 20;
    public static bool HamboUlti = false;
    public static float CoolDown;
    public static float delaySkill = 5;
    //Chicken's Skill Cooldown
    public static float CoolDownChicken;
    public static float delaySkillChicken = 4;
    public static float CoolDownChickenU;
    public static float delaySkillChickenU = 20;
    //General Cooldown
    public static float CoolDownTimerUlti;
    public static float CoolDownTimerSecond; 
    //Broc Lee's Ulti Stuff
    public GameObject BrocLeeShieldPush;
    public GameObject BrocLeePos;
   
   
	//4 = LT //5 = LB //6 = RT //7 = RB
	// Update is called once per frame
	void Update () {
        
            
                CoolDownTimerUlti -= Time.deltaTime;
                CoolDownTimerSecond -= Time.deltaTime;
           
        
        if(CoolDownTimerUlti < 0)
        {
            CoolDownTimerUlti = 0;
        }
        if (CoolDownTimerSecond < 0)
        {
            CoolDownTimerSecond = 0;
        }
        if (EnemyShop.EnemyShopToggle == false && Player.Stunned == false)
        {
            //Broco's Ability
            if (SetLevel.Broco == true)
            {
                if ((ControllerManager.GetButtonDown(7) == true && Time.time > CoolDownBrocU || Input.GetKeyDown(KeyCode.Z)) && Time.time > CoolDownBrocU)
                {
                    CoolDownBrocU = Time.time + delaySkillBrocU;
                    Instantiate(BrocLeeShieldPush, BrocLeePos.transform.position, BrocLeePos.transform.rotation);
                    CoolDownTimerUlti = delaySkillBrocU;
                }
            }
            //Shield around Food Item (will be made at later time)
            //Hambo's Abiltiy
            if (SetLevel.Hambo == true)
            {
                if ((ControllerManager.GetButtonDown(5) == true || Input.GetMouseButtonDown(1)) && Time.time > CoolDownHambo)
                {
                    ToolTip.SecondaryShot = true;
                    CoolDown = Time.time + delaySkill;
                    Shooting.fireRate = 0.1f;
                    CoolDownHambo = Time.time + delaySkillHambo;
                    CoolDownTimerSecond = delaySkillHambo;
                }
                if (Time.time > CoolDownHambo)
                {

                }
                if (Time.time > CoolDown)
                {
                    Shooting.fireRate = 0.2f;
                }
                if ((ControllerManager.GetButtonDown(7) == true && Time.time > CoolDownHamboU || Input.GetKeyDown(KeyCode.Z) && Time.time > CoolDownHamboU))
                {
                    CoolDown = Time.time + delaySkill;
                    Shooting.fireRate = 0.1f;
                    HamboUlti = true;
                    CoolDownHamboU = Time.time + delaySkillHamboU;
                    CoolDownTimerUlti = delaySkillHamboU;
                }
                if (Time.time > CoolDownHamboU)
                {

                }
                if (Time.time > CoolDown)
                {
                    Shooting.fireRate = 0.2f;
                    HamboUlti = false;
                }
            }
        }
    }
}
