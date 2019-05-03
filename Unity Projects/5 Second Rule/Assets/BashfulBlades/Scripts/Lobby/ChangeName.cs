using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeName : MonoBehaviour 
{
    public Button nameChange;
    public Text NameChange;
    public static string Name = "";
    public static bool NameChanged;
	// Use this for initialization
	void Start ()
    {
        NameChanged = false;
        nameChange.onClick.AddListener(changeName);
	}

    void changeName()
    {
        NameChanged = true;
        Name = NameChange.text;
    }
}
