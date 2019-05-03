using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class KeyboardGamePadToggle : MonoBehaviour {


    public GameObject KeyBoardLayout;
    public GameObject GamePadLayout;
    public GameObject EnemyShopToggleObject;
    public Text LayoutText;
  
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Movement.GPConnected == true)
        {
            KeyBoardLayout.SetActive(false);
            GamePadLayout.SetActive(true);
          //  LayoutText.text = "Press D-PAD UP To Progress";

        }
    else if(Movement.GPConnected == false)
        {
            KeyBoardLayout.SetActive(true);
            GamePadLayout.SetActive(false);
            //LayoutText.text = "Press 'Y' To Progress";
        }
    if(EnemyShop.EnemyShopToggle == true)
        {
            EnemyShopToggleObject.SetActive(true);
        }
    else if (EnemyShop.EnemyShopToggle == false)
        {
            EnemyShopToggleObject.SetActive(false);
        }
	}
}
