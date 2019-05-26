using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleScript : Equipment
{

    public LayerMask no_grapple;

    private RaycastHit hit_info; 
    private Rigidbody body;

    public float max_dist; // max grapple distance
    public float strength;  // strength of grapple pull 
    
    private Vector3 direction; // grapple direction

    public GameObject line; // line for debugging

    private bool grappling;
    private bool reeling;

    private Vector3 offset; // offset from camera

    GameObject[] rope;

    public GameObject pt0, pt1, pt2, pt3;


    private float prev_dist;
    private float prev_body_dist;

    private Vector3 prev_pos;

    private float total_time; // current total of elapsed grappling time 
    public float max_time = 1.0f; //how long the grapple force is added in seconds

    public GameObject rope_section_prefab;
    private float rope_length;
    private const float DEFAULT_SPRING_MAX = 5.0f;

    private GameObject hook;

   

    //distance from body to grapple hook
    private float dist_to_hook = 0.0f;

    private float dist_temp;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        
        grappling = false;
        
        total_time = 0.0f;
        player_body = GameObject.Find("Body");
        body = player_body.GetComponent<Rigidbody>();
        cam = GameObject.Find("View");
        line.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (grappling)
        {
            //DrawGrapple();

            direction = hook.transform.position - player_body.transform.position;

            updateGrapple2();
            //updateGrapple();
        }
        
        
    }
    private void LateUpdate()
    {
        if (grappling)
        {
            
            prev_body_dist = direction.magnitude;
            prev_pos = transform.position + transform.up * transform.localScale.y * 2;
            //updateGrapple2();
            
        }
    }

    void FixedUpdate()
    {
        if (grappling) // mid grapple
        {
            ///total_time += Time.fixedDeltaTime; // add to grapple time count

            //if (total_time < max_time)
            //{

            //float current_dist = (direction).magnitude;
            // body.AddForce(direction * strength * (current_dist/rope_length) , ForceMode.Impulse);  // rigidbody force towards grappple position
            //}
            
            if (reeling)
            {
                //updateGrapple2();
                if (!Input.GetButton("Fire2"))
                {
                    reeling = false;
                    player_body.GetComponent<Body>().Grappling = false;
                    
                    grappling = false;

                    RemoveGrapple2();
                }
                addImpluse();
            }
            

        }
        else
        {
            
            //total_time = 0.0f;
        }
    }
    

    
    //updates cylinder based grapple rope
    private void updateGrapple()
    {
        Vector3 position = transform.position + transform.up * transform.localScale.y * 2;
        float y_scale = rope_section_prefab.transform.localScale.y * 2;
        Vector3 curr_dir = hit_info.point - position;
        Vector3 unit_direction = curr_dir.normalized * y_scale;

        //rotate rope section to face towards hit point
        Quaternion rot = Quaternion.FromToRotation(rope_section_prefab.transform.up, curr_dir);

        //size of rope section in height
        float height = rope_section_prefab.GetComponent<CapsuleCollider>().height * y_scale;
        int ideal_sections = (int)(curr_dir.magnitude / (height / 2.0f));

        //rope_length = curr_dir.magnitude;

        
        int delta = rope.Length - ideal_sections;
        if (ideal_sections < rope.Length)
        {
            
            GameObject[] temp = new GameObject[ideal_sections];
            ConfigurableJoint joint2 = gameObject.GetComponent<ConfigurableJoint>();
            for (int i = 0; i < rope.Length; i++)
            {
                if (i < (delta))
                {
                    print(i+" "+rope[i].name);
                    Destroy(rope[i]);
                    
                    continue;
                }
                temp[i-delta] = rope[i];
            }
            rope = temp;
            joint2.connectedBody = rope[0].GetComponent<Rigidbody>();
        }
        

        


    }

    //adds thrust from reeling in grapple
    private void addImpluse()
    {
        
        player_body.GetComponent<Rigidbody>().AddForce(direction.normalized * 1.0f  , ForceMode.Impulse);
    }

    //updates line renderer based grapple rope
    private void updateGrapple2()
    {

        Vector3 position = transform.position + transform.up * transform.localScale.y * 2;
        Vector3 curr_dir = hit_info.point - position;
        float dist = curr_dir.magnitude;

        //if (Mathf.Abs(prev_body_dist - direction.magnitude) > ((1.0f / 3) * prev_body_dist))
        {

            //Vector3 p0_to_p1 = rope[1].transform.position - position;
            //Vector3 p0_to_p3 = rope[3].transform.position - position;
            //Vector3 p0_to_p2 = rope[2].transform.position - position;

            //Vector3 unit = p0_to_p3.normalized;
            //Vector3 p1_on_unit = (unit * Vector3.Dot(p0_to_p1, unit));
            //Vector3 offset1 = p0_to_p1 - p1_on_unit;

            //Vector3 p2_on_unit = unit * Vector3.Dot(p0_to_p2, unit);
            //Vector3 offset2 = p0_to_p2 - p2_on_unit;


            Vector3 unit_dir = curr_dir.normalized;
            float section = dist / 3.0f;
            float delta = dist_temp - direction.magnitude;
            float delta2 = Mathf.Abs(dist_temp - direction.magnitude);
            
            for (int i = 0; i < 3; i++)
            {

                //if ( (direction.magnitude <= (prev_body_dist )) || prev_body_dist == 0.0f)
                //if ( (delta < 0.5f && delta >=0.0f)|| prev_body_dist == 0.0f)
                if (( delta2 < 0.1f) || prev_body_dist == 0.0f)
                {

                    

                    Destroy(rope[i].GetComponent<ConfigurableJoint>());
                    rope[i].transform.position = (position + (i) * section * unit_dir);
                    //Destroy(rope[i].GetComponent<ConfigurableJoint>());
                    ConfigurableJoint joint2 = rope[i].AddComponent<ConfigurableJoint>();
                    joint2.xMotion = ConfigurableJointMotion.Locked;
                    joint2.yMotion = ConfigurableJointMotion.Locked;
                    joint2.zMotion = ConfigurableJointMotion.Locked;

                    SoftJointLimit softJointLimit2 = new SoftJointLimit();
                    softJointLimit2.limit = 0.05f;
                    joint2.linearLimit = softJointLimit2;

                    SoftJointLimitSpring softJointLimitSpring2 = new SoftJointLimitSpring();
                    softJointLimitSpring2.spring = 0f;
                    softJointLimitSpring2.damper = 5;

                    joint2.linearLimitSpring = softJointLimitSpring2;


                    joint2.connectedBody = rope[i + 1].GetComponent<Rigidbody>();
                    
                    break;
                }
                dist_temp = direction.magnitude;
                rope[i].transform.position = (position + (i) * section * unit_dir);

                Destroy(rope[i].GetComponent<ConfigurableJoint>());

                

                ConfigurableJoint joint = rope[i].AddComponent<ConfigurableJoint>();
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;

                SoftJointLimit softJointLimit = new SoftJointLimit();
                softJointLimit.limit = 0.05f;
                joint.linearLimit = softJointLimit;

                SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
                softJointLimitSpring.spring = 0f;
                softJointLimitSpring.damper = 5;
                joint.linearLimitSpring = softJointLimitSpring;


                joint.connectedBody = rope[i+1].GetComponent<Rigidbody>();
                // connect joint from gun to first link
                if (i == 0)
                {
                    Destroy(gameObject.GetComponent<ConfigurableJoint>());
                    ConfigurableJoint joint2 = gameObject.AddComponent<ConfigurableJoint>();
                    joint2.xMotion = ConfigurableJointMotion.Locked;
                    joint2.yMotion = ConfigurableJointMotion.Locked;
                    joint2.zMotion = ConfigurableJointMotion.Locked;


                    SoftJointLimit softJointLimit2 = new SoftJointLimit();
                    softJointLimit2.limit = 0.05f;
                    joint2.linearLimit = softJointLimit2;

                    SoftJointLimitSpring softJointLimitSpring2 = new SoftJointLimitSpring();
                    softJointLimitSpring2.spring = 0f;
                    softJointLimitSpring2.damper = 5;
                    joint2.linearLimitSpring = softJointLimitSpring2;


                    joint2.connectedBody = rope[i].GetComponent<Rigidbody>();
                    rope[1].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    rope[2].GetComponent<Rigidbody>().velocity = Vector3.zero;
                }


            }
            
            prev_body_dist = direction.magnitude;
        }
        
        
        
    }


    // Use equipment
    public override void Use()
    {
        print("grapple used");

        if (grappling)
        {
            //updateGrapple2();
            if (Input.GetButton("Fire2"))
            {
                reeling = true;
                return;
            }
            player_body.GetComponent<Body>().Grappling = false;
            print("done grapple");
            grappling = false;

            RemoveGrapple2();
        }

        // raycast to grapple point, ignore trigger colliders
        else if (!grappling && Physics.Raycast(cam.transform.position, cam.transform.forward,
            out hit_info, max_dist, ~no_grapple, QueryTriggerInteraction.Ignore))
        {

            line.SetActive(true);
            grappling = true;
            direction = (hit_info.point - body.transform.position).normalized;
            DrawHook(); // draw hook end of grapple
            //DrawGrappleRope(); // draw rope of grapple
            DrawGrappleRope2(); // draw rope of grapple
            
        }

    }
    
    //despawn cylinder based grapple rope
    void RemoveGrapple()
    {
        //destroy the rope objects
        foreach (GameObject go in rope)
        {
            Destroy(go);
        }
        Destroy(hook);
    }
    
    //despawn line renderer based grapple rope
    void RemoveGrapple2()
    {
        line.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            Destroy(rope[i].GetComponent<ConfigurableJoint>());
        }
        Destroy(hook);
    }


    // attach grapple hook onto wall and joint between it and player
    void DrawHook()
    {

        //create hook, attach it to raycast hit point, add configurable
        //joint between hook and player's rigid body
        hook = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hook.transform.position = hit_info.point;
        hook.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        Rigidbody rb = hook.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        dist_to_hook = (hook.transform.position - player_body.transform.position).magnitude ;

        
        ConfigurableJoint joint = player_body.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        joint.connectedAnchor = Vector3.zero;
        joint.anchor  = Vector3.zero;

      

        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = dist_to_hook;
        joint.linearLimit = softJointLimit;

        SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
        softJointLimitSpring.spring = 1000f;
        softJointLimitSpring.damper = 200f;

        joint.linearLimitSpring = softJointLimitSpring;
        joint.connectedBody = rb;
    }

    // draw line using bezier curve line renderer as grapple rope
    void DrawGrappleRope2()
    {
        Vector3 position = transform.position + transform.up * transform.localScale.y * 2;
        Vector3 direction = hit_info.point - position;

        float dist = direction.magnitude;
        //print("prev "+dist);
        prev_dist = dist;

        Vector3 unit_dir = direction.normalized;

        float section = dist / 3;

        rope = new GameObject[4] {pt0,pt1,pt2,pt3 };

        for (int i = 0; i < 4; i++)
        {
            rope[i].transform.position = position + (i) * section * unit_dir;


            ConfigurableJoint joint = rope[i].AddComponent<ConfigurableJoint>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0.001f;
            joint.linearLimit = softJointLimit;

            SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
            softJointLimitSpring.spring = 0f;

            joint.linearLimitSpring = softJointLimitSpring;



            //connect last rope section to hook

            if (i == 3)
            {
                rope[i].GetComponent<ConfigurableJoint>().connectedBody = hook.GetComponent<Rigidbody>();
            }

            // connect joint from gun to first link
            if (i == 0)
            {

                ConfigurableJoint joint2 = gameObject.AddComponent<ConfigurableJoint>();
                joint2.xMotion = ConfigurableJointMotion.Locked;
                joint2.yMotion = ConfigurableJointMotion.Locked;
                joint2.zMotion = ConfigurableJointMotion.Locked;


                SoftJointLimit softJointLimit2 = new SoftJointLimit();
                softJointLimit2.limit = 0.001f;
                joint.linearLimit = softJointLimit2;

                SoftJointLimitSpring softJointLimitSpring2 = new SoftJointLimitSpring();
                softJointLimitSpring2.spring = 0f;
                //softJointLimitSpring.damper = 5;
                joint2.linearLimitSpring = softJointLimitSpring2;


                joint2.connectedBody = rope[i].GetComponent<Rigidbody>();
            }

            if (i > 0 )
            {
                rope[i - 1].GetComponent<ConfigurableJoint>().connectedBody = rope[i].GetComponent<Rigidbody>();

            }


        }
 

    }

    // draw a line using cylinder objects as grapple rope
    void DrawGrappleRope()
    {

        Vector3 position = transform.position + transform.up * transform.localScale.y * 2;
        float y_scale = rope_section_prefab.transform.localScale.y * 2;
        Vector3 direction = hit_info.point - position;
        Vector3 unit_direction = direction.normalized * y_scale;

        //rotate rope section to face towards hit point
        Quaternion rot = Quaternion.FromToRotation(rope_section_prefab.transform.up, direction);
        
        //size of rope section in height
        float height = rope_section_prefab.GetComponent<CapsuleCollider>().height * y_scale;
        int sections = (int)(direction.magnitude / (height/2));
        
        rope_length = direction.magnitude;

        

        rope = new GameObject[sections];
        
        //create each rope section
        for (int i = 0; i < sections; i++)
        {
            Vector3 position_next = position + i * unit_direction;
            GameObject go = Instantiate(rope_section_prefab, position_next,rot ) as GameObject;
            go.name = i.ToString();
            // create joint for each rope section
            
                ConfigurableJoint joint = go.AddComponent<ConfigurableJoint>();
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;


           

            SoftJointLimit softJointLimit = new SoftJointLimit();
                softJointLimit.limit = 0.001f;
                joint.linearLimit = softJointLimit;

            

                SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
                softJointLimitSpring.spring = 0f;
                
                joint.linearLimitSpring = softJointLimitSpring;
                

            
            //connect last rope section to hook
            
            if (i == sections - 1)
            {
                go.GetComponent<ConfigurableJoint>().connectedBody = hook.GetComponent<Rigidbody>();
            }

            // connect joint from gun to first link
            if (i == 0) 
            {

                ConfigurableJoint joint2 = gameObject.AddComponent<ConfigurableJoint>();
                joint2.xMotion = ConfigurableJointMotion.Locked;
                joint2.yMotion = ConfigurableJointMotion.Locked;
                joint2.zMotion = ConfigurableJointMotion.Locked;

               

                SoftJointLimit softJointLimit2 = new SoftJointLimit();
                softJointLimit2.limit = 0.001f;
                joint.linearLimit = softJointLimit2;

                SoftJointLimitSpring softJointLimitSpring2 = new SoftJointLimitSpring();
                softJointLimitSpring2.spring = 0f;
                //softJointLimitSpring.damper = 5;
                joint2.linearLimitSpring = softJointLimitSpring2;
                

                joint2.connectedBody = go.GetComponent<Rigidbody>();
            }

             if (i > 0 && sections >1)
            {
                rope[i - 1].GetComponent<ConfigurableJoint>().connectedBody = go.GetComponent<Rigidbody>();
                
            }

            
            
            rope[i] = go;
            
        }

    }

    // get position of hook
    public Vector3 HookPos()
    {
        return hook.transform.position;
    }


}
