using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UpdateMoney : MonoBehaviour {
    public Text money;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        money.text = Player.Money.ToString();
	}
}
