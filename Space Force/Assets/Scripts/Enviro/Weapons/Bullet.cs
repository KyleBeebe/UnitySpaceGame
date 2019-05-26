using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        // Control how fast and what direction our laser bolt from the ship
        // will travel.
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

    }


    void OnTriggerEnter(Collider other)
    {
        
    }

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }

    
}
