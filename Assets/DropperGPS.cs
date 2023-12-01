using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperGPS : MonoBehaviour
{
   public Transform target;
    public float waypointSpacing = 1f; // Spacing between waypoints on the LineRenderer
    private LineRenderer lineRenderer;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Set the initial position of the LineRenderer to the player's position
        lineRenderer.positionCount = 100;
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 34f, transform.position.z));

        // Set the target position for navigation
        SetTargetPosition(target.position);
    }

    void SetTargetPosition(Vector3 position)
    {
        // Check if the target position is on the NavMesh
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, 100f, UnityEngine.AI.NavMesh.AllAreas))
        {
            // Set the target position for navigation
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError("Target position is not on the NavMesh.");
        }
    }

    void UpdateLineRenderer()
    {
        // Calculate the shortest path using NavMeshPath
        UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
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
        // Check if the player has reached the target
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Optionally, you can handle what happens when the player reaches the target here
            Debug.Log("Player reached the target!");
        }

        // Update LineRenderer continuously
        UpdateLineRenderer();
    }
}
