using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass_Guard : MonoBehaviour
{
    public GameObject broken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void Break()
    {
        broken.SetActive(true);
        Destroy(gameObject);
    }
}
