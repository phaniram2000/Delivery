using UnityEngine;
using System;
using UnityEngine.UI;

public class DriveState : MonoBehaviour, ITruckState
{
    private Truck truck;
    private bool forward;

    public void EnterState(Truck truck)
    {
        this.truck = truck;
    }

    public void UpdateState()
    {
        truck.carSpeed = (2 * Mathf.PI * truck.frontLeftCollider.radius * truck.frontLeftCollider.rpm * 60) / 1000;
        truck.localVelocityX = truck.transform.InverseTransformDirection(truck.carRigidbody.velocity).x;
        truck.localVelocityZ = truck.transform.InverseTransformDirection(truck.carRigidbody.velocity).z;
        if (truck.useTouchControls && truck.touchControlsSetup)
        {
            TouchControls();
        }

        AnimateWheelMeshes();
    }


    private void TouchControls()
    {
        if (truck.throttlePTI.buttonPressed && !truck._GearShift.MoveBack)
        {
            truck.CancelInvoke("DecelerateCar");
            truck.deceleratingCar = false;
            GoForward();
        }

        if (truck.reversePTI.buttonPressed && truck._GearShift.MoveBack)
        {
            truck.CancelInvoke("DecelerateCar");
            truck.deceleratingCar = false;
            GoReverse();
        }

        if (truck.turnLeftPTI.buttonPressed || truck.joystick.Horizontal < 0.1f)
        {
            TurnLeft();
        }

        if (truck.turnRightPTI.buttonPressed || truck.joystick.Horizontal > 0)
        {
            TurnRight();
        }

        if (truck.handbrakePTI.buttonPressed && truck.UseHandbrake)
        {
            truck.CancelInvoke("DecelerateCar");
            truck.deceleratingCar = false;
            Handbrake();
        }

        if (!truck.handbrakePTI.buttonPressed)
        {
            RecoverTraction();
        }

        if ((!truck.throttlePTI.buttonPressed && !truck.reversePTI.buttonPressed))
        {
            ThrottleOff();
        }

        if ((!truck.reversePTI.buttonPressed && !truck.throttlePTI.buttonPressed) &&
            !truck.handbrakePTI.buttonPressed &&
            !truck.deceleratingCar)
        {
            truck.InvokeRepeating("DecelerateCar", 0f, 0.2f);
            //truck.deceleratingCar = true;
        }

        if (!truck.turnLeftPTI.buttonPressed && !truck.turnRightPTI.buttonPressed && truck.steeringAxis != 0f)
        {
            ResetSteeringAngle();
        }
    }

    public void TurnLeft()
    {
        if (truck.Steering)
        {
            truck.horizontalInput = SimpleInput.GetAxis("Horizontal");
        }

        if (!truck.Steering)
        {
            truck.horizontalInput = truck.joystick.Horizontal;
        }

        truck.steeringAngle = truck.horizontalInput * truck.maxSteeringAngle;

        truck.frontLeftCollider.steerAngle = truck.steeringAngle;
        truck.frontRightCollider.steerAngle = truck.steeringAngle;
    }

    public void TurnRight()
    {
        //  float horizontalInput = SimpleInput.GetAxis("Horizontal");
        if (truck.Steering)
        {
            truck.horizontalInput = SimpleInput.GetAxis("Horizontal");
        }

        if (!truck.Steering)
        {
            truck.horizontalInput = truck.joystick.Horizontal;
        }

        truck.steeringAngle = truck.horizontalInput * truck.maxSteeringAngle;

        truck.frontLeftCollider.steerAngle = truck.steeringAngle;
        truck.frontRightCollider.steerAngle = truck.steeringAngle;
    }

    public void ResetSteeringAngle()
    {
        if (truck.steeringAxis < 0f)
        {
            truck.steeringAxis = truck.steeringAxis + (Time.deltaTime * 10f * truck.steeringSpeed);
        }
        else if (truck.steeringAxis > 0f)
        {
            truck.steeringAxis = truck.steeringAxis - (Time.deltaTime * 10f * truck.steeringSpeed);
        }

        if (Mathf.Abs(truck.frontLeftCollider.steerAngle) < 1f)
        {
            truck.steeringAxis = 0f;
        }

        var steeringAngle = truck.steeringAxis * truck.maxSteeringAngle;
        truck.frontLeftCollider.steerAngle =
            Mathf.Lerp(truck.frontLeftCollider.steerAngle, steeringAngle, truck.steeringSpeed);
        truck.frontRightCollider.steerAngle =
            Mathf.Lerp(truck.frontRightCollider.steerAngle, steeringAngle, truck.steeringSpeed);
    }

