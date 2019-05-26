using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    private Rigidbody rb;
    public float move_speed;
    public float rotate_speed;
    public float jump_force;

    private bool jumping;

    private float height = 3.0f;

    public LayerMask ground;

    private float ground_theta;
    private Quaternion m_MyQuaternion;

    RaycastHit hit_info;
    private bool grounded;

    private GameObject player;

    private GameObject view;

    private Vector3 anchor;

    private bool grappling = false;

    private float tether_dist;

    public GameObject reset;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //freeze all rigidbody rotations
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        jumping = false;
        m_MyQuaternion = new Quaternion();
        m_MyQuaternion = Quaternion.identity;
       
        player = GameObject.Find("Player");
        view = GameObject.Find("View");
    }

    // Update is called once per frame
    void Update()
    {
       
        GroundCheck();   
        DebugLine();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }


    // move player 
    public void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // move player 
        float move_vertical = vertical * move_speed * Time.deltaTime;
        float move_horizontal = horizontal * move_speed * Time.deltaTime;


        Vector3 vert = transform.forward * move_vertical;
        Vector3 horiz = transform.right * move_horizontal;

       
        rb.MovePosition(transform.position + vert + horiz);
        
        
    }

    public bool Grappling{
        set { grappling = value; }
        get { return grappling; }
    }
    public float Tether_Dist
    {
        set { tether_dist = value; }
    }
    public Vector3 Anchor
    {
        set { anchor = value; }
    }

    // make player jump
    public void Jump()
    {
        
        if (!jumping)
        {
            jumping = true;
            rb.AddForce(transform.up * jump_force, ForceMode.Impulse);
            
        }
        
    }

    // rotate player body
    void Rotate()
    {
        transform.rotation = m_MyQuaternion * transform.rotation;
        m_MyQuaternion = Quaternion.identity;
    }

    

    

    // ray cast to check if on ground
    void GroundCheck()
    {
        Debug.DrawLine(transform.position, transform.position - transform.up * height, Color.blue);
        if (Physics.Raycast(transform.position, -transform.up,out hit_info,height,ground))
        {

            
            jumping = false;
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //tell door object to open
        if (other.tag == "door")
        {
            other.GetComponent<DoorScript>().Open();
            
        }
        //tell player to handle cylinder level physics
        if (other.tag == "Centrifuge")
        {
            
            handleCentrifugePhysics(true);
        }

        if (other.tag == "Sentry")
        {
            // tell sentry that player entered trigger
            other.GetComponent<Sentry>().SetPlayerEnter(true);

        }
        // player falls to death, reset to hub area
        if (other.tag == "Fall_Death")
        {

            Reset();
           

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            view.GetComponentInParent<CharacterHealth>().DealDamage(5); 
        }
    }
    //reset player to starting area
    public void Reset()
    {
        rb.velocity = Vector3.zero;
        transform.position = reset.transform.position;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sentry")
        {
            // tell sentry that player entered trigger
            other.GetComponent<Sentry>().SetPlayerEnter(false);
        }
        if (other.tag == "Centrifuge")
        {
            print("trigger centri");
            handleCentrifugePhysics(false);
            Physics.gravity = new Vector3(0.0f,-9.8f,0.0f);
        }
    }
    // handle collisions with enemy particle weapons
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject.FindWithTag("Player").GetComponent<CharacterHealth>().DealDamage(1);
        }
    }



    // when centrifuge trigg entered, signal camera to handle uprighting 
    private void handleCentrifugePhysics(bool handle)
    {
        view.GetComponent<Cam>().handleCentrifuge(handle);
    }


    void DebugLine()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * height * 2, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.up * height , Color.red);
    }
}