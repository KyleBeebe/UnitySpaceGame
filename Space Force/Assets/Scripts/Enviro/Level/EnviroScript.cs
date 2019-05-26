using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroScript : MonoBehaviour
{
    public GameObject sphere;
    public bool invert;
    // Start is called before the first frame update
    void Start()
    {
        if (invert)
        {
            invertSphere();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void invertSphere()
    {
        MeshFilter old_filter = sphere.GetComponent<MeshFilter>();
        Mesh old_mesh = old_filter.sharedMesh;

        Vector3[] vertices = old_mesh.vertices;
        Vector3[] normals = old_mesh.normals;
        int[] indices = old_mesh.triangles;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        for (int i = 0; i < indices.Length; i+=3)
        {
            int temp = indices[i];
            indices[i] = indices[i+2 ];
            indices[i + 2] = temp;
        }

       

        old_mesh.vertices = vertices;
        old_mesh.normals = normals;
        old_mesh.triangles = indices;
      

        old_mesh.RecalculateTangents();

        


        sphere.SetActive(false);
    }
}
