using UnityEngine;
using System.Collections;

public class Animate : MonoBehaviour
{
    public bool isthisforwarding;
    public Animator anim;
    public Transform BodyRot;
    public Vector3 TargetPos;  
    public float RotY;    
    private int LeftX;
    private int LeftY;   
    private GameObject Target;
    public bool Left = false, Right = false, Up = false, Down = false , Attack = false;
    // Use this for initialization
    void Start()
    {        
        Target = GameObject.FindGameObjectWithTag("TargetLook");      
        anim.SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        isthisforwarding = anim.GetBool("Forward");
        ControllerManager.GPConnected = ControllerManager.IsConnected();
        if (ControllerManager.GPConnected == true)
        {
            AnimController();
        }
        else if (ControllerManager.GPConnected == false)
        {

            AnimKeyMouse();
        }
        BoolCheck();
    }

    void LateUpdate()
    {
        BodyRot.LookAt(Target.transform.position);
        BodyRot.Rotate(new Vector3(0, RotY, 0));
    }

    void BoolCheck()
    {
        if (Attack == false)
        {
            if (Left == true && Right == false && Up == false && Down == false)
            {
                RotY = -90;
            }
            else if (Left == true && Right == false && Up == true && Down == false)
            {
                RotY = -45;
            }
            else if (Left == true && Right == false && Up == false && Down == true)
            {
                RotY = -135;
            }
            else if (Left == false && Right == true && Up == false && Down == false)
            {
                RotY = 90;
            }
            else if (Left == false && Right == true && Up == false && Down == false)
            {
                RotY = 90;
            }
            else if (Left == false && Right == true && Up == true && Down == false)
            {
                RotY = 45;
            }
            else if (Left == false && Right == true && Up == false && Down == true)
            {
                RotY = 135;
            }
            else if (Left == false && Right == false && Up == true && Down == false)
            {
                RotY = 0;
            }
            else if (Left == false && Right == false && Up == false && Down == true)
            {
                RotY = 180;
            }
        }
    }
    void AnimKeyMouse()
    {
        if (Input.GetMouseButton(0) == true)
        {
            RotY = 0;
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            if (SetLevel.Chicken == true)
            {
                anim.SetBool("Attacking", true);
            }
        }
        else if (Input.GetMouseButton(0) == false)
        {
            if (SetLevel.Chicken == true)
            {
                anim.SetBool("Attacking", false);
            }
        }

        //W
        if (Input.GetKey(KeyCode.W) == true || Input.GetKey(KeyCode.S) == true || Input.GetKey(KeyCode.A) == true || Input.GetKey(KeyCode.D) == true)
        {
            anim.SetBool("Idle", false);
            if (anim.GetBool("Forward") == false)
            {
                anim.SetBool("Forward", true);
            }
            else if (Input.GetKey(KeyCode.W) == true)
            {
                Up = true;
            }
            else if (Input.GetKey(KeyCode.S) == true)
            {
                Down = true;
            }
            else if (Input.GetKey(KeyCode.A) == true)
            {
                Left = true;
            }
            else if (Input.GetKey(KeyCode.D) == true)
            {
                Right = true;
            }
        }
        else if (Input.GetKey(KeyCode.W) == false || Input.GetKey(KeyCode.S) == false || Input.GetKey(KeyCode.A) == false || Input.GetKey(KeyCode.D) == false)
        {
           
            if (Input.GetKeyUp(KeyCode.W) == false)
            {
                Up = false;
            }
            else if (Input.GetKey(KeyCode.S) == false)
            {
                Down = false;
            }
            else if (Input.GetKey(KeyCode.A) == false)
            {
                Left = false;
            }
            else if (Input.GetKey(KeyCode.D) == false)
            {
                Right = false;
            }
            RotY = 0;
        }
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            anim.SetBool("Idle", true);
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            RotY = 0;
        }
    }
    void AnimController()
    {        
            if (ControllerManager.GetButtonDown(6) == true)
            {
                RotY = 0;
                Up = false;
                Down = false;
                Left = false;
                Right = false;
                Attack = true;
                if (SetLevel.Chicken == true)
                {
                anim.SetBool("Attacking", true);
                }
            }
            else if (ControllerManager.GetButtonDown(6) == false)
            {
                Attack = false;
                if (SetLevel.Chicken == true)
                {
                    anim.SetBool("Attacking", false);
                }
            }
        LeftY = ControllerManager.GetLeftJoyStickY();
        LeftX = ControllerManager.GetLeftJoyStickX();
        //Y
        if (LeftY > ControllerMaxMIN.Left_ControllerMaxY)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Forward", true);
            Up = true;
            Down = false;
        }
        else if (LeftY < ControllerMaxMIN.Left_ControllerMinY)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Forward", true);
            Up = false;
            Down = true;
        }
        if (LeftY < ControllerMaxMIN.Left_ControllerBaseY && LeftY > -ControllerMaxMIN.Left_ControllerBaseY)
        {            
            Up = false;
            Down = false;            
        }
        //X
        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Forward", true);
            Right = true;
            Left = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Forward", true);
            Left = true;
            Right = false;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX)
        {           
            Right = false;
            Left = false;
        }

        if(LeftX < ControllerMaxMIN.Left_ControllerBaseX && LeftX > -ControllerMaxMIN.Left_ControllerBaseX &&LeftY < ControllerMaxMIN.Left_ControllerBaseY && LeftY > -ControllerMaxMIN.Left_ControllerBaseY )
        {
            RotY = 0;
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            anim.SetBool("Forward", false);
            anim.SetBool("Idle", true);
        }
    }
}

