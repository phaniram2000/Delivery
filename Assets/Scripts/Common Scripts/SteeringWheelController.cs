using UnityEngine;
using SimpleInputNamespace;

public class SteeringWheelControl : MonoBehaviour
{
    public Transform steeringWheel; // Reference to the steering wheel object.
    public float rotationSpeed = 100.0f; // Adjust the speed as needed.

    void Update()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float newZRotation = steeringWheel.rotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newZRotation);

    }
}