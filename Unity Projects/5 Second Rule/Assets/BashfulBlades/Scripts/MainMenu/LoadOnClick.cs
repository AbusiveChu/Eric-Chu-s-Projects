using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadOnClick : MonoBehaviour 
{

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        //SceneManager.LoadScene(MainMenu);
    }
}
