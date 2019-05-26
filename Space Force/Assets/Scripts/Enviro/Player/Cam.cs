using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private GameObject player_body;
    public float turn_speed = 2.0f; // turning speed

    Vector2 yaw_pitch;
    Vector2 smoothVec;

    public float sens = 3.0f;
    public float smoothing = 3.0f;

   

    private float clamp_angle = 85; // angle to clamp pitch rotation

    public Vector3 offset;

    bool handle_centrifuge = false;
    public GameObject core;

    float time = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        player_body = GameObject.Find("Body");
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        transform.position = player_body.transform.position ;
    }
    private void FixedUpdate()
    {
        
        DebugLine();
        
    }
    private void LateUpdate()
    {
       
    }

    // camera rotation 
    public void Rotate()
    {
        
        Vector2 look = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        look = Vector2.Scale(look, new Vector2(sens * smoothing, sens * smoothing));

        //smoothly interpolate look rotation angles 
        smoothVec.x = Mathf.Lerp(smoothVec.x, look.x, 1.0f / smoothing );
        smoothVec.y = Mathf.Lerp(smoothVec.y, look.y, 1.0f / smoothing  );

        yaw_pitch += smoothVec;
        
        // clamp pitch angle 
        yaw_pitch.y = Mathf.Clamp(yaw_pitch.y, -clamp_angle, clamp_angle);

        // pitch rotation quaternion
        Quaternion pitch_rot = Quaternion.AngleAxis(-yaw_pitch.y, Vector3.right);

        // yaw rotation quaternion
        transform.rotation = Quaternion.AngleAxis(yaw_pitch.x, Vector3.up) * pitch_rot;

        //set new concatenated rotation
        player_body.transform.rotation =
             Quaternion.AngleAxis(yaw_pitch.x, Vector3.up);

        if (handle_centrifuge) // if currently in centrifuge
        {
            
            uprightPlayer();

        }

        if (Time.timeScale == 0)
        {

        }


    }
    

    public void handleCentrifuge(bool handle)
    {
        handle_centrifuge = handle ;

       
    }
    // keep player upright while in centrifuge
    private void uprightPlayer()
    {
        Vector3 to_me = (transform.position - core.transform.position);

        Vector3 next = Vector3.Dot(to_me, core.transform.right) * core.transform.right;

        Vector3 closest = core.transform.position + next;
        Debug.DrawLine(Vector3.zero, core.transform.position  + next, Color.yellow);
        
        Vector3 me_to_closest = closest - transform.position;

        Quaternion new_rot = Quaternion.FromToRotation(player_body.transform.up, me_to_closest);

        Quaternion new_body = new_rot * player_body.transform.rotation;
        Quaternion new_cam = new_rot * transform.rotation;

        player_body.transform.rotation = Quaternion.Slerp(player_body.transform.rotation, new_body,time);
        transform.rotation = Quaternion.Slerp(transform.rotation, new_cam, time);

        Physics.gravity = -player_body.transform.up * 9.8f;

        time += Time.deltaTime;
    }

    // debug lines showing forward and right vectors
    void DebugLine()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward  * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right * 2, Color.cyan);
    }
}
