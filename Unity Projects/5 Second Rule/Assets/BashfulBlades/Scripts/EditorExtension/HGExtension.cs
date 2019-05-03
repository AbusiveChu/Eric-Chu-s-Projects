//#if UNITY_EDITOR

//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System.Runtime.InteropServices;



//public class HGExtension : EditorWindow
//{
//    //Init
//    [DllImport("Assignment3_SourceControl")]
//    public static extern void StartHG();
//    [DllImport("Assignment3_SourceControl")]
//    public static extern bool createBatchFile(int number, string RepoLocation, string RepoLink);

//    string RepoLink;
//    string RepoLocation;
//    // Use this for initialization
//    void Start()
//    {
//        StartHG();
//    }

//    // Add menu item named "My Window" to the Window menu
//    [MenuItem("Window/HG Control")]
//    public static void ShowWindow()
//    {
//        StartHG();
//        //Show existing window instance. If one doesn't exist, make one.
//        EditorWindow.GetWindow(typeof(HGExtension));
//    }

//    void OnGUI()
//    {
//        //Ask for Location
//        GUILayout.Label("Input Repo Website Link", EditorStyles.boldLabel);
//        RepoLink = EditorGUILayout.TextField("", RepoLink);
//        GUILayout.Label("Input Folder Location", EditorStyles.boldLabel);
//        RepoLocation = EditorGUILayout.TextField("", RepoLocation);

//        //1 = Create // 2 = Add FILES // 3 = remove FILES // 4 = COMMIT // 5 = PUSH TO REPO // 6 = PULL // 7 = UPDATE //

//        //Press Button to Create Repo       
//        if (GUILayout.Button("Create Repo"))
//        {
//            createBatchFile(1, RepoLocation,RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Add All Files"))
//        {
//            createBatchFile(2, RepoLocation, RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Remove All Files"))
//        {
//            createBatchFile(3, RepoLocation, RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Commit Changes"))
//        {
//            createBatchFile(4, RepoLocation, RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Push to Repo"))
//        {
//            createBatchFile(5, RepoLocation, RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Pull from Repo"))
//        {
//            createBatchFile(6, RepoLocation, RepoLink);
//        }
//        //Press Button to Create Repo       
//        if (GUILayout.Button("Update to Repo"))
//        {
//            createBatchFile(7, RepoLocation, RepoLink);
//        }


//    }
//}
//#endif