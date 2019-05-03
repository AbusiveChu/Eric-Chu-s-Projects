using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class MouseAimCamera : MonoBehaviour 
{
    public GameObject target;
    public GameObject pivot;
    public GameObject VerticalTarget;
    public float rotateSpeed = 5;

    bool camToggleEnable = true;
    bool keyDown = false;
    public int camToggle = 0;

    [DllImport("User32.dll")]
    public static extern System.IntPtr GetActiveWindow();

    [Tooltip("Crosshair texture")]
    public Texture2D crosshairTex;

    public int DeltaX = 0;
    public int DeltaY = 0;

    int tempX = 0;
    int tempY = 0;

    public float angle;
   // float time = 0;
   // float DeltaTime = 0;

    public bool mouseMove = false;

    float YMinLimit = -40.0f;
    float YMaxLimit = 80.0f;
    Vector3 offset;

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < 90 || angle > 270)
        {
            if (angle > 180)
                angle -= 360;
            if (max > 180)
                max -= 360;
            if (min > 180)
                min -= 360;
        }

        angle = Mathf.Clamp(angle, min, max);

        if (angle < 0)
            angle += 360;
        return angle;
    }
    void Start()
    {
        //SetHookTarget(GetActiveWindow());
        offset = target.transform.position - transform.position;
    }

    void Update()
    {
        if (tempX != Input.mousePosition.x || tempY != Input.mousePosition.y)
        {
            DeltaX = tempX - (int)Input.mousePosition.x;
            DeltaY = tempY - (int)Input.mousePosition.y;
            tempX = (int)Input.mousePosition.x;
            tempY = (int)Input.mousePosition.y;
            mouseMove = true;
        }

        else
        {
            mouseMove = false;
          
            DeltaY = 0;
            DeltaX = 0;
        }
        if (EnemyShop.EnemyShopToggle == false)
        {
            float horizontal = -(DeltaX) * rotateSpeed;
            target.transform.Rotate(0, horizontal, 0);

            float vertical = -(DeltaY * rotateSpeed);
            angle = VerticalTarget.transform.eulerAngles.y;

            //VerticalTarget.transform.eulerAngles = new Vector3(0, ClampAngle(angle, YMinLimit, YMaxLimit), 0);

            VerticalTarget.transform.Rotate(vertical, 0, 0);


            float desiredAngleY = VerticalTarget.transform.eulerAngles.x;
            float desiredAngleX = target.transform.eulerAngles.y;

            Quaternion rotation = Quaternion.Euler(desiredAngleY, desiredAngleX, 0);

            if (Input.GetKeyDown(KeyCode.T) && camToggleEnable)
            {
                if (keyDown == false)
                {
                    if (camToggle == 0)
                    {
                        camToggle = 1;
                    }
                    else if (camToggle == 1)
                    {
                        camToggle = 2;
                    }
                    else if (camToggle == 2)
                    {
                        camToggle = 0;
                    }
                    keyDown = true;
                }
            }
            else
            {
                keyDown = false;
            }
            transform.position = target.transform.position - (rotation * offset);
            //Camera Toggle
            if (camToggle == 0)
            {
                pivot.transform.localPosition = new Vector3(3.13f, 3.11f, 0.0f);
            }
            else if (camToggle == 1)
            {
                pivot.transform.localPosition = new Vector3(0.0f, 2.2f, -1.74f);
                transform.localPosition = new Vector3(0.0f, 3.34f, -12.79f);
            }
            else if (camToggle == 2)
            {
                pivot.transform.localPosition = new Vector3(0.0f, 2.2f, -1.74f);
                transform.localPosition = new Vector3(0.0f, 2.81f, 18.85f);
            }
            transform.LookAt(pivot.transform);
        }
    }



    //Crosshair placement
    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairTex.width / 2);
        float yMin = (Screen.height / 2) - (crosshairTex.height / 2);
        if (EnemyShop.EnemyShopToggle == false)
        {
            GUI.DrawTexture(new Rect(xMin, yMin, crosshairTex.width, crosshairTex.height), crosshairTex);
        }
    }

    void LateUpdate()
    {
       
       if (Cursor.lockState != CursorLockMode.Confined)
       {
           Cursor.lockState = CursorLockMode.Confined;
       }
    }
}
