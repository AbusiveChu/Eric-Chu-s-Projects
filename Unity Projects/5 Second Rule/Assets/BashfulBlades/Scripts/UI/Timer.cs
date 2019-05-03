using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Timer : MonoBehaviour {

    public float timer = 60;
    string minutes;
    string seconds;
    public Text Text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        minutes = Mathf.Floor(timer / 60).ToString("00");
        seconds = Mathf.Floor(timer % 60).ToString("00");
        Text.text = minutes + ":" + seconds;
        
        if(timer <= 0)
        {
            Application.LoadLevel(3);
        }
    }
}
