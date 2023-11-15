using UnityEngine;
using SimpleInputNamespace;

public class SteeringWheelControl : MonoBehaviour
{
    public Transform steeringWheel; // Reference to the steering wheel object.
    public float rotationSpeed = 100.0f; // Adjust the speed as needed.

    public bool ReversSteer;

    void Update()
    {
        float newZRotation;
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        if (ReversSteer)
        {
            newZRotation = -steeringWheel.rotation.eulerAngles.z;
        }
        else
        {
            newZRotation = steeringWheel.rotation.eulerAngles.z;
        }

        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZRotation);
    }
}