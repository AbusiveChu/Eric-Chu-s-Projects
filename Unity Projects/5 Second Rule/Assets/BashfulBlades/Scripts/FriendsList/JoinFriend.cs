using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class JoinFriend : MonoBehaviour 
{
    Button join;
    public string IP;
    public string nickName;

    bool connectCheck = false;

    public int friendNum;

    void Start()
    {
        join = GetComponent<Button>();

        join.onClick.AddListener(JoinLobby);
    }

	void Update () 
    {
        if (connectCheck)
        {
            if (NetworkManager.ListenUpdate() == 0)
                SceneManager.LoadScene(9);
        }
	}

    void JoinLobby()
    {
        NetworkManager.SetIP(IP);
        NetworkManager.Init();
        connectCheck = true;
    }
}
