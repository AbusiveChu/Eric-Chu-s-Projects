using UnityEngine;
using System.Collections;

public class ControllerAim : MonoBehaviour 
{
    public GameObject target;
    public GameObject pivot;
    public GameObject VerticalTarget;
    public float rotateSpeed = 5;

    [Tooltip("Crosshair texture")]
    public Texture2D crosshairTex;

    float YMinLimit = 271.0f;
    float YMaxLimit = 330.0f;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);
       
        float vertical = -(Input.GetAxis("Mouse Y") * rotateSpeed);
        if (Shooting.AimSlow)
        {
            rotateSpeed = 3.8f;
        }
        else
        {
            rotateSpeed = 5.0f;
        }

   
        if (VerticalTarget.transform.eulerAngles.x < YMinLimit)
        {
            VerticalTarget.transform.eulerAngles.Set(YMinLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
        }
        else if (VerticalTarget.transform.eulerAngles.x > YMaxLimit)
        {
            VerticalTarget.transform.eulerAngles.Set(YMaxLimit, VerticalTarget.transform.eulerAngles.y, VerticalTarget.transform.eulerAngles.z);
        }

        VerticalTarget.transform.Rotate(vertical, 0, 0);

        float desiredAngleY = VerticalTarget.transform.eulerAngles.x;
        float desiredAngleX = target.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(desiredAngleY, desiredAngleX, 0);

        transform.position = target.transform.position - (rotation * offset);
        transform.LookAt(pivot.transform);
    }
    //Crosshair placement
    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairTex.width / 2);
        float yMin = (Screen.height / 2) - (crosshairTex.height / 2);

        GUI.DrawTexture(new Rect(xMin, yMin - 12, crosshairTex.width, crosshairTex.height - 12), crosshairTex);
    }
}
