using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//super class for enemy objects
public class Enemy : MonoBehaviour
{
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float dmg)
    {
        
        health -= dmg;
    }

    
}
