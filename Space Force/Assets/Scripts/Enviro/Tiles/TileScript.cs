using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : Damagable
{
    SkinnedMeshRenderer skin;
    private float size = 0; // percent of animation done

    // Start is called before the first frame update
    void Start()
    {
        
        skin = GetComponent<SkinnedMeshRenderer>();
        
        skin.SetBlendShapeWeight(0, 0 );
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // tell tile to damage itself
    public override void Damage()
    {
        if (!broken && size < 100.0f)
        {
            print("tile hit");
            
            size += 20.0f;
            size = Mathf.Min(100.0f,size);
            //adds to blend shape weight. 
            skin.SetBlendShapeWeight(0,size);
            
        }
        else
        {
            
            broken = true;
        }
    }

}
