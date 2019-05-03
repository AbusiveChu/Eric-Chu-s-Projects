using UnityEngine;
using System.Collections;

public class PlayerTwoShooting : MonoBehaviour {

    public GameObject[] Guns;
    public GameObject[] Ultis;
    public AudioSource ShootSound;
    public Transform shotSpawn;
    public int PlayerTwoCharacter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (NetworkUpdate.RecvButtons[3] == 1)
    {
        if (PlayerTwo.PlayerTwoChar == 1)//Broco
        {
            //Guns[0].BulletType = 2;
            Instantiate(Guns[0], shotSpawn.position, shotSpawn.rotation);
        }
        if (PlayerTwo.PlayerTwoChar == 2)//Hambo
        {
            //Guns[1].BulletType = 2;
            Instantiate(Guns[1], shotSpawn.position, shotSpawn.rotation);
        }
        if (PlayerTwo.PlayerTwoChar == 3)//Chicken
        {
            //Guns[4].BulletType = 2;
            Instantiate(Guns[4], shotSpawn.position, shotSpawn.rotation);
        }
        if (PlayerTwo.PlayerTwoChar == 4)//Carrot
        {
           // Guns[3].BulletType = 2;
            Instantiate(Guns[3], shotSpawn.position, shotSpawn.rotation);
        }
    }
    if (NetworkUpdate.RecvButtons[1] == 1)
    {
        if (PlayerTwo.PlayerTwoChar == 1)//Broco
        {
            //Guns[2].BulletType = 2;
            Instantiate(Guns[2], shotSpawn.position, shotSpawn.rotation);
        }
        if (PlayerTwo.PlayerTwoChar == 2)//Hambo
        {

        }
        if (PlayerTwo.PlayerTwoChar == 3)//Chicken
        {
            //Guns[6].BulletType = 2;
            Instantiate(Guns[6], shotSpawn.position, shotSpawn.rotation);
        }
        if (PlayerTwo.PlayerTwoChar == 4)//Carrot
        {
            //Guns[5].BulletType = 2;
            Instantiate(Guns[5], shotSpawn.position, shotSpawn.rotation);
        }
    }
	}
}
