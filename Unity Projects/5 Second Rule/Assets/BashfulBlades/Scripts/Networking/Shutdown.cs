using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Shutdown : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnApplicationQuit()
    {
        NetworkManager.Shutdown();
    }
}
