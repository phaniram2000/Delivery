using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class people : MonoBehaviour
{
    public bool back;
    public float movespeed, forwardRotation, Backrotation;
    private static readonly int Walk = Animator.StringToHash("Walk");

    public bool isPaused = false;
    public float obstacleDetectionDistance = 5f; // Adjust this value based on your needs

    public LayerMask obstacleLayer;

    private Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Anim.SetBool(Walk, true);
    }

    private void Update()
    {
        if (!isPaused)
        {
            Anim.SetBool(Walk, true);
            transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
        }

        if (isPaused)
        {
            Anim.SetBool(Walk, false);
        }

        // Check for obstacles and pause if necessary
        CheckForObstacles();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moveback"))
        {
            if (back == false)
            {
                transform.DORotate(
                    new Vector3(transform.rotation.x, Backrotation, transform.rotation.z),
                    .01f);
                back = true;
            }
            else if (back == true)
            {
                transform.DORotate(
                    new Vector3(transform.rotation.x, forwardRotation, transform.rotation.z),
                    .01f);
                back = false;
            }
        }
    }

    void CheckForObstacles()
    {
        // Raycast to detect obstacles
        RaycastHit hitCenter;
        Vector3 rayDirection = transform.forward;
        Vector3 Raypoint = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        Debug.DrawRay(Raypoint, rayDirection * obstacleDetectionDistance, Color.red);

        if (Physics.Raycast(Raypoint, rayDirection, out hitCenter, obstacleDetectionDistance, obstacleLayer))
        {
            Debug.Log(hitCenter.transform.name);
            // Pause movement if an obstacle is detected
            PauseMovement();
            return; // No need to check other directions if the center ray hits an obstacle
        }

        if (isPaused)
        {
            float distanceToResume = 10f; // Adjust this value based on your needs
            if (Vector3.Distance(transform.position, hitCenter.point) > distanceToResume)
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