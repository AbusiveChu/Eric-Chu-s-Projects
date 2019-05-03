using UnityEngine;
using System.Collections;

public class SendStuff : MonoBehaviour {

    Transform player1;
    string send = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        player1 = GetComponent<Transform>();

        BuildPack();
        NetworkManager.SendPackU(send);
	}
    void BuildPack()
    {
        //This is not the final packet build
        send = "";
        send += player1.position.x.ToString();
        send += ",";
        send += player1.position.y.ToString();
        send += ",";
        send += player1.position.z.ToString();
    }
}
