using UnityEngine;
using System.Collections;

public class SpawnerToogle : MonoBehaviour {


    public GameObject AdventureModeSpawner;
    public GameObject MultiplayerSpawner;
	// Use this for initialization
	void Start () {
        if (EnemyClumpSpawner.EnemyClumpSystem == false)
        {
            AdventureModeSpawner.SetActive(true);
            MultiplayerSpawner.SetActive(false);
        }
        else if (EnemyClumpSpawner.EnemyClumpSystem == true)
        {
            AdventureModeSpawner.SetActive(false);
            MultiplayerSpawner.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
