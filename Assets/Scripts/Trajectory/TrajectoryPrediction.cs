using UnityEngine;

public class TrajectoryPrediction : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int numPositions = 100; // Adjust the number of positions as needed
    public float predictionDistance = 5.0f;
    public float maxSteeringAngle = 30.0f; // Maximum steering angle in degrees
    public float steeringAngleDamping = 0.2f; // Adjust damping factor
    private Vector3[] linePositions;
    private PrometeoCarController MainController;

    private void Start()
    {
        MainController = FindObjectOfType<PrometeoCarController>();
        lineRenderer.positionCount = numPositions;
        linePositions = new Vector3[numPositions];
    }

    private void Update()
    {
        // Get user input for steering (e.g., Input.GetAxis("Horizontal"))
        float steeringInput = MainController.SteeringAngle;

        // Apply damping to the steering angle to reduce oversteering
        float dampedSteeringAngle = Mathf.Lerp(0, steeringInput, steeringAngleDamping);

        for (int i = 0; i < numPositions; i++)
        {
            float t = i / (float)(numPositions - 1);
            float angle = Mathf.Clamp(dampedSteeringAngle, -maxSteeringAngle, maxSteeringAngle);
            angle *= predictionDistance * t;

            // Calculate the position for each point along the prediction line
            Vector3 endpoint = transform.position +
                               Quaternion.Euler(0, angle, 0) * transform.forward * predictionDistance * t;
            linePositions[i] = endpoint;
        }

        // Update the Line Renderer positions
        lineRenderer.SetPositions(linePositions);
    }
}