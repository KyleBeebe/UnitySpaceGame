using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public GameObject laser_prefab;
    public GameObject firePoint;

    private GameObject spawned_laser;

    // Start is called before the first frame update
    void Start()
    {
        spawned_laser = Instantiate(laser_prefab,firePoint.transform) ;
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaser();
        
       
    }

    public void EnableLaser()
    {
        spawned_laser.SetActive(true);
    }

    public void DisableLaser()
    {
        spawned_laser.SetActive(false);
    }

    void UpdateLaser(){
    
        spawned_laser.transform.position = firePoint.transform.position;
        print(spawned_laser.transform.position);

    }

}
