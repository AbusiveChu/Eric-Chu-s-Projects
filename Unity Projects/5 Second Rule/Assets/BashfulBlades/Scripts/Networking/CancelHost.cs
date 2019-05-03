using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CancelHost : MonoBehaviour 
{
    Button cancelButton;

	void Start () 
    {
        cancelButton = GetComponent<Button>();

        cancelButton.onClick.AddListener(GoBack);
	}
	
	//Go Back to the mainmenu
    public static void GoBack()
    {
        NetworkManager.Shutdown();
        SceneManager.LoadScene(0);
    }
}
