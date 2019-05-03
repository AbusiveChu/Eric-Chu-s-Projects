using UnityEngine;
using System.Collections;

public class RemoteAnimate : MonoBehaviour
{

    public Animator anim;
    public Transform Character;
    public float WatcherX;
    public float RotY;
    public bool Left = false, Right = false, Up = false, Down = false, Attack = false;
    public float WatcherZ;
    // Use this for initialization
    void Start()
    {
        WatcherX = Character.transform.position.x;
        WatcherZ = Character.transform.position.z;
    }
    //Z//DOWN IS NEG // UP IS POS
    //X//LEFT IS NEG // RIGHT IS POS
    // Update is called once per frame
    void Update()
    {
        if(Character.transform.position.x == WatcherX && Character.transform.position.z == WatcherZ)
        {
            anim.SetBool("Forward", false);
            anim.SetBool("Idle", true);
        }
        //X
        if (Character.transform.position.x > WatcherX)
        {
            anim.SetBool("Idle", false);
            WatcherX = Character.transform.position.x;
            anim.SetBool("Forward", true);
            Right = true;
        }
        else if (Character.transform.position.x < WatcherX)
        {
            anim.SetBool("Idle", false);
            WatcherX = Character.transform.position.x;
            anim.SetBool("Forward", true);
            Left = true;
        }
        else if (Character.transform.position.x == WatcherX)
        {
            Left = false;
            Right = false;
        }
        //Z
        if (Character.transform.position.z > WatcherZ)
        {
            anim.SetBool("Idle", false);
            WatcherZ = Character.transform.position.z;
            anim.SetBool("Forward", true);
            Up = true;
        }
        else if (Character.transform.position.z < WatcherZ)
        {
            anim.SetBool("Idle", false);
            WatcherZ = Character.transform.position.z;
            anim.SetBool("Forward", true);
            Down = true;
        }
        else if (Character.transform.position.z == WatcherZ)
        {            
            Up = false;
            Down = false;
        }
        if (Character.transform.position.x == WatcherX)
        {
            Left = false;
            Right = false;
        }

        

        BoolCheck();
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
}
