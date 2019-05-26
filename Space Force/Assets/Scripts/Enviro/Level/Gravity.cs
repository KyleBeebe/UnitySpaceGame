using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Vector3 gravity;
    // Start is called before the first frame update
    void Start()
    {
        
        gravity = (-transform.parent.transform.up) * 9.8f;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(gravity, ForceMode.Acceleration);
        
    }
}
