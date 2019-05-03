//#if UNITY_EDITOR

//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System.Runtime.InteropServices;

//public class DTSystemEditor : EditorWindow
//{
//    //file Value
//    string f_loc;
//    int FileLocation;
//    //Options Holder

//    //string[]
//    string f_StoryText;
//    string[] f_OptionsText = new string[5];
//    int f_OptionsTextValue1;
//    int f_OptionsTextValue2;
//    int f_OptionsTextValue3;
//    int f_OptionsTextValue4;

//    string[] OptionTextValue = new string[5];

//    int choice;
//    //Init
//    [DllImport("dtsystem")]
//    public static extern void StartStory();
//    //Save Function
//    [DllImport("dtsystem")]
//    public static extern void SaveStory(int FileValue, string StoryToSave, string OptionOne, string OptionTwo, string OptionThree, string OptionFour, int OptionOneV, int OptionTwoV, int OptionThreeV, int OptionFourV);
//    //Load Functions
//    [DllImport("dtsystem")]
//    public static extern void LoadStory(int location);
//    //
//    [DllImport("dtsystem")]
//    public static extern void LoadOption(int location);
//    // Return Story
//    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
//    public static extern System.IntPtr GetStoryText();
//    //Return Options
//    [DllImport("dtsystem", CallingConvention = CallingConvention.Cdecl)]
//    public static extern System.IntPtr GetOption(int OptionNumber);
//    //Return Options Value
//    [DllImport("dtsystem")]
//    public static extern int GetOptionValue(int OptionNumber);
//    // Return Choice
//    [DllImport("dtsystem")]
//    public static extern int ReturnAnswer(int choice);
//    // Use this for initialization   
//    int temp;


//    void Start()
//    {
//        StartStory();
//    }

//    // Add menu item named "My Window" to the Window menu
//    [MenuItem("Window/DTS")]
//    public static void ShowWindow()
//    {
//        StartStory();
//        //Show existing window instance. If one doesn't exist, make one.
//        EditorWindow.GetWindow(typeof(DTSystemEditor));
//    }

//    void OnGUI()
//    {
//        //Ask for Location
//        GUILayout.Label("Input File Number", EditorStyles.boldLabel);
//        f_loc = EditorGUILayout.TextField("File Number", FileLocation.ToString());

//        //Press Button to load
//        if (GUILayout.Button("Load Story & Options"))
//        {

//            LoadStory(FileLocation);
//            LoadNewText();
//        }


//        f_StoryText = EditorGUILayout.TextField("Story Text", f_StoryText);
//        f_OptionsText[1] = EditorGUILayout.TextField("Option One Text", f_OptionsText[1]);
//        OptionTextValue[1] = EditorGUILayout.TextField("Option One Value", f_OptionsTextValue1.ToString());
//        f_OptionsText[2] = EditorGUILayout.TextField("Option Two Text", f_OptionsText[2]);
//        OptionTextValue[2] = EditorGUILayout.TextField("Option Two Value", f_OptionsTextValue2.ToString());
//        f_OptionsText[3] = EditorGUILayout.TextField("Option Three Text", f_OptionsText[3]);
//        OptionTextValue[3] = EditorGUILayout.TextField("Option Three Value", f_OptionsTextValue3.ToString());
//        f_OptionsText[4] = EditorGUILayout.TextField("Option Four Text", f_OptionsText[4]);
//        OptionTextValue[4] = EditorGUILayout.TextField("Option Four Value", f_OptionsTextValue4.ToString());
//        //Grab all information
//        if (GUILayout.Button("Save Changes"))
//        {
//            SaveStory(FileLocation, f_StoryText, f_OptionsText[1], f_OptionsText[2], f_OptionsText[3], f_OptionsText[4], f_OptionsTextValue1, f_OptionsTextValue2, f_OptionsTextValue3, f_OptionsTextValue4);
//        }
//        if (GUILayout.Button("Goto Option One's Story Part"))
//        {
//            temp = FileLocation;
//            f_loc = f_OptionsTextValue1.ToString();

//            LoadStory(f_OptionsTextValue1);
//            LoadNewText();
//            LoadNewValues();
//        }
//        if (GUILayout.Button("Goto Option Two's Story Part"))
//        {
//            temp = FileLocation;
//            FileLocation = f_OptionsTextValue2;
//            LoadStory(f_OptionsTextValue2);
//            LoadNewText();
//            LoadNewValues();
//        }
//        if (GUILayout.Button("Goto Option Three's Story Part"))
//        {
//            temp = FileLocation;
//            FileLocation = f_OptionsTextValue3;
//            LoadStory(f_OptionsTextValue3);
//            LoadNewText();
//            LoadNewValues();
//        }
//        if (GUILayout.Button("Goto Option Four's Story Part"))
//        {
//            temp = FileLocation;
//            FileLocation = f_OptionsTextValue4;
//            LoadStory(f_OptionsTextValue4);
//            LoadNewText();
//            LoadNewValues();
//        }
//        if (GUILayout.Button("GO Back"))
//        {
//            LoadStory(temp);
//            LoadNewText();
//            LoadNewText();
//        }
//        if (GUI.changed == true)
//        {

//            int.TryParse(f_loc, out FileLocation);
//            int.TryParse(OptionTextValue[1], out f_OptionsTextValue1);
//            int.TryParse(OptionTextValue[2], out f_OptionsTextValue2);
//            int.TryParse(OptionTextValue[3], out f_OptionsTextValue3);
//            int.TryParse(OptionTextValue[4], out f_OptionsTextValue4);

//        }
//    }

//    void LoadNewText()
//    {

//        f_StoryText = Marshal.PtrToStringAnsi(GetStoryText());
//        f_OptionsText[1] = Marshal.PtrToStringAnsi(GetOption(1));
//        f_OptionsText[2] = Marshal.PtrToStringAnsi(GetOption(2));
//        f_OptionsText[3] = Marshal.PtrToStringAnsi(GetOption(3));
//        f_OptionsText[4] = Marshal.PtrToStringAnsi(GetOption(4));
//        f_OptionsTextValue1 = GetOptionValue(1);
//        f_OptionsTextValue2 = GetOptionValue(2);
//        f_OptionsTextValue3 = GetOptionValue(3);
//        f_OptionsTextValue4 = GetOptionValue(4);
//        Debug.Log(GetOptionValue(1));

//    }
//    void LoadNewValues()
//    {
//        f_loc = f_OptionsTextValue1.ToString();
//        OptionTextValue[1] = f_OptionsTextValue1.ToString();
//        OptionTextValue[2] = f_OptionsTextValue1.ToString();
//        OptionTextValue[3] = f_OptionsTextValue1.ToString();
//        OptionTextValue[4] = f_OptionsTextValue1.ToString();
//    }

//}

//#endif