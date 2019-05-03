using UnityEngine;
using System.Collections;

public class FoodChunkScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            Food.PlayerHP = Food.PlayerHP + 10;
            this.gameObject.SetActive(false);
            //FoodItemSpawnScript.FoodItemOnce = false;
        }
    }
}
