using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class ControllerManager : MonoBehaviour {

    public static bool GPConnected;
    //Bool
    //public bool GPConnected;
    [DllImport("ControllerPlugin")]
    public static extern bool IsConnected();
    //Init
    [DllImport("ControllerPlugin")]
    public static extern void Initialize(int controllerNum);
    //Vib
    [DllImport("ControllerPlugin")]
    public static extern void Vibrate(int leftRumble, int RightRumble);
    //Bool Button Down/Release
    [DllImport("ControllerPlugin")]
    public static extern bool GetButtonDown(int buttonName);
    // Use this for initialization
    [DllImport("ControllerPlugin")]
    public static extern int GetLeftJoyStickX();
    [DllImport("ControllerPlugin")]
    public static extern int GetLeftJoyStickY();
    [DllImport("ControllerPlugin")]
    public static extern int GetRightJoyStickX();
    [DllImport("ControllerPlugin")]
    public static extern int GetRightJoyStickY();


}
