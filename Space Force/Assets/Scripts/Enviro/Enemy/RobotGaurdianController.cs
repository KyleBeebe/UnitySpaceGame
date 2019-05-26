using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGaurdianController : Enemy
{

    public Transform target;
    public float wakeRadius;
    public float moveSpeed;

    ParticleSystem[] particleLauncher;
    RaycastHit hit_info;
    Rigidbody rb;
    bool alive;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        particleLauncher = GetComponentsInChildren<ParticleSystem>();
        particleLauncher[0].Stop();
        particleLauncher[1].Stop();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            transform.LookAt(target.position);
            Vector3 dir = target.position - transform.position;
            if (Vector3.Distance(target.position, transform.position) <= wakeRadius)
            {
                Physics.Raycast(transform.position, dir, out hit_info, wakeRadius);
                if (hit_info.collider.tag == "Player")
                {
                    print("ray hit");
                    print(hit_info.collider.tag);

                    rb.velocity = dir.normalized * moveSpeed;
                    if (!particleLauncher[0].isPlaying)
                    {
                        particleLauncher[0].Play();
                    }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    particleLauncher[0].Stop();
                }
            }
            CheckDeath();
            
        }
    }

    void CheckDeath()
    {

        if (health <= 0.0f && alive)
        {

            alive = false;
            particleLauncher[0].Stop();
            particleLauncher[1].Emit(50);
            rb.useGravity = true;
        }
    }
}
