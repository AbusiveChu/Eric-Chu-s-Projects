using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RedCircle : MonoBehaviour {

    public int RedCircleID;
    public GameObject FollowingObject;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(EnemyAI.EnemyList.ContainsKey(RedCircleID) != true)
        {          
            Destroy(gameObject);
        }       
        else if(EnemyAI.EnemyList.ContainsKey(RedCircleID) == true)
        {
            if (FollowingObject != null)
            {
                transform.position = EnemyAI.EnemyList[RedCircleID].transform.position;
            }
            else if(FollowingObject == null)
            {
                Destroy(gameObject);
            }
        }
       
	}
}
