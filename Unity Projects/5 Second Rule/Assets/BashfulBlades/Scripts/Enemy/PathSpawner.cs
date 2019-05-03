using UnityEngine;
using System.Collections;

public class PathSpawner : MonoBehaviour {
    public GameObject[] PathSpawnPos;
    public GameObject Trail;
    public static int PathPick;
    public int progress;
    public bool once = true;
	// Use this for initialization
	void Start ()
    {
        PathPick = 0;
        progress= 0;
        once = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (once == true)
        {
            Vector3 tempVec = new Vector3(PathSpawnPos[progress].transform.position.x, PathSpawnPos[progress].transform.position.y, PathSpawnPos[progress].transform.position.z);
            Instantiate(Trail, tempVec, PathSpawnPos[progress].transform.rotation);
            PathPick++;
            progress++;
         if(progress > 5)
            {
                once = false;
            }
           
            
        }
	}
}
