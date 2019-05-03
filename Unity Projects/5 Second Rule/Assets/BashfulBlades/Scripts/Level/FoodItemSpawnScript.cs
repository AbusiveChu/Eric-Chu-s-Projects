using UnityEngine;
using System.Collections;

public class FoodItemSpawnScript : MonoBehaviour {


    public GameObject[] FoodSpawn = new GameObject[3];
    public int WaveCalc;
    public static bool FoodItemOnce;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (EnemySpawn.CheckPoint == false)
        {
            if (FoodItemOnce == false)
            {
                if (EnemySpawn.WaveNumber > 0)
                {
                    if (SetLevel.SinglePlayer == true)
                    {
                        if (EnemySpawn.WaveNumber % 2 == 0)
                        {
                            int temp = Random.Range(0, 2);
                            FoodSpawn[temp].SetActive(true);
                            FoodItemOnce = true;
                        }
                    }
                    else if (SetLevel.SinglePlayer == false)
                    {
                        if (EnemySpawn.WaveNumber % 5 == 0)
                        {
                            int temp = Random.Range(0, 2);
                            FoodSpawn[temp].SetActive(true);
                            FoodItemOnce = true;
                        }
                    }

                }
            }
        }
        else if(EnemySpawn.CheckPoint == true)
        {
            FoodItemOnce = false;
            FoodSpawn[0].SetActive(false);
            FoodSpawn[1].SetActive(false);
            FoodSpawn[2].SetActive(false);
        }
	}
}
