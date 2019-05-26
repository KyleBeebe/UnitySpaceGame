using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Equipment : MonoBehaviour
{
    static protected GameObject player_body;
    static protected GameObject cam;
    static bool initialized = false;


    //initialize fields if not already done
    protected void Init()
    {
        if (!initialized)
        {
            player_body = GameObject.Find("Body");
            cam = GameObject.Find("View");
            initialized = true;
        }
        
    }

    public virtual void Use()
    {

    }
    

}