    public void GoForward()
    {
        if (Mathf.Abs(truck.localVelocityX) > 2.5f)
        {
            truck.isDrifting = true;
            DriftCarPS();
        }
        else
        {
            truck.isDrifting = false;
            DriftCarPS();
        }

        truck.throttleAxis = truck.throttleAxis + (Time.deltaTime * 3f);
        if (truck.throttleAxis > 1f)
        {
            truck.throttleAxis = 1f;
        }

        if (truck.localVelocityZ < -1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.RoundToInt(truck.carSpeed) < truck.maxSpeed)
            {
                truck.frontLeftCollider.brakeTorque = 0;
                truck.frontLeftCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.frontRightCollider.brakeTorque = 0;
                truck.frontRightCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.rearLeftCollider.brakeTorque = 0;
                truck.rearLeftCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.rearRightCollider.brakeTorque = 0;
                truck.rearRightCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
            }
            else
            {
                truck.frontLeftCollider.motorTorque = 0;
                truck.frontRightCollider.motorTorque = 0;
                truck.rearLeftCollider.motorTorque = 0;
                truck.rearRightCollider.motorTorque = 0;
            }
        }

        updateFuelGate();
        truck.BreakLight_L.SetActive(false);
        truck.BreakLight_R.SetActive(false);
    }

    public void GoReverse()
    {
        if (Mathf.Abs(truck.localVelocityX) > 2.5f)
        {
            truck.isDrifting = true;
            DriftCarPS();
        }
        else
        {
            truck.isDrifting = false;
            DriftCarPS();
        }

        truck.throttleAxis = truck.throttleAxis - (Time.deltaTime * 3f);
        if (truck.throttleAxis < -1f)
        {
            truck.throttleAxis = -1f;
        }

        if (truck.localVelocityZ > 1f)
        {
            Brakes();
        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(truck.carSpeed)) < truck.maxReverseSpeed)
            {
                truck.frontLeftCollider.brakeTorque = 0;
                truck.frontLeftCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.frontRightCollider.brakeTorque = 0;
                truck.frontRightCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.rearLeftCollider.brakeTorque = 0;
                truck.rearLeftCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
                truck.rearRightCollider.brakeTorque = 0;
                truck.rearRightCollider.motorTorque = (truck.accelerationMultiplier * 50f) * truck.throttleAxis;
            }
            else
            {
                truck.frontLeftCollider.motorTorque = 0;
                truck.frontRightCollider.motorTorque = 0;
                truck.rearLeftCollider.motorTorque = 0;
                truck.rearRightCollider.motorTorque = 0;
            }
        }

        updateFuelGate();
        truck.BreakLight_L.SetActive(false);
        truck.BreakLight_R.SetActive(false);
    }

    void AnimateWheelMeshes()
    {
        try
        {
            Quaternion FLWRotation;
            Vector3 FLWPosition;
            truck.frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
            truck.frontLeftMesh.transform.position = FLWPosition;
            truck.frontLeftMesh.transform.rotation = FLWRotation;

            Quaternion FRWRotation;
            Vector3 FRWPosition;
            truck.frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
            truck.frontRightMesh.transform.position = FRWPosition;
            truck.frontRightMesh.transform.rotation = FRWRotation;

            Quaternion RLWRotation;
            Vector3 RLWPosition;
            truck.rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
            truck.rearLeftMesh.transform.position = RLWPosition;
            truck.rearLeftMesh.transform.rotation = RLWRotation;

            Quaternion RRWRotation;
            Vector3 RRWPosition;
            truck.rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
            truck.rearRightMesh.transform.position = RRWPosition;
            truck.rearRightMesh.transform.rotation = RRWRotation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }

    public void ThrottleOff()
    {
        truck.frontLeftCollider.motorTorque = 0;
        truck.frontRightCollider.motorTorque = 0;
        truck.rearLeftCollider.motorTorque = 0;
        truck.rearRightCollider.motorTorque = 0;
    }

    public void Brakes()
    {
        truck.frontLeftCollider.brakeTorque = truck.brakeForce;
        truck.frontRightCollider.brakeTorque = truck.brakeForce;
        truck.rearLeftCollider.brakeTorque = truck.brakeForce;
        truck.rearRightCollider.brakeTorque = truck.brakeForce;
    }

    public void Handbrake()
    {
        truck.CancelInvoke("RecoverTraction");
        truck.driftingAxis = truck.driftingAxis + (Time.deltaTime);
        float secureStartingPoint = truck.driftingAxis * truck.FLWextremumSlip * truck.handbrakeDriftMultiplier;

        if (secureStartingPoint < truck.FLWextremumSlip)
        {
            truck.driftingAxis = truck.FLWextremumSlip / (truck.FLWextremumSlip * truck.handbrakeDriftMultiplier);
        }

        if (truck.driftingAxis > 1f)
        {
            truck.driftingAxis = 1f;
        }

        if (Mathf.Abs(truck.localVelocityX) > 2.5f)
        {
            truck.isDrifting = true;
        }
        else
        {
            truck.isDrifting = false;
        }

        if (truck.driftingAxis < 1f)
        {
            truck.FLwheelFriction.extremumSlip =
                truck.FLWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.frontLeftCollider.sidewaysFriction = truck.FLwheelFriction;

            truck.FRwheelFriction.extremumSlip =
                truck.FRWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.frontRightCollider.sidewaysFriction = truck.FRwheelFriction;

            truck.RLwheelFriction.extremumSlip =
                truck.RLWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.rearLeftCollider.sidewaysFriction = truck.RLwheelFriction;

            truck.RRwheelFriction.extremumSlip =
                truck.RRWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.rearRightCollider.sidewaysFriction = truck.RRwheelFriction;
        }

        truck.isTractionLocked = true;
        DriftCarPS();
    }

    public void DriftCarPS()
    {
        if (truck.useEffects)
        {
            try
            {
                if (truck.isDrifting)
                {
                    truck.RLWParticleSystem.Play();
                    truck.RRWParticleSystem.Play();
                }
                else if (!truck.isDrifting)
                {
                    truck.RLWParticleSystem.Stop();
                    truck.RRWParticleSystem.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }

            try
            {
                if ((truck.isTractionLocked || Mathf.Abs(truck.localVelocityX) > 5f) && Mathf.Abs(truck.carSpeed) > 12f)
                {
                    truck.RLWTireSkid.emitting = true;
                    truck.RRWTireSkid.emitting = true;
                }
                else
                {
                    truck.RLWTireSkid.emitting = false;
                    truck.RRWTireSkid.emitting = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
        else if (!truck.useEffects)
        {
            if (truck.RLWParticleSystem != null)
            {
                truck.RLWParticleSystem.Stop();
            }

            if (truck.RRWParticleSystem != null)
            {
                truck.RRWParticleSystem.Stop();
            }

            if (truck.RLWTireSkid != null)
            {
                truck.RLWTireSkid.emitting = false;
            }

            if (truck.RRWTireSkid != null)
            {
                truck.RRWTireSkid.emitting = false;
            }
        }
    }

    public void RecoverTraction()
    {
        truck.isTractionLocked = false;
        truck.driftingAxis = truck.driftingAxis - (Time.deltaTime / 1.5f);
        if (truck.driftingAxis < 0f)
        {
            truck.driftingAxis = 0f;
        }

        if (truck.FLwheelFriction.extremumSlip > truck.FLWextremumSlip)
        {
            truck.FLwheelFriction.extremumSlip =
                truck.FLWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.frontLeftCollider.sidewaysFriction = truck.FLwheelFriction;

            truck.FRwheelFriction.extremumSlip =
                truck.FRWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.frontRightCollider.sidewaysFriction = truck.FRwheelFriction;

            truck.RLwheelFriction.extremumSlip =
                truck.RLWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.rearLeftCollider.sidewaysFriction = truck.RLwheelFriction;

            truck.RRwheelFriction.extremumSlip =
                truck.RRWextremumSlip * truck.handbrakeDriftMultiplier * truck.driftingAxis;
            truck.rearRightCollider.sidewaysFriction = truck.RRwheelFriction;

            Invoke("RecoverTraction", Time.deltaTime);
        }
        else if (truck.FLwheelFriction.extremumSlip < truck.FLWextremumSlip)
        {
            truck.FLwheelFriction.extremumSlip = truck.FLWextremumSlip;
            truck.frontLeftCollider.sidewaysFriction = truck.FLwheelFriction;

            truck.FRwheelFriction.extremumSlip = truck.FRWextremumSlip;
            truck.frontRightCollider.sidewaysFriction = truck.FRwheelFriction;

            truck.RLwheelFriction.extremumSlip = truck.RLWextremumSlip;
            truck.rearLeftCollider.sidewaysFriction = truck.RLwheelFriction;

            truck.RRwheelFriction.extremumSlip = truck.RRWextremumSlip;
            truck.rearRightCollider.sidewaysFriction = truck.RRwheelFriction;

            truck.driftingAxis = 0f;
        }
    }

    private void updateFuelGate()
    {
        if (truck.FuelGage.value > 0)
        {
            // Decrease the slider value gradually over time
            truck.FuelGage.value -= truck.fuelConsumption_Rate * Time.deltaTime;
            truck.SliderColorDesider();
        }
        else
        {
           // truck.SwitchState(new OutOfFuelState());
        }
    }

  


    public void ExitState()
    {
        // Implement actions when exiting Drive state
    }
}