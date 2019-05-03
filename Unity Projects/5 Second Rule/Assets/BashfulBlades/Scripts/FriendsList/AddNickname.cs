using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddNickname : MonoBehaviour
{
    public static string nickName = "";
    UnityEngine.UI.InputField IPInput;
    void Start()
    {
        IPInput = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(InputCheck);
        IPInput.onEndEdit = se;
    }
    void Update()
    {
        if (AddFriend.buttonPressed)
        {
            this.transform.GetChild(2).GetComponent<Text>().text = string.Empty;
        }
    }
    public void InputCheck(string arg)
    {
        //Debug.Log(arg);
        nickName = arg;
    }
}
