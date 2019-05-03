using UnityEngine;
using System.Collections;

public class EnemyDetect : MonoBehaviour {

    public GameObject FoodFeedback;
    public GameObject FoodParticleFeedback;
    public float FoodFeedbackReset = 10;
    public float FoodFeedbackCoolDown = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > FoodFeedbackCoolDown)
        {
            FoodFeedback.SetActive(false);
            FoodParticleFeedback.SetActive(false);
        }
	}

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy")
        {
            FoodFeedback.SetActive(true);
            FoodParticleFeedback.SetActive(true);
            FoodFeedbackCoolDown = Time.time + FoodFeedbackReset;
        }

    }
}
