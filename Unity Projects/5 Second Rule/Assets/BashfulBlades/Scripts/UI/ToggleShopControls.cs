using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToggleShopControls : MonoBehaviour {
    public Text PressA;
    public Text PressY;
    private bool doonce;
    public Text[] DustMiteAmount = new Text[2];
    public Text[] FlyingGuyAmount = new Text[2];
    public Text[] BlueGermAmount = new Text[2];
    public Text[] RedGermAmount = new Text[2];
	// Use this for initialization
	void Start () {
        doonce = true;
	}
	
	// Update is called once per frame
	void Update () {
        DustMiteAmount  [0].text = NetworkEnemySpawn.NetSend_UniqueGreenAmountPerWave.ToString();
        FlyingGuyAmount [0].text = NetworkEnemySpawn.NetSend_UniqueYellowAmountPerWave.ToString();
        BlueGermAmount  [0].text = NetworkEnemySpawn.NetSend_UniqueBlueAmountPerWave.ToString();
        RedGermAmount   [0].text = NetworkEnemySpawn.NetSend_UniqueRedAmountPerWave.ToString();
        DustMiteAmount  [1].text = NetworkEnemySpawn.NetSend_BossGreenAmountPerWave.ToString();
        FlyingGuyAmount [1].text = NetworkEnemySpawn.NetSend_BossYellowAmountPerWave.ToString();
        BlueGermAmount  [1].text = NetworkEnemySpawn.NetSend_BossBlueAmountPerWave.ToString();
        RedGermAmount[1].text = NetworkEnemySpawn.NetSend_BossRedAmountPerWave.ToString();

         if(doonce == true)
        {
	if( ControllerManager.IsConnected() == true)
    {
        PressA.text = "Press A To Purchase";
        PressY.text = "Press Y To Close";        
    }
    else if(ControllerManager.IsConnected() == false)
    {
        PressA.text = "Press ENTER To Purchase";
        PressY.text = "Press U To Close";  
    }
    }
	}
}
