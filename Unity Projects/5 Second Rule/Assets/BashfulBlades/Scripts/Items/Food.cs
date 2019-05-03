using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Food : MonoBehaviour
{
    public static float PlayerHP = 100;
    public AudioSource Chomp;
    // Use this for initialization
    void Start()
    {
        PlayerHP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Life Check
        if (PlayerHP <= 0)
        {
            BoidMovement._boids.Clear();
            SceneManager.LoadScene(1);
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy" || collisionInfo.collider.tag == "StunBigEnemy")
        {
            Chomp.Play();
        }

    }
}
