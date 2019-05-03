using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class debugcontroller : MonoBehaviour {

    public Text[] numbers;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //LEFT
        numbers[0].text = "Left Control X: " + ControllerManager.GetLeftJoyStickX().ToString();
        numbers[1].text = "Left Control Y: " + ControllerManager.GetLeftJoyStickY().ToString();
        numbers[2].text = "Left Max X: " + ControllerMaxMIN.Left_ControllerMaxX.ToString();
        numbers[3].text = "Left Min X: " + ControllerMaxMIN.Left_ControllerMinX.ToString();
        numbers[4].text = "Left Max Y: " + ControllerMaxMIN.Left_ControllerMaxY.ToString();
        numbers[5].text = "Left Min Y: " + ControllerMaxMIN.Left_ControllerMinY.ToString();
        numbers[6].text = "Left Base X: " + ControllerMaxMIN.Left_ControllerBaseX.ToString();
        numbers[7].text = "Left Base Y: " + ControllerMaxMIN.Left_ControllerBaseY.ToString();


        //RIGHT
        numbers[8].text = "Right Control X: " + ControllerManager.GetRightJoyStickX().ToString();
        numbers[9].text = "Right Control Y: " + ControllerManager.GetRightJoyStickY().ToString();
        numbers[10].text = "Right Max X: " + ControllerMaxMIN.Right_ControllerMaxX.ToString();
        numbers[11].text = "Right Min X: " + ControllerMaxMIN.Right_ControllerMinX.ToString();
        numbers[12].text = "Right Max Y: " + ControllerMaxMIN.Right_ControllerMaxY.ToString();
        numbers[13].text = "Right Min Y: " + ControllerMaxMIN.Right_ControllerMinY.ToString();
        numbers[14].text = "Right Base X: " + ControllerMaxMIN.Right_ControllerBaseX.ToString();
        numbers[15].text = "Right Base Y: " + ControllerMaxMIN.Right_ControllerBaseY.ToString();
	}
}
