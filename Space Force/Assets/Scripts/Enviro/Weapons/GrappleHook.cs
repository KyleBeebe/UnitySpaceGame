using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public Vector3 direction;
    public float max_dist;
    private float curr_dist = 0.0f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(.1f, .1f, .1f);

        //gameObject.GetComponent<Collider>().isTrigger = true;
        
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        
        //sphere.GetComponent<HingeJoint>().connectedBody
        
    }

   
    void FixedUpdate()
    {
        rb.AddRelativeForce(direction , ForceMode.VelocityChange);

    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        
    }
    public void SetMaxDist(float dist)
    {
        max_dist = dist;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        ContactPoint contact = collision.contacts[0];
        rb.isKinematic = true;

        transform.position = contact.point;
        HingeJoint hj = gameObject.AddComponent<HingeJoint>();
        hj.connectedBody = collision.rigidbody;
        hj.connectedAnchor = contact.point;


    }
}
