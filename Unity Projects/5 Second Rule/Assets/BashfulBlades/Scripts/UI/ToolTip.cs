using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToolTip : MonoBehaviour
{

    public GameObject Tooltip;
    public Text TooltipText;
    public bool ToolTipToogle;
    public static int ToolTipProgress = -1;
    public static int ToolTipProgressTraps;
    public static int EnemyKills;
    public static int BombKills;
    public static int UltiKills;
    public static float ToolTipCoolDown;
    public static float ToolTipDelay = 5;
    public static float ItemToolTipCoolDown;
    public static float ItemToolTipDelay = 2;
    public static bool didhemove = false;
    public static bool SecondaryShot = false;
    public static bool[] TrapSetup = new bool[4];
    private bool[] traponce = new bool[3];
    // Use this for initialization
    void Start()
    {
        traponce[0] = true;
        traponce[1] = true;
        traponce[2] = true;
    }
    // Update is called once per frame
    void Update()
    {

        if (Time.time > ToolTipCoolDown)
        {
            Tooltip.SetActive(false);
        }
        if (Time.time < ToolTipCoolDown)
        {
            Tooltip.SetActive(true);
        }
        if (ToolTipProgress < 0)
        {
            TooltipText.text = "Press X to begin the wave!";
            ToolTipCoolDown = Time.time + ToolTipDelay;
            ToolTipProgress++;
        }
        if (ToolTipProgress == 0)
        {
            if (EnemySpawn.WaveNumber > 0 && EnemySpawn.CheckPoint == false)
            {
                TooltipText.text = "Use Left-Stick to move and Right-Stick to look around";
                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 1)
        {
            if (EnemySpawn.WaveNumber == 1 && didhemove == true)
            {
                TooltipText.text = "Press RT to Shoot! Germ Kills: " +  EnemyKills + "/10";
                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 2)
        {
            if (EnemyKills > 9)
            {
                TooltipText.text = "Use Left,Right and Down on the DPAD to Place Traps!";
                ToolTipCoolDown = Time.time + ToolTipDelay;
                TrapSetup[0] = true;
                //ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 3)
        {
            if (Time.time > ItemToolTipCoolDown)
            {
                TooltipText.text = "Press LT to throw blackpepper bombs";
                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 4)
        {
            if (BombKills > 2)
            {
                if (SetLevel.Broco == true)
                {
                    TooltipText.text = "Press LB to use your Secondary Broc Lee shoots a Arc Shot";
                }
                else if (SetLevel.Hambo == true)
                {
                    TooltipText.text = "Press LB to use your Secondary Hambo gets increased firerate";
                }
                else if (SetLevel.Chicken == true)
                {
                    TooltipText.text = "Press LB to use your Secondary Chicken shoots a grease shot slowing enemies";
                }
                else if (SetLevel.Carrot == true)
                {
                    TooltipText.text = "Press LB to use your Secondary Carrot Girl shoots a carrot mine";
                }

                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 5)
        {
            if (SecondaryShot == true)
            {

                if (SetLevel.Broco == true)
                {
                    TooltipText.text = "Press RB to use your Ultimate! Broc lee creates a glowing shield around him";
                }
                else if (SetLevel.Hambo == true)
                {
                    TooltipText.text = "Press RB to use your Ultimate! Hambo gets EXPLOSIVE SHOTS";
                }
                else if (SetLevel.Chicken == true)
                {
                    TooltipText.text = "Press RB to use your Ultimate! Chicken shoots a massive EXPLOSION";
                }
                else if (SetLevel.Carrot == true)
                {
                    TooltipText.text = "Press RB to use your Ultimate! Carrot Girl shoots out a METEOR";
                }
                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
                UltiKills = 0;
            }
        }
        if (ToolTipProgress == 6)
        {
            if (UltiKills > 8)
            {
                TooltipText.text = "Defend your food!";
                ToolTipCoolDown = Time.time + ToolTipDelay;
                ToolTipProgress++;
            }
        }
        if (ToolTipProgress == 7)
        {
            if (EnemyKills > 50)
            {
                Tooltip.SetActive(false);
            }
        }

        if (TrapSetup[0] == true)
        {
            if (TrapSetup[1] == true)
            {
                TooltipText.text = "The Honeycomb slows enemies that go over it. Traps placed " + ToolTipProgressTraps + "/4" ;
                ToolTipCoolDown = Time.time + ToolTipDelay;

                if (traponce[0] == true)
                {
                    ToolTipProgressTraps++;
                    traponce[0] = false;
                }
            }
            else if (TrapSetup[2] == true)
            {
                TooltipText.text = "The mousetrap when triggered goes BOOM. Traps placed " + ToolTipProgressTraps + "/4";
                ToolTipCoolDown = Time.time + ToolTipDelay;

                if (traponce[1] == true)
                {
                    ToolTipProgressTraps++;
                    traponce[1] = false;
                }
            }
            else if (TrapSetup[3] == true)
            {
                TooltipText.text = "The Garlic Clove attracts enemies. Traps placed " + ToolTipProgressTraps + "/4";
                ToolTipCoolDown = Time.time + ToolTipDelay;

                if (traponce[2] == true)
                {
                    ToolTipProgressTraps++;
                    traponce[2] = false;
                }
            }

            if (ToolTipProgressTraps > 2)
            {
                ItemToolTipCoolDown = Time.time + ItemToolTipDelay;
                ToolTipProgress++;
                TrapSetup[0] = false;
            }

        }


    }
}
