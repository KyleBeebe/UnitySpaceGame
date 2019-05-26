using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//super class to damagable tiles and etc
public class Damagable : MonoBehaviour
{
    public bool broken;
    public int damage_speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        broken = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Damage() {

    }
}
