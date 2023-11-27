using UnityEngine;

public class TruckController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public Transform[] wheelTransforms;

    void Update()
    {
        UpdateWheelPoses();
    }

    void UpdateWheelPoses()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider wheelCollider = wheelColliders[i];
            Transform wheelTransform = wheelTransforms[i];

            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);

            // Update the position and rotation of the wheel transform
            wheelTransform.position = pos;
            wheelTransform.rotation = rot;
        }
    }
}
