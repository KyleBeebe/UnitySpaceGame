using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdianParticleController : MonoBehaviour
{
    public ParticleSystem particleLauncher;

    List<ParticleCollisionEvent> collisionEvents;
    CharacterHealth health;

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        health = GameObject.FindWithTag("Player").GetComponent<CharacterHealth>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);


        foreach(ParticleCollisionEvent curr in collisionEvents){
            if(other.tag == "Player")
            {
                health.DealDamage(0.01f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
