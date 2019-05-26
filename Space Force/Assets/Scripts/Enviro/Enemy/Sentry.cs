using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    public bool player_entered;
    GameObject player;

    public GameObject pivot;
    public GameObject lens;
    private bool alive;

    public Material laser_mat;
    

    // targetinglaser beam
    LineRenderer laser;

    // default looking forward view of sentry
    private Quaternion default_orientation;

    // turning rate of sentry
    private float turning_rate = 50f;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Body");
        default_orientation = pivot.transform.rotation;
        laser = gameObject.AddComponent<LineRenderer>();
        laser.material = laser_mat;
        laser.startColor = Color.red;
        laser.endColor = Color.red;
        laser.positionCount = 2;
        laser.widthMultiplier = 0.01f;
        alive = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            
            CheckDeath();
            //player enters the trigger collider
            if (player_entered)
            {

                Aim();
            }
            else
            {
                // return orientation to default if player cannot be seen
                pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation, default_orientation,
                    turning_rate * Time.deltaTime);
                laser.positionCount = 0;
                GetComponent<AudioSource>().Stop(); // Stop the sound effect.
            }

        }
        else
        {
            Quaternion new_rot = Quaternion.FromToRotation(pivot.transform.up, -Vector3.up) * pivot.transform.rotation;
            pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation,
                new_rot, turning_rate * Time.deltaTime);
        }
        
    }

    
    public void SetPlayerEnter(bool enter)
    {
        
        player_entered = enter;
    }

    //raycast for visibility of player, aim if ray hits.
    public void Aim()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }

        RaycastHit hit_info;
        float radius = GetComponent<SphereCollider>().radius;
        Vector3 direction = (player.transform.position - transform.position);

        // debug lines
        //Debug.DrawLine(transform.position, transform.position + transform.right * 5, Color.red);
        //Debug.DrawLine(transform.position, transform.position + transform.up * 5, Color.green);
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 5, Color.blue);

        // raycast towards player
        if (Physics.Raycast(transform.position , direction , out hit_info, radius)){
            print("ray hit");
            print(hit_info.collider.tag);
            // ray hits player collider
            if (hit_info.collider.tag == "Player")
            {
                
                // create new rotation towards player
                Quaternion new_rot = Quaternion.FromToRotation(pivot.transform.up, direction) * pivot.transform.rotation;
                pivot.transform.rotation = Quaternion.RotateTowards(pivot.transform.rotation,
                    new_rot, turning_rate * Time.deltaTime);
                laser.positionCount = 2;
                // create start and end point for (laser) line renderer
                laser.SetPosition(0, lens.transform.position);
                laser.SetPosition(1, player.transform.position - new Vector3(0.0f, 0.25f, 0.0f));

                CharacterHealth health = GameObject.FindWithTag("Player").GetComponent<CharacterHealth>();

                Debug.Log("Dealing damage to player");
                health.DealDamage(0.1f);

                Debug.Log("Damage dealt");
            }
        }
        
    }
    void CheckDeath()
    {

        if (health <= 0.0f && alive)
        {
            
            alive = false;
            GetComponentInChildren<ParticleSystem>().Play();
            laser.enabled = false;
        }
    }


}
