using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DTSSS : MonoBehaviour {
   
    [DllImport("dtsystem")]
    public static extern void StartStory();
    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetStoryText(int location);
    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetOptionOneText(int location);
    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetOptionTwoText(int location);
    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetOptionThreeText(int location);
    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr GetOptionFourText(int location);
    [DllImport("dtsystem")]
    public static extern int ReturnAnswer(int choice);
	// Use this for initialization   
	void Start () {
        StartStory();
	}
	
	// Update is called once per frame
	void Update () {             
        
	}
}
