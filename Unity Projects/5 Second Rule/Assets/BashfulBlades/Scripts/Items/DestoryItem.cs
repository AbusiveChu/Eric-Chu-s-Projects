using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DestoryItem : MonoBehaviour {
	//public float ItemHP = 10.0f;
    public Material[] DecayMat;
    public Renderer ItemMat;
    private int decayvalue;
    public TextMesh ItemHPDisplay;
    public float Countdown;
    public bool lifetime = false;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (lifetime == true)
        {
            Countdown = Countdown - 1;
            if (Countdown == 10)
            {
                ItemMat.material = DecayMat[0];
            }
            if (Countdown == 8)
            {
                ItemMat.material = DecayMat[1];
            }
            if (Countdown == 6)
            {
                ItemMat.material = DecayMat[2];
            }
            if (Countdown == 4)
            {
                ItemMat.material = DecayMat[3];
            }
            if (Countdown == 2)
            {
                ItemMat.material = DecayMat[4];
            }
            if (Countdown < 0)
            {
                Destroy(gameObject);
            }
        }
        ItemHPDisplay.text = Countdown.ToString();
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Enemy")
        {
            if (lifetime == false)
            {
                Countdown = 100;
            }
            lifetime = true;
            
        }  
    }
}

