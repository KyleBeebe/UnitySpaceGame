using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmos_rotate : MonoBehaviour
{
    public float rot_speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spin = new Vector3(0.0f, rot_speed * Time.deltaTime,0.0f);
        gameObject.transform.Rotate(spin);
    }
}
