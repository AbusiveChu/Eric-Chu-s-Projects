using UnityEngine;
using System.Collections;

public class UpAndDown : MonoBehaviour {

    public RectTransform Arrow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	

        if(Arrow.rect.position.y < 30)
        {
            Arrow.transform.position = new Vector2(Arrow.rect.position.x, Arrow.rect.position.y + 5);
        }
        if(Arrow.rect.position.y > 60)
        {
            Arrow.transform.position = new Vector2(Arrow.rect.position.x, Arrow.rect.position.y - 5);
        }

       
	}
}
