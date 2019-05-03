using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameBox : MonoBehaviour 
{
    public static string Name = "";
    void Start()
    {
        var IPInput = gameObject.GetComponent<InputField>();
        var se = new InputField.SubmitEvent();
        se.AddListener(InputCheck);
        IPInput.onEndEdit = se;
    }

    public void InputCheck(string arg)
    {
        Name = arg;
    }
}
