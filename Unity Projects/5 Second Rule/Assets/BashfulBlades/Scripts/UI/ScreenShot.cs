using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenShot : MonoBehaviour {

    [DllImport("ScreenshotPlug")]
    public static extern void TakeScreenShot(int x, int y, int width, int height, string fileName);


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        int x1 = 0;
	    int y1 = 0;
	    int x2 = Screen.width;
	    int y2 = Screen.height;
       
	
	}
}
