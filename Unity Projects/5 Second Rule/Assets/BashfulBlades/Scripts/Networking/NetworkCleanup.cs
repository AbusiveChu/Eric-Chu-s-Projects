using UnityEngine;
using System.Collections;

public class NetworkCleanup : MonoBehaviour
{

    // Use this for initialization
    void OnApplicationQuit()
    {
        NetworkManager.Shutdown();
    }
}
