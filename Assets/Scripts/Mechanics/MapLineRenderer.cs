using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLineRenderer : MonoBehaviour
{
     public Transform[] waypoints;
    public int curveSegments = 10; // Number of segments to create the curve

    void Start()
    {
        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // Calculate the number of points needed for the curve
        int numPoints = (waypoints.Length - 1) * curveSegments + 1;

        lineRenderer.positionCount = numPoints;

        // Loop through each segment of the curve
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            // Calculate control points for the Bezier curve
            Vector3 p0 = waypoints[i].position;
            Vector3 p1 = waypoints[i].position + (waypoints[i + 1].position - waypoints[i].position) / 2f;
            Vector3 p2 = waypoints[i + 1].position;

            // Loop through each segment of the curve
            for (int j = 0; j < curveSegments; j++)
            {
                float t = j / (float)curveSegments;

                // Calculate position on the Bezier curve
                Vector3 position = CalculateBezierPoint(t, p0, p1, p2);

                // Set the position in the line renderer
                int index = i * curveSegments + j;
                lineRenderer.SetPosition(index, position);
            }
        }

        // Set the last point to the last waypoint
        lineRenderer.SetPosition(numPoints - 1, waypoints[waypoints.Length - 1].position);
    }

    // Calculate a point on the Bezier curve given t
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p2;

        return p;
    }
}