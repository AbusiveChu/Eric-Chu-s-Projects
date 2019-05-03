using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class HostNetwork : MonoBehaviour
{
    bool doOnce = false;
    bool doTwice = false;

    void Start()
    {
        NetworkManager.Init();
        NetworkManager.ListenStart();
        NetworkManager.ListenUpdate();
        doOnce = true;
    }
    void Update()
    {
        if (doOnce)
        {
            //Debug.Log("Load the lobby here");
            Debug.Log(Marshal.PtrToStringAnsi(NetworkManager.GetRecvT()));
            SceneManager.LoadScene(9);
        }
    }
}
