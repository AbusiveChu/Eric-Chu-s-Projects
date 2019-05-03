using UnityEngine;
using System.Collections;

public class removewall : MonoBehaviour
{
    public GameObject Wall;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EnemySpawn.CheckPoint == false)
        {
            Wall.SetActive(false);
        }
        else if (EnemySpawn.CheckPoint == true)
        {
            Wall.SetActive(true);
        }
    }
}
