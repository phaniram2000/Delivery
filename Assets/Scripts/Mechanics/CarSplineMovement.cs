using System;
using DG.Tweening;
using UnityEngine;
using Dreamteck.Splines;

public class CarSplineMovement : MonoBehaviour
{
    public SplineFollower spline; // Assuming you have a Dreamteck spline component
    public float speed;
    private float distanceTravelled = 0f;
    public bool isPaused = false;
    public float obstacleDetectionDistance = 5f; // Adjust this value based on your needs
    public LayerMask obstacleLayer;

    private void Start()
    {
        spline = GetComponent<SplineFollower>();
    }

    void Update()
    {
        if (!isPaused)
        {
            spline.followSpeed = speed;
        }

        if (isPaused)
        {
            spline.followSpeed = 0;
        }

        // Check for obstacles and pause if necessary
        CheckForObstacles();
    }

    void CheckForObstacles()
    {
        // Raycast to detect obstacles
        RaycastHit hitCenter, hitLeft, hitRight;
        Vector3 rayDirection = transform.forward;

        // Debug draw the rays in the Unity Editor
        Debug.DrawRay(transform.position, rayDirection * obstacleDetectionDistance, Color.red);

        // Raycast to the center
        if (Physics.Raycast(transform.position, rayDirection, out hitCenter, obstacleDetectionDistance, obstacleLayer))
        {
            // Pause movement if an obstacle is detected
            PauseMovement();
            return; // No need to check other directions if the center ray hits an obstacle
        }

        // Raycast to the left
        Vector3 leftDirection = Quaternion.Euler(0, 20, 0) * rayDirection; // Adjust the angle based on your needs
        Debug.DrawRay(transform.position, leftDirection * obstacleDetectionDistance, Color.red);
        if (Physics.Raycast(transform.position, leftDirection, out hitLeft, obstacleDetectionDistance, obstacleLayer))
        {
            // Pause movement if an obstacle is detected
            PauseMovement();
            return; // No need to check other directions if the left ray hits an obstacle
        }

        // Raycast to the right
        Vector3 rightDirection = Quaternion.Euler(0, -20, 0) * rayDirection; // Adjust the angle based on your needs
        Debug.DrawRay(transform.position, rightDirection * obstacleDetectionDistance, Color.red);
        if (Physics.Raycast(transform.position, rightDirection, out hitRight, obstacleDetectionDistance, obstacleLayer))
        {
            // Pause movement if an obstacle is detected
            PauseMovement();
            return; // No need to check other directions if the right ray hits an obstacle
        }

        // Resume movement if no obstacle is in range
        if (isPaused)
        {
            float distanceToResume = 7f; // Adjust this value based on your needs
            if (Vector3.Distance(transform.position, hitCenter.point) > distanceToResume &&
                Vector3.Distance(transform.position, hitLeft.point) > distanceToResume &&
                Vector3.Distance(transform.position, hitRight.point) > distanceToResume)
            {
                ResumeMovement();
            }
        }
    }


    void PauseMovement()
    {
        isPaused = true;
        // Add logic to gradually decelerate the car if needed
    }

    void ResumeMovement()
    {
        isPaused = false;
        // Add logic to resume movement
    }
}