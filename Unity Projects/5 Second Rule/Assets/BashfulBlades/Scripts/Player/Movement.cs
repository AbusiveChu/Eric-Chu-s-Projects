using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Movement : MonoBehaviour
{
    
    public float maxRotateSpeed = 5;
    public static bool GPConnected;
    public float BaseSpeed;
   // public float NormalSpeed = 30f;
   // public float SprintSpeed = 40f;
    public Rigidbody PlayerRigid;
    public float gravityMult = 5f;
    private int kbX;
    private int kbY;
    private int LeftX;
    private int LeftY;
    private int RightX;
    private int RightY;
    public float MovementSpeedLeft;
   // private float MovementSpeedNegLeft;
    public float MovementSpeedRight;
 //   private float MovementSpeedNegRight;
    private float verticalRight;
    public GameObject target;
    public GameObject pivot;
    public GameObject VerticalTarget;
    public static float YMinLimit = 271.0f;
    public static float YMaxLimit = 330.0f;
    public GameObject[] jumpExceptions = new GameObject[4] { null, null, null, null };
    public bool isGrounded = true;
    public float jumpForce = 10f;

    void Start()
    {
    //    InputManager.SetHookTarget(InputManager.GetActiveWindow());
        //MovementSpeedNegRight = -MovementSpeedRight;
      //  MovementSpeedNegLeft = -MovementSpeedLeft;
        ControllerManager.Initialize(1);
    }

    void OnCollisionEnter(Collision Other)
    {
        if (Other.gameObject.tag == "Ground")
        {
            if (isGrounded)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (jumpExceptions[i] == null)
                    {
                        jumpExceptions[i] = Other.gameObject;
                        break;
                    }
                }
            }
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision Other)
    {
        if (Other.gameObject.tag == "Ground")
        {
            for (int i = 0; i < 4; i++)
            {
                if (jumpExceptions[i] == Other.gameObject)
                {
                    jumpExceptions[i] = null;
                }

                else
                {
                    isGrounded = false;
                }
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Pause.paused == false && Player.Stunned == false && EnemyShop.EnemyShopToggle == false)
        {
            GPConnected = ControllerManager.IsConnected();
            if (GPConnected == true)
            {
                if (Shooting.AimSlow)
                {
                    maxRotateSpeed = 3.8f;
                }
                else
                {
                    maxRotateSpeed = 5.0f;
                }

                RightX = ControllerManager.GetRightJoyStickX();
                RightY = ControllerManager.GetRightJoyStickY();
                LeftX = ControllerManager.GetLeftJoyStickX();
                LeftY = ControllerManager.GetLeftJoyStickY();
                LeftMovementStick();
                RightStickMovement();
            }
            if (GPConnected == false)
            {
                if (Input.GetKey(KeyCode.D) == true)
                {
                    kbX = 1;
                }
                else if (Input.GetKey(KeyCode.A) == true)
                {
                    kbX = -1;
                }
                else
                {
                    kbX = 0;
                }

                if (Input.GetKey(KeyCode.W) == true)
                {
                    kbY = 1;
                }
                else if (Input.GetKey(KeyCode.S) == true)
                {
                    kbY = -1;
                }
                else
                { 
                    kbY = 0; 
                }

                
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded || ControllerManager.GetButtonDown(0) && isGrounded)
            {
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            float horizontal = kbX * BaseSpeed * Time.deltaTime;
            transform.Translate(horizontal, 0, 0);
            float vertical = kbY * BaseSpeed * Time.deltaTime;
            transform.Translate(0, 0, vertical);
            PlayerRigid.AddForce(Physics.gravity * gravityMult, ForceMode.Acceleration);

        }


    }
    void RightStickMovement()
    {
        float verticalRight = 0;
        float horizontalRight = 0;
    
        //if (RightX < 1000 && RightX > -1000 && RightY < 1000 && RightY > -1000)
        //{
        //   // RightStickNotMoving = true;
        //}
        //values were 10,000
        if (RightX > ControllerMaxMIN.Right_ControllerMaxX)
        {
            //RIGHT AIM
            if (RightX / 10000 >= maxRotateSpeed)
            {
                horizontalRight = maxRotateSpeed;
            }
            else
            {
                horizontalRight = RightX / 10000;
            }
        }
        else if (RightX < ControllerMaxMIN.Right_ControllerMinX)
        {
            if (RightX / 10000 <= -maxRotateSpeed)
            {
                horizontalRight = -maxRotateSpeed;
            }
            else
            {
                horizontalRight = RightX / 10000;
            }
        }
        if (RightY > ControllerMaxMIN.Right_ControllerMaxY)
        {
            if (RightY / 15000 >= maxRotateSpeed)
            {
                verticalRight = -maxRotateSpeed;
            }
            else
            {
                verticalRight = -RightY / 15000;
            }
            if (VerticalTarget.transform.eulerAngles.x < YMinLimit)
            {
                VerticalTarget.transform.eulerAngles.Set(YMinLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
            }
            else if (VerticalTarget.transform.eulerAngles.x > YMaxLimit)
            {
                VerticalTarget.transform.eulerAngles.Set(YMaxLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
            }
        }
        else if (RightY < ControllerMaxMIN.Right_ControllerMinY)
        {
            if (RightY / 15000 >= maxRotateSpeed)
            {
                verticalRight = -maxRotateSpeed;
            }
            else
            {
                verticalRight = -RightY / 15000;
            }

            if (VerticalTarget.transform.eulerAngles.x < YMinLimit)
            {
                VerticalTarget.transform.eulerAngles.Set(YMinLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
            }
            else if (VerticalTarget.transform.eulerAngles.x > YMaxLimit)
            {
                VerticalTarget.transform.eulerAngles.Set(YMaxLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
            }

        }
        target.transform.Rotate(0, horizontalRight, 0);
        VerticalTarget.transform.Rotate(verticalRight, 0, 0);
    }
    void LeftMovementStick()
    {
        float verticalLeft = 0;
        float horizontalLeft = 0;
        if (LeftX > ControllerMaxMIN.Left_ControllerMaxX)
        {
            horizontalLeft = BaseSpeed * Time.deltaTime;
            ToolTip.didhemove = true;
        }
        else if (LeftX < ControllerMaxMIN.Left_ControllerMinX)
        {
            horizontalLeft = -BaseSpeed * Time.deltaTime;
            ToolTip.didhemove = true;
        }
        if (LeftY > ControllerMaxMIN.Left_ControllerMaxY)
        {
            verticalLeft = BaseSpeed * Time.deltaTime;
            ToolTip.didhemove = true;
        }
        else if (LeftY < ControllerMaxMIN.Left_ControllerMinY)
        {
            verticalLeft = -BaseSpeed * Time.deltaTime;
            ToolTip.didhemove = true;

        }

        transform.Translate(horizontalLeft, 0, verticalLeft);
        PlayerRigid.AddForce(Physics.gravity * gravityMult, ForceMode.Acceleration);
    }
}
