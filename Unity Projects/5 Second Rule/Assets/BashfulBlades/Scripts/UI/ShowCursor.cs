using UnityEngine;
using System.Collections;

public class ShowCursor : MonoBehaviour 
{
    public bool isCursorOn;
	
	void Start () 
    {
        Cursor.visible = isCursorOn;
	}
}
