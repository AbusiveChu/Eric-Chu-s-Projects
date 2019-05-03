using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClearFriendList : MonoBehaviour 
{
    Button clearList;

    void Start()
    {
        clearList = GetComponent<Button>();

        clearList.onClick.AddListener(ClearList);
    }

    void ClearList()
    {
        AddFriend.FriendNum = 0;

        for (int i = 0; i < 6; i++)
        {
            Destroy(AddFriend.friends[i]);
        }
    }
}
