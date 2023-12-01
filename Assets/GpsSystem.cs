using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GpsSystem : MonoBehaviour
{
  public Transform target1;
    public Transform target2;
    public float waypointSpacing = 1f; // Spacing between waypoints on the LineRenderer
    private LineRenderer lineRenderer;
    private NavMeshAgent navMeshAgent;
    private Transform TargetPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set the initial position of the LineRenderer to the player's position
        lineRenderer.positionCount = 100;
        

        // Determine the initial target based on the shortest distance
       // SetShortestDistanceTarget();
    }

  public  void SetShortestDistanceTarget()
    {
        
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 34f, transform.position.z));
        // Calculate distances
        float distanceToTarget1 = Vector3.Distance(transform.position, target1.position);
        float distanceToTarget2 = Vector3.Distance(transform.position, target2.position);

        // Set the target position for navigation based on the shortest distance
        if (distanceToTarget1 < distanceToTarget2)
        {
            SetTargetPosition(target1.position);
        }
        else
        {
            SetTargetPosition(target2.position);
        }
    }
  

    void SetTargetPosition(Vector3 position)
    {
        // Check if the target position is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 100f, NavMesh.AllAreas))
        {
            // Set the target position for navigation
            navMeshAgent.SetDestination(hit.position);
        }
    }

    void UpdateLineRenderer()
    {
        // Calculate the shortest path using NavMeshPath
        NavMeshPath navMeshPath = new NavMeshPath();
        navMeshAgent.CalculatePath(navMeshAgent.destination, navMeshPath);

        // Update LineRenderer with waypoints at a fixed height
        lineRenderer.positionCount = navMeshPath.corners.Length;
        for (int i = 0; i < navMeshPath.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(navMeshPath.corners[i].x, 34f, navMeshPath.corners[i].z));
        }
    }

    void Update()
    {
        // if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        // {
        //     SetShortestDistanceTarget();
        // }

        UpdateLineRenderer();
    }
}