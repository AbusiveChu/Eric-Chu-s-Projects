using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Feedback : MonoBehaviour {

    public Text[] TrapsGamePad;
   public Text[] TrapsKeyboard;
   public GameObject TrapKeyboardUI;
    public GameObject TrapGamePadUI;
    public GameObject[] COINSKeyboardUI;
    public GameObject[] COINSGamePadUI;
    public Text AmountLeft;

    //COOLDOWNS
    public Text UltiKeyboard;
    public Text SecondKeyboard;
    public Text UltiGamepad;
    public Text SecondGamepad;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Movement.GPConnected == false)
        {

            UltiKeyboard.text = Abilities.CoolDownTimerUlti.ToString("0");
            SecondKeyboard.text = Abilities.CoolDownTimerSecond.ToString("0");
            UltiKeyboard.color = Color.red;
            SecondKeyboard.color = Color.red;
        }                           
        else if (Movement.GPConnected == true)
        {     
            UltiGamepad.text = Abilities.CoolDownTimerUlti.ToString("0");
            SecondGamepad.text = Abilities.CoolDownTimerSecond.ToString("0");
            UltiGamepad.color = Color.red;
            SecondGamepad.color = Color.red;

        }
        if (Abilities.CoolDownTimerUlti <= 0)
        {
            UltiGamepad.text = "GO";
            UltiKeyboard.text = "GO";
            UltiGamepad.color = Color.green;
            UltiKeyboard.color = Color.green;
        }
        if (Abilities.CoolDownTimerSecond <= 0)
        {
            SecondGamepad.text = "GO";
            SecondKeyboard.text = "GO";
            SecondGamepad.color = Color.green;
            SecondKeyboard.color = Color.green;
        }

        AmountLeft.text = EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber].ToString();
        if(ItemScript.BPAmount <= 0)
        {
            TrapsGamePad[0].text = ItemScript.BPCost.ToString();
            TrapsKeyboard[0].text = ItemScript.BPCost.ToString();
            TrapsGamePad[0].color = Color.yellow;
            TrapsKeyboard[0].color = Color.yellow;
            if (Movement.GPConnected == false)
            {
                COINSKeyboardUI[0].SetActive(true);
            }
            else if(Movement.GPConnected == true)
            {
                COINSGamePadUI[0].SetActive(true);
            }
        }
        else if(ItemScript.BPAmount > 0)
        {
            TrapsGamePad[0].text = ItemScript.BPAmount.ToString();
            TrapsKeyboard[0].text = ItemScript.BPAmount.ToString();
        
        }
        if (ItemScript.GCAmount <= 0)
        {
            TrapsGamePad[1].text = ItemScript.GCCost.ToString();
            TrapsKeyboard[1].text = ItemScript.GCCost.ToString();
            TrapsGamePad[1].color = Color.yellow;
            TrapsKeyboard[1].color = Color.yellow;
            if (Movement.GPConnected == false)
            {
                COINSKeyboardUI[1].SetActive(true);
            }
            else if (Movement.GPConnected == true)
            {
                COINSGamePadUI[1].SetActive(true);
            }
        }
        else if (ItemScript.GCAmount > 0)
        {
            TrapsGamePad[1].text = ItemScript.GCAmount.ToString();
            TrapsKeyboard[1].text = ItemScript.GCAmount.ToString();
        
        }
        if (ItemScript.HCAmount <= 0)
        {
            TrapsGamePad[2].text = ItemScript.HCCost.ToString();
            TrapsKeyboard[2].text = ItemScript.HCCost.ToString();
            TrapsGamePad[2].color = Color.yellow;
            TrapsKeyboard[2].color = Color.yellow;
            if (Movement.GPConnected == false)
            {
                COINSKeyboardUI[2].SetActive(true);
            }
            else if (Movement.GPConnected == true)
            {
                COINSGamePadUI[2].SetActive(true);
            }
        }
        else if (ItemScript.HCAmount > 0)
        {
            TrapsGamePad[2].text = ItemScript.HCAmount.ToString();
            TrapsKeyboard[2].text = ItemScript.HCAmount.ToString();
    
        }
        if (ItemScript.MTAmount <= 0)
        {
            TrapsGamePad[3].text = ItemScript.MTCost.ToString();
            TrapsKeyboard[3].text = ItemScript.MTCost.ToString();
            TrapsGamePad[3].color = Color.yellow;
            TrapsKeyboard[3].color = Color.yellow;
            if (Movement.GPConnected == false)
            {
                COINSKeyboardUI[3].SetActive(true);
            }
            else if (Movement.GPConnected == true)
            {
                COINSGamePadUI[3].SetActive(true);
            }
        }
        else if (ItemScript.MTAmount > 0)
        {
            TrapsGamePad[3].text = ItemScript.MTAmount.ToString();
            TrapsKeyboard[3].text = ItemScript.MTAmount.ToString();
        }     
    }
}
