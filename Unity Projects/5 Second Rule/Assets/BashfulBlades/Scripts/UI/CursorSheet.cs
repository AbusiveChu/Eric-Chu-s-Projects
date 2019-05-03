using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Runtime.InteropServices;


public class CursorSheet : MonoBehaviour
{

    public bool CursorToggle;
    // Use this for initialization
    void Start()
    {
        Cursor.visible = CursorToggle;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || ControllerManager.GetButtonDown(10))
        {
#if UNITY_EDITOR

            EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE

            Application.Quit();
#endif
        }
    }
}
