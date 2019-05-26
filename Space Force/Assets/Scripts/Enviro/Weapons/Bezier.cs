using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{

    public LineRenderer line;
    public Transform pt0, pt1, pt2, pt3;

    private const int total_points = 50;

    public Material rope_mat;

    // Start is called before the first frame update
    void Start()
    {
        
        line.positionCount = total_points;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = rope_mat;
        CubicCurve();
        
    }
    // turns line renderer on or off via setting number of points
    public void Visibility(bool visible)
    {
        if (visible)
        {
            line.positionCount = total_points;
        }
        else
        {
            line.positionCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CubicCurve();
    }

    // calculate  position on cubic curve for all points and inserts into line renderer
    private void CubicCurve()
    {
        for (int i = 0; i < total_points; i++)
        {
            float t = i / (float)total_points;
            line.SetPosition(i,CalcCubicPoint(t,pt0.position,pt1.position,pt2.position, pt3.position));
        }
    }
    //calculate position on cubic curve at time t for the given 4 control points
    private Vector3 CalcCubicPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;
        Vector3 p = u3 * p0;
        p += 3 * u2 * t * p1;
        p += 3 * u * t2 * p2;
        p += t3 * p3;
        return p;
    }
    
}
