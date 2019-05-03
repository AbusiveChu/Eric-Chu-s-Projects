using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static int Money = 0;
   
    public static float StunDur;
    public static bool Stunned = false;
    public AudioSource BigStunSound;
    public AudioSource StunSound;
    public GameObject Stun;
   
    void Update()
    {
        if (Stunned == true)
        {
            Stun.SetActive(true);
            if (Time.time > StunDur)
            {
                Stunned = false;
                Stun.SetActive(false);
            }
        }
         
    }

    void OnCollisionEnter(Collision Other)
    {
        if (Other.collider.tag == "StunEnemy")
        {
            if (Stunned == false)
            {
                StunSound.Play();
                Stunned = true;
                StunDur = Time.time + 2;
                EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
                Destroy(Other.gameObject);
            }

        }
        if (Other.collider.tag == "StunEnemyBig")
        {
            if (Stunned == false)
            {
                BigStunSound.Play();
                Stunned = true;
                StunDur = Time.time + 5;
                EnemySpawn.TotalWaveAmount[EnemySpawn.WaveNumber] -= 1;
                Destroy(Other.gameObject);
            }
        }
    }
}
