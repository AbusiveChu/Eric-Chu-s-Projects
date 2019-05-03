using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckInput : MonoBehaviour
{
    public static string IPAddress = "";
    void Start()
    {
        var IPInput = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(InputCheck);
        IPInput.onEndEdit = se;
    }

    public void InputCheck(string arg)
    {
        IPAddress = arg;
    }
}
