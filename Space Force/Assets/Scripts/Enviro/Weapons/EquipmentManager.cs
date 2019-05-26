using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    Equipment[] equipment;

    private Equipment primary;
    private Equipment secondary;

    
    public GameObject shot;

    // Start is called before the first frame update
    void Start()
    {
        equipment = gameObject.GetComponentsInChildren<Equipment>(true);
        primary = equipment[1];
        secondary = equipment[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // use assigned primary weapon
    public void UsePrimary()
    {
       
        
        primary.Use();
    }
    // use assigned secondary weapon(grapple)
    public void UseSecondary()
    {
        secondary.Use();
    }
}
