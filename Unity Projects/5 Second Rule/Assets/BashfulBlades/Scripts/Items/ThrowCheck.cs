using UnityEngine;
using System.Collections;

public class ThrowCheck : MonoBehaviour 
{
    bool throwable = true;
    void OnTriggerEnter(Collider otherObj)
    {
        if(otherObj.tag != "Player" && otherObj.tag != "pickTo" && otherObj.tag != "ignoreColl")
        {
            throwable = false;
        }
    }
    void OnTriggerExit(Collider otherObj)
    {
        if (otherObj.tag != "Player" && otherObj.tag != "pickTo" && otherObj.tag != "ignoreColl")
        {
            throwable = true;
        }
    }
}
