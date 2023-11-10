using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class TruckControl : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public GameObject throttleButton;
    PrometeoTouchInput throttlePTI;
    private UiManager _uiManager;

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void Start()
    {
        throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
        _uiManager = FindObjectOfType<UiManager>();
    }

    private float motor;

    public void FixedUpdate()
    {
        // if (throttlePTI.buttonPressed && !_uiManager.MoveBack)
        // {
        //     motor = maxMotorTorque;
        // }
        //
        // if (throttlePTI.buttonPressed && _uiManager.MoveBack)
        // {
        //     motor = -maxMotorTorque;
        // }


        float steering = maxSteeringAngle * SimpleInput.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}