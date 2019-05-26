using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Equipment
{
    public GameObject laser;
    public GameObject gun_tip;

    //seconds delay between shots
    public float shot_delay;
    // shot delay timer
    private float current_time = 0.0f;
    private bool fired = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            current_time += Time.deltaTime;
        }
        //reset timer and fired if delay timer exceeds delay
        if (current_time >= shot_delay)
        {
            fired = false;
            current_time = 0.0f;
        }
    }
    //uses current gun
    public override void Use()
    {
        if (!fired)
        {
            GetComponent<AudioSource>().Play(); // Play the lazer fire sound.
            GameObject laser_clone = (GameObject)Instantiate(laser, gun_tip.transform.position, transform.rotation);
            fired = true;
        }

    } 

   
}
