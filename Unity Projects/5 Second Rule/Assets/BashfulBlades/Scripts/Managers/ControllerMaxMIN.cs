using UnityEngine;
using System.Collections;

public class ControllerMaxMIN : MonoBehaviour
{
    //
    private int offset = 10;
    private float baseoffset = 0.25f;
    //Detect Controller MAX/MIN and stuff
    //Base Values
    public static float Left_ControllerBaseX;
    public static float Left_ControllerBaseY;
    public static float Right_ControllerBaseX;
    public static float Right_ControllerBaseY;
    //Righ statictint
    public static int Right_ControllerMaxX;
    public static int Right_ControllerMinX;
    public static int Right_ControllerMaxY;
    public static int Right_ControllerMinY;
    //Left static int                     ;
    public static int Left_ControllerMaxX;
    public static int Left_ControllerMinX;
    public static int Left_ControllerMaxY;
    public static int Left_ControllerMinY;
    //List ener                              
    //Righ t
    private int Listen_Right_ControllerX;
    private int Listen_Right_ControllerY;
    private int Listen_Right_ControllerMaxX;
    private int Listen_Right_ControllerMaxY;
    private int Listen_Right_ControllerMinX;
    private int Listen_Right_ControllerMinY;


    private int Listen_Left_ControllerX;
    private int Listen_Left_ControllerY;
    private int Listen_Left_ControllerMaxX;
    private int Listen_Left_ControllerMaxY;
    private int Listen_Left_ControllerMinX;
    private int Listen_Left_ControllerMinY;
    // Use this for initialization
    
    void Start()
    {
        ControllerManager.Initialize(1);
        
    }

    // Update is called once per frame
    void Update()
    {
        Listen_Left_ControllerX = ControllerManager.GetLeftJoyStickX();
        Listen_Left_ControllerY = ControllerManager.GetLeftJoyStickY();
        Listen_Right_ControllerX = ControllerManager.GetRightJoyStickX();
        Listen_Right_ControllerY = ControllerManager.GetRightJoyStickY();
        Left_ControllerBaseX = (Left_ControllerMaxX * baseoffset);
        Left_ControllerBaseY = (Left_ControllerMaxY * baseoffset); 
        Right_ControllerBaseX = (Right_ControllerMaxX * baseoffset); 
        Right_ControllerBaseY = (Right_ControllerMaxY * baseoffset); 
        //LISTEN FOR RIGHT X
        if (Listen_Right_ControllerX > Right_ControllerBaseX)
        {
            Listen_Right_ControllerMaxX = Listen_Right_ControllerX;
            if (Listen_Right_ControllerMaxX > Right_ControllerMaxX)
            {
                Right_ControllerMaxX = (Listen_Right_ControllerX - (Listen_Right_ControllerX/ offset));
            }
        }
        if (Listen_Right_ControllerX < Right_ControllerBaseX)
        {
            Listen_Right_ControllerMinX = Listen_Right_ControllerX;
            if (Listen_Right_ControllerMinX < Right_ControllerMinX)
            {
                Right_ControllerMinX = (Listen_Right_ControllerX - (Listen_Right_ControllerX / offset));
            }
        }
        //LISTEN FOR RIGHT Y
        if (Listen_Right_ControllerY > Right_ControllerBaseY)
        {
            Listen_Right_ControllerMaxY = Listen_Right_ControllerY;
            if (Listen_Right_ControllerMaxY > Right_ControllerMaxY)
            {
                Right_ControllerMaxY = (Listen_Right_ControllerY - (Listen_Right_ControllerY / offset));
            }
        }
        if (Listen_Right_ControllerY < Right_ControllerBaseY)
        {
            Listen_Right_ControllerMinY = Listen_Right_ControllerY;
            if (Listen_Right_ControllerMinY < Right_ControllerMinY)
            {
                Right_ControllerMinY = (Listen_Right_ControllerY - (Listen_Right_ControllerY / offset));
            }
        }
        //LISTEN FOR Left X
        if (Listen_Left_ControllerX > Left_ControllerBaseX)
        {
            Listen_Left_ControllerMaxX = Listen_Left_ControllerX;
            if (Listen_Left_ControllerMaxX > Left_ControllerMaxX)
            {
                Left_ControllerMaxX = (Listen_Left_ControllerX - (Listen_Left_ControllerX / offset));
            }
        }
        if (Listen_Left_ControllerX < Left_ControllerBaseX)
        {
            Listen_Left_ControllerMinX = Listen_Left_ControllerX;
            if (Listen_Left_ControllerMinX < Left_ControllerMinX)
            {
                Left_ControllerMinX = (Listen_Left_ControllerX - (Listen_Left_ControllerX / offset));
            }
        }
        //LISTEN FOR Left Y
        if (Listen_Left_ControllerY > Left_ControllerBaseY)
        {
            Listen_Left_ControllerMaxY = Listen_Left_ControllerY;
            if (Listen_Left_ControllerMaxY > Left_ControllerMaxY)
            {
                Left_ControllerMaxY = (Listen_Left_ControllerY - (Listen_Left_ControllerY / offset));
            }
        }
        if (Listen_Left_ControllerY < Left_ControllerBaseY)
        {
            Listen_Left_ControllerMinY = Listen_Left_ControllerY;
            if (Listen_Left_ControllerMinY < Left_ControllerMinY)
            {
                Left_ControllerMinY = (Listen_Left_ControllerY - (Listen_Left_ControllerY / offset));
            }
        }
    }
}
