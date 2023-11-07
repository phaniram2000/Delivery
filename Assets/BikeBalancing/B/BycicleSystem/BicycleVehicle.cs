using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleVehicle : MonoBehaviour
{
    public Joystick _Joystick;
    float horizontalInput;
    float vereticallInput;

    public Transform handle;
    public bool braking;
    Rigidbody rb;

    public Vector3 COG;

    [SerializeField] float motorforce;
    [SerializeField] float brakeForce;
    float currentbrakeForce;

    float steeringAngle;
    [SerializeField] float currentSteeringAngle;
    [Range(0f, 0.1f)] [SerializeField] float speedteercontrolTime;
    [SerializeField] float maxSteeringAngle;
    [Range(0.000001f, 1)] [SerializeField] float turnSmoothing;

    [SerializeField] private float maxlayingAngle;

    public float targetlayingAngle;
    [Range(-40, 40)] public float layingammount;
    [Range(0.000001f, 1)] [SerializeField] float leanSmoothing;

    [SerializeField] WheelCollider frontWheel;
    [SerializeField] WheelCollider backWheel;

    [SerializeField] Transform frontWheeltransform;
    [SerializeField] Transform backWheeltransform;

    [SerializeField] TrailRenderer fronttrail;
    [SerializeField] TrailRenderer rearttrail;

    public bool frontGrounded;
    public bool rearGrounded;

    // Start is called before the first frame update
    void Start()
    {
        StopEmitTrail();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        Breaking();
        HandleSteering();
        UpdateWheels();
        UpdateHandle();
        LayOnTurn();
        DownPresureOnSpeed();
        EmitTrail();
        if (Input.touchCount > 0)
        {
            braking = false;
            HandleEngine();
        }

        if (Input.touchCount < 1)
        {
            Debug.Log("breaking");
            braking = true;
        }

        if (braking == true)
        {
            ApplyBraking();
        }
    }

    public void GetInput()
    {
        horizontalInput = _Joystick.Horizontal;
        vereticallInput = _Joystick.Vertical;
        // braking = Input.GetKey(KeyCode.Space);
    }

    public void HandleEngine()
    {
        // backWheel.motorTorque = vereticallInput * motorforce;
        backWheel.motorTorque = motorforce;
    }


    private void Breaking()
    {
        currentbrakeForce = braking ? brakeForce : 0f;
        if (braking)
        {
            ApplyBraking();
        }
        else
        {
            ReleaseBrakibg();
        }
    }

    public void DownPresureOnSpeed()
    {
        Vector3 downforce = Vector3.down;
        float downpressure;
        if (rb.velocity.magnitude > 5)
        {
            downpressure = rb.velocity.magnitude;
            rb.AddForce(downforce * downpressure, ForceMode.Force);
        }
    }

    public void ApplyBraking()
    {
        //frontWheel.brakeTorque = currentbrakeForce/2;
        frontWheel.brakeTorque = currentbrakeForce;
        backWheel.brakeTorque = currentbrakeForce;
    }

    public void ReleaseBrakibg()
    {
        frontWheel.brakeTorque = 0;
        backWheel.brakeTorque = 0;
    }

    public void SpeedSteerinReductor()
    {
        
        if (rb.velocity.magnitude < 5) //We set the limiting factor for the steering thus allowing how much steer we give to the player in relation to the speed
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 40, speedteercontrolTime);
        }

        if (rb.velocity.magnitude > 5 && rb.velocity.magnitude < 30)
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 30, speedteercontrolTime);
        }

        if (rb.velocity.magnitude > 30 && rb.velocity.magnitude < 50)
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 15, speedteercontrolTime);
        }

        if (rb.velocity.magnitude > 50 && rb.velocity.magnitude < 60)
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 10, speedteercontrolTime);
        }

        if (rb.velocity.magnitude > 60)
        {
            maxSteeringAngle = Mathf.LerpAngle(maxSteeringAngle, 5, speedteercontrolTime);
        }
    }

    public void HandleSteering()
    {
        SpeedSteerinReductor();

        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, maxSteeringAngle * horizontalInput, turnSmoothing);
        frontWheel.steerAngle = currentSteeringAngle;

        //We set the target laying angle to the + or - input value of our steering 
        //We invert our input for rotating in the ocrrect axis
        targetlayingAngle = maxlayingAngle * -horizontalInput;
    }

    private void LayOnTurn()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;

        if (rb.velocity.magnitude < 1)
        {
            layingammount = Mathf.LerpAngle(layingammount, 0f, 0.05f);
            transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, layingammount);
            return;
        }

        if (currentSteeringAngle < 0.5f && currentSteeringAngle > -0.5) //We're stright
        {
            layingammount = Mathf.LerpAngle(layingammount, 0f, leanSmoothing);
        }
        else //We're turning
        {
            layingammount = Mathf.LerpAngle(layingammount, targetlayingAngle, leanSmoothing);
            rb.centerOfMass = new Vector3(rb.centerOfMass.x, COG.y, rb.centerOfMass.z);
        }

        transform.rotation = Quaternion.Euler(currentRot.x, currentRot.y, layingammount);
    }

    public void UpdateWheels()
    {
        UpdateSingleWheel(frontWheel, frontWheeltransform);
        UpdateSingleWheel(backWheel, backWheeltransform);
    }

    public void UpdateHandle()
    {
        Quaternion sethandleRot;
        sethandleRot = frontWheeltransform.rotation;
        handle.localRotation = Quaternion.Euler(handle.localRotation.eulerAngles.x, currentSteeringAngle,
            handle.localRotation.eulerAngles.z);
    }
    private void EmitTrail()
    {
        frontGrounded = frontWheel.GetGroundHit(out WheelHit Fhit);
        rearGrounded = backWheel.GetGroundHit(out WheelHit Rhit);

        if (frontGrounded)
        {
            fronttrail.emitting = true;
        }
        else
        {
            fronttrail.emitting = false;
        }

        if (rearGrounded)
        {
            rearttrail.emitting = true;
        }
        else
        {
            rearttrail.emitting = false;
        }

        //fronttrail.emitting = true;
        //rearttrail.emitting = true;
    }

    private void StopEmitTrail()
    {
        fronttrail.emitting = false;
        rearttrail.emitting = false;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}