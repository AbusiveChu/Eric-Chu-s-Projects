using UnityEngine;
using System.Collections;

public class FoodChunkControl : MonoBehaviour {


    public GameObject[] FoodItems;
	// Use this for initialization
	void Start () {
	for(int i = 0; i < FoodItems.Length;i++)
    {
        FoodItems[i].SetActive(false);
    }
	}
	
	// Update is called once per frame
	void Update () {

	if(SetLevel.Broco == true)
    {
        FoodItems[0].SetActive(true);
    }
    else if (SetLevel.Hambo == true)
    {
        FoodItems[1].SetActive(true);
    }
    else if (SetLevel.Carrot == true)
    {
        FoodItems[2].SetActive(true);
    }
    else  if (SetLevel.Chicken == true)
    {
        FoodItems[3].SetActive(true);
    }
	}
}
