using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class LevelEnd : MonoBehaviour 
{
    [DllImport("InputDLL")]
    public static extern void Shutdown();

    void OnApplicationQuit()
    { 
    
    }
}
