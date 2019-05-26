using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollingDroidController : Enemy
{
    NavMeshAgent agent;
    Animator anim;
    Vector3 home;
    float targetDistance=float.MaxValue;

    public Transform target;
    public float attackRadius;
    public float wakeRadius;
    public float hitPoints;
    public float damage;
    public float knockback;
    public float lift;

    public float walkingSpeed;
    public float chargingSpeed;
    public float chargingAngularSpeed;
    public float accelerationLerp;

    bool phasing;

    bool standing;
    bool walking;
    bool rolling;
    bool lost;

    bool alive;

    ParticleSystem particleLauncher;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        home = transform.position;
        agent.speed = 0;
        alive = true;
        standing = walking = rolling = lost = phasing = false;
        particleLauncher = GetComponentInChildren<ParticleSystem>();
        particleLauncher.Stop();
    }

    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Open_Anim", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            agent.SetDestination(target.position);

            if (phasing)
            {
                // Do nothing.  This keeps Coroutines from being cloned multiple times.
            }
            else if (!lost && targetDistance <= wakeRadius)
            { // get active
                if (!standing) // fully seated
                {
                    // animation to stand up
                    anim.SetBool("Open_Anim", true);
                    // assign new state after delay (animation must finish)
                    StartCoroutine("PowerOn");
                }
                else if (!walking) // ready to move
                {
                    // animation to walking
                    anim.SetBool("Walk_Anim", true);
                    // assign new state with no delay
                    agent.speed = walkingSpeed;
                    walking = true;
                }
                else if (targetDistance <= attackRadius) // time to book it
                {
                    if (!rolling) // roll out
                    {
                        anim.SetBool("Walk_Anim", false);
                        anim.SetBool("Roll_Anim", true);
                        StartCoroutine("StartRolling");
                    }
                    else
                    {
                        // keep on rolling
                    }
                }
            }
            else // chill out
            {
                if (rolling)
                {
                    anim.SetBool("Walk_Anim", false);
                    anim.SetBool("Roll_Anim", false);
                    agent.speed = 0;
                    StartCoroutine("StopRolling");

                }
                else if (walking)
                {
                    anim.SetBool("Walk_Anim", false);
                    walking = false;
                    agent.speed = 0;
                    lost = true;
                }
                else if (lost)
                {
                    agent.speed = 0;
                    StartCoroutine("LookAround");
                }
                else if (standing)
                {
                    anim.SetBool("Open_Anim", false);
                    StartCoroutine("PowerOff");
                }
            }
            CheckDeath();
        }
        
    }

    private void FixedUpdate()
    {
        targetDistance = Vector3.Distance(transform.position, target.position);
    }

    IEnumerator PowerOn()
    {
        phasing = true;
        yield return new WaitForSeconds(7f);
        standing = true;
        phasing = false;
    }

    IEnumerator PowerOff()
    {
        phasing = true;
        yield return new WaitForSeconds(.81f);
        standing = false;
        phasing = false;
    }

    IEnumerator LookAround()
    {
        phasing = true;
        yield return new WaitForSeconds(3.3f);
        lost = false;
        phasing = false;
    }

    IEnumerator StartRolling()
    {
        phasing = true;
        yield return new WaitForSeconds(2.5f);
        agent.speed = chargingSpeed;
        rolling = true;
        phasing = false;
    }

    IEnumerator StopRolling()
    {
        phasing = true;
        yield return new WaitForSeconds(2.5f);
        rolling = false;
        lost = true;
        phasing = false;
    }

    IEnumerator BTFO()
    {
        yield return new WaitForSeconds(5f);
        particleLauncher.Stop();
        agent.speed = 0;
    }

    void CheckDeath()
    {

        if (health <= 0.0f && alive)
        {

            alive = false;
            particleLauncher.Play();
            anim.SetBool("Open_Anim", false);
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Roll_Anim", false);
            StartCoroutine("BTFO");
        }
    }
    void CheckKey()
    {
        // Walk
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("Walk_Anim", false);
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim.GetBool("Roll_Anim"))
            {
                anim.SetBool("Roll_Anim", false);
            }
            else
            {
                anim.SetBool("Roll_Anim", true);
            }
        }

        // Close
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!anim.GetBool("Open_Anim"))
            {
                anim.SetBool("Open_Anim", true);
            }
            else
            {
                anim.SetBool("Open_Anim", false);
            }
        }
    }//CheckKey()
}
