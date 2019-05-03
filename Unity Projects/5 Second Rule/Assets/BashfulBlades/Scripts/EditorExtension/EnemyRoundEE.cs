#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Runtime.InteropServices;

public class EnemyRoundEE : EditorWindow
{
    //Init
    [DllImport("dtsystem")]
    public static extern void StartStory();
    //Init
    [DllImport("dtsystem")]
    public static extern void saveEnemyRound(int RoundNumber, int GCWONE, int YCWONE, int BCWONE, int RCWONE, int GCWTWO, int YCWTWO, int BCWTWO, int RCWTWO, int GCWTHREE, int YCWTHREE, int BCWTHREE, int RCWTHREE, int GCWFOUR, int YCWFOUR, int BCWFOUR, int RCWFOUR, int GCWFIVE, int YCWFIVE, int BCWFIVE, int RCWFIVE);
    //Init
    [DllImport("dtsystem")]
    public static extern void loadEnemyRound(int RoundNumber);
    //Init
    [DllImport("dtsystem")]
    public static extern int loadEnemyWave(int Germ, int Wave);
    //Init
    [DllImport("dtsystem")]
    public static extern int loadEnemyDelay(int Germ, int Wave);


    string EnemyRoundS;
    int EnemyRound;
    string[] GreenCountS = new string[6];
    string[] YellowCountS = new string[6];
    string[] BlueCountS = new string[6];
    string[] RedCountS = new string[6];
    int[] GreenCount = new int[6];
    int[] YellowCount = new int[6];
    int[] BlueCount = new int[6];
    int[] RedCount = new int[6];
    int[] GreenDelay = new int[6];
    int[] YellowDelay = new int[6];
    int[] BlueDelay = new int[6];
    int[] RedDelay = new int[6];
    void Start()
    {
        StartStory();
    }
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/EnemyRound Editor")]
    public static void ShowWindow()
    {
        StartStory();
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(EnemyRoundEE));
    }
    void OnGUI()
    {
        GUILayout.Label("Enemy Round Editor", EditorStyles.boldLabel);
        EnemyRoundS = EditorGUILayout.TextField("Round Number: ", EnemyRound.ToString());
        //Press Button to load
        if (GUILayout.Button("Load Round"))
        {
            loadEnemyRound(EnemyRound);         
            for (int i = 1; i < 6; i++)
            {              
                GreenCount[i] = loadEnemyWave(0, i);
                GreenDelay[i] = loadEnemyDelay(0, i);
            }
            for (int i = 1; i < 6; i++)
            {
                YellowCount[i] = loadEnemyWave(1, i);
                YellowDelay[i] = loadEnemyDelay(1, i);
            }
            for (int i = 1; i < 6; i++)
            {
                BlueCount[i] = loadEnemyWave(2, i);
                BlueDelay[i] = loadEnemyDelay(2, i);
            }
            for (int i = 1; i < 6; i++)
            {
                RedCount[i] = loadEnemyWave(3, i);
                RedDelay[i] = loadEnemyDelay(3, i);
            }
        }
       
        GUILayout.Label("Wave 1", EditorStyles.boldLabel);
        GreenCountS[1] = EditorGUILayout.TextField("Green Count Wave 1: ", GreenCount[1].ToString());
        YellowCountS[1] = EditorGUILayout.TextField("Yellow Count Wave 1: ", YellowCount[1].ToString());
        BlueCountS[1] = EditorGUILayout.TextField("Blue Count Wave 1: ", BlueCount[1].ToString());
        RedCountS[1] = EditorGUILayout.TextField("Red Count Wave 1: ", RedCount[1].ToString());
        GUILayout.Label("Wave 2", EditorStyles.boldLabel);
        GreenCountS[2] = EditorGUILayout.TextField("Green Count Wave 2: ", GreenCount[2].ToString());
        YellowCountS[2] = EditorGUILayout.TextField("Yellow Count Wave 2: ", YellowCount[2].ToString());
        BlueCountS[2] = EditorGUILayout.TextField("Blue Count Wave 2: ", BlueCount[2].ToString());
        RedCountS[2] = EditorGUILayout.TextField("Red Count Wave 2: ", RedCount[2].ToString());
        GUILayout.Label("Wave 3", EditorStyles.boldLabel);
        GreenCountS[3] = EditorGUILayout.TextField("Green Count Wave 3: ", GreenCount[3].ToString());
        YellowCountS[3] = EditorGUILayout.TextField("Yellow Count Wave 3: ", YellowCount[3].ToString());
        BlueCountS[3] = EditorGUILayout.TextField("Blue Count Wave 3: ", BlueCount[3].ToString());
        RedCountS[3] = EditorGUILayout.TextField("Red Count Wave 3: ", RedCount[3].ToString());
        GUILayout.Label("Wave 4", EditorStyles.boldLabel);
        GreenCountS[4] = EditorGUILayout.TextField("Green Count Wave 4: ", GreenCount[4].ToString());
        YellowCountS[4] = EditorGUILayout.TextField("Yellow Count Wave 4: ", YellowCount[4].ToString());
        BlueCountS[4] = EditorGUILayout.TextField("Blue Count Wave 4: ", BlueCount[4].ToString());
        RedCountS[4] = EditorGUILayout.TextField("Red Count Wave 4: ", RedCount[4].ToString());
        GUILayout.Label("Wave 5", EditorStyles.boldLabel);
        GreenCountS[5] = EditorGUILayout.TextField("Green Count Wave 5: ", GreenCount[5].ToString());
        YellowCountS[5] = EditorGUILayout.TextField("Yellow Count Wave 5: ", YellowCount[5].ToString());
        BlueCountS[5] = EditorGUILayout.TextField("Blue Count Wave 5: ", BlueCount[5].ToString());
        RedCountS[5] = EditorGUILayout.TextField("Red Count Wave 5: ", RedCount[5].ToString());
        GUILayout.Label("Spawn Delay = 60/Count (300 Count Max)", EditorStyles.boldLabel);
        GUILayout.Label("Blue Germ Count = Count * 10 (Mini Blue)", EditorStyles.boldLabel);
        if(GUI.changed == true)
        {
            for(int i = 1; i < 6; i++)
            {
                int.TryParse(EnemyRoundS, out EnemyRound);
                int.TryParse(GreenCountS[i], out GreenCount[i]);
                int.TryParse(YellowCountS[i], out YellowCount[i]);
                int.TryParse(BlueCountS[i], out BlueCount[i]);
                int.TryParse(RedCountS[i], out RedCount[i]);
            }
        }
        if(GUILayout.Button("Save Round"))
        {
            saveEnemyRound(EnemyRound, GreenCount[1], YellowCount[1], BlueCount[1], RedCount[1], GreenCount[2], YellowCount[2], BlueCount[2], RedCount[2], GreenCount[3], YellowCount[3], BlueCount[3], RedCount[3], GreenCount[4], YellowCount[4], BlueCount[4], RedCount[4], GreenCount[5], YellowCount[5], BlueCount[5], RedCount[5]);
        }




    }
    //Display Round and Germ Count;


}
#endif