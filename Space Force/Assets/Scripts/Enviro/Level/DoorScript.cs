using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Open") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Open 0")  )
        {
            anim.Play("Door_Open");
        }
        
       
    }
}
