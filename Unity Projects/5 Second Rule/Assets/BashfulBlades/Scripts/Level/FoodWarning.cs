using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FoodWarning : MonoBehaviour {

    
    public Text Warning;
    public GameObject WarningGO;
    public GameObject WarningPAT;
    public string CustomText;
    public float DisappearTimer;
    public int Type;
    public bool BeingAttacked;
	// Use this for initialization
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
        if (DisappearTimer > Time.time)
        {
            Warning.text = "";
           // WarningGO.SetActive(false);
        }
	
       
	}
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Enemy" || collisionInfo.collider.tag == "StunEnemy" || collisionInfo.collider.tag == "StunBigEnemy")
        {
            DisappearTimer = Time.time + 5;
            Warning.text = CustomText;
            WarningGO.SetActive(true);
          
        }

    }
}
