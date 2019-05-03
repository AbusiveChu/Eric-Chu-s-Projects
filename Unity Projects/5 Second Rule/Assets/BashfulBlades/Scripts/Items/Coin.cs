using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
    public Transform PlayerPos;
	public float Distance;
    public AudioSource CoinSound;
	// Use this for initialization
	void Start () {
        
        PlayerPos = GameObject.Find("Player 1").transform;
	}
	
	// Update is called once per frame
	void Update () {

		Distance = Vector3.Distance (transform.position, PlayerPos.position);
		if (Distance > 6) {
			transform.LookAt (PlayerPos.transform);
			transform.position += transform.forward * 100 * Time.deltaTime;
		}
		else if(Distance < 6)
		{       
			Player.Money += 1;
			Destroy(gameObject);
		}

    
	}
}
