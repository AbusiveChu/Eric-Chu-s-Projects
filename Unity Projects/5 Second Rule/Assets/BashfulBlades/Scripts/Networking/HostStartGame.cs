using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HostStartGame : MonoBehaviour 
{
    Button startGame;
    string startPack;
    public static bool GameStart = false;

	void Start () 
    {
        if (!NetworkManager.GetIsServer())
        {
            gameObject.SetActive(false);
        }

        startGame = GetComponent<Button>();

        startGame.onClick.AddListener(StartGame);
	}

   public void StartGame()
    {
        GameStart = true;
        startPack = "";
       
        startPack += NetworkManager.ClientCount().ToString();
        NetworkUpdate.numberOfPlayers = NetworkManager.ClientCount() + 1;
        startPack += '~';
        startPack += "GameStart";
        NetworkManager.SendPackT(startPack);

        SceneManager.LoadScene(5); //Load the game
    }
}