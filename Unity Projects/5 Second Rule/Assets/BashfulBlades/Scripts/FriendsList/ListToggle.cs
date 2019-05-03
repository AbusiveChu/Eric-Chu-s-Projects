using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListToggle : MonoBehaviour 
{
    public GameObject FriendsList;

    Button connectButton;

    void Start()
    {
        connectButton = GetComponent<Button>();

        connectButton.onClick.AddListener(ToggleMenu);
    }

    void ToggleMenu()
    {
        FriendsList.SetActive(!FriendsList.activeSelf);
    }
}
