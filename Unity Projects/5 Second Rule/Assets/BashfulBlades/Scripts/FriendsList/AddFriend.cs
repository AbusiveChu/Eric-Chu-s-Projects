using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddFriend : MonoBehaviour 
{
    public static int FriendNum = 0;
    public GameObject friendPrefab;
    public GameObject friendList;

    Button addButton, join;
    public GameObject newFriend;
    public Text address, nickName;
    public GameObject test;

    public static bool buttonPressed = false;

    public static GameObject[] friends = new GameObject[6];

	void Start () 
    {
        addButton = GetComponent<Button>(); 

        addButton.onClick.AddListener(AddF);
	}
    
    void AddF()
    {
        if (FriendNum < 6 && !string.IsNullOrEmpty(AddIPAddress.IPAddress) && !string.IsNullOrEmpty(AddNickname.nickName))
        {
            buttonPressed = true;
            newFriend = (GameObject)Instantiate(friendPrefab, new Vector3(0.0f, 163.5f - (40.0f * FriendNum), 0.0f), new Quaternion(0, 0, 0, 0));
            newFriend.transform.SetParent(friendList.transform, false);

            address = newFriend.transform.GetChild(1).GetComponent<Text>();
            nickName = newFriend.transform.GetChild(2).GetComponent<Text>();
            join = newFriend.transform.GetChild(3).GetComponent<Button>();

            address.text = AddIPAddress.IPAddress;
            nickName.text = AddNickname.nickName;

            join.GetComponent<JoinFriend>().IP = address.text;
            join.GetComponent<JoinFriend>().nickName = nickName.text;
            join.GetComponent<JoinFriend>().friendNum = FriendNum;

            friends[FriendNum] = newFriend;

            FriendNum++;
        }
    }
}
