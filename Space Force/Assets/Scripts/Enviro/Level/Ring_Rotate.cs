﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring_Rotate : MonoBehaviour
{
    public int direction;
    public float speed;
    public Vector3 axis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed * direction*Time.deltaTime );
    }
}
