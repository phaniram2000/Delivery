using System;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour
{
    private ITruckState currentState;
    [Space(20)] public DynamicJoystick joystick;
    [Space(10)] [Range(0, 190)] public int maxSpeed = 90;
    [Range(10, 120)] public int maxReverseSpeed = 45;
    [Range(1, 50)] public int accelerationMultiplier = 2;
    [Space(10)] [Range(10, 45)] public int maxSteeringAngle = 27;
    [Range(0.1f, 1f)] public float steeringSpeed = 0.5f;
    [Space(10)] [Range(100, 600)] public int brakeForce = 350;
    [Range(1, 100)] public int decelerationMultiplier = 2;
    [Range(1, 10)] public int handbrakeDriftMultiplier = 5;
    [Space(10)] public Vector3 bodyMassCenter;
    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)] public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)] public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)] public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;
    [Space(20)] [Space(10)] public bool useEffects = false;
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;
    [Space(10)] public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;
    [Space(20)] [Space(10)] public bool useUI = false;
    public Text carSpeedText;
    [Space(20)] [Space(10)] public bool useSounds = false;
    public AudioSource carEngineSound;
    public AudioSource tireScreechSound;
    internal float initialCarEngineSoundPitch;
    [Space(20)] [Space(10)] public bool useTouchControls = false;
    public GameObject throttleButton;
    internal PrometeoTouchInput throttlePTI;
    public GameObject reverseButton;
    internal PrometeoTouchInput reversePTI;
    public GameObject turnRightButton;
    internal PrometeoTouchInput turnRightPTI;
    public GameObject turnLeftButton;
    internal PrometeoTouchInput turnLeftPTI;
    public GameObject handbrakeButton;
    internal PrometeoTouchInput handbrakePTI;
    [HideInInspector] public float carSpeed;
    [HideInInspector] public bool isDrifting;
    [HideInInspector] public bool isTractionLocked;
    internal Rigidbody carRigidbody;
    internal float steeringAxis;
    internal float throttleAxis;
    internal float driftingAxis;
    internal float localVelocityZ;
    internal float localVelocityX;
    internal bool deceleratingCar;
    internal bool touchControlsSetup = false;
    [SerializeField] private bool UseSteering;
    public bool UseHandbrake;
    private bool forward;
    internal WheelFrictionCurve FLwheelFriction;
    internal float FLWextremumSlip;
    internal WheelFrictionCurve FRwheelFriction;
    internal float FRWextremumSlip;
    internal WheelFrictionCurve RLwheelFriction;
    internal float RLWextremumSlip;
    internal WheelFrictionCurve RRwheelFriction;
    internal float RRWextremumSlip;
    private UiManager _uiManager;
    internal GearShift _GearShift;
    internal float horizontalInput;
    internal float steeringAngle;

    public bool Steering
    {
        get { return UseSteering; }
        private set { UseSteering = value; }
    }

    public float SteeringAngle
    {
        get { return steeringAngle; }
        private set { steeringAngle = value; }
    }

    private void OnEnable()
    {
        GameEvents.TapToPlay += OnTapToPlay;
    }

    private void OnDisable()
    {
        GameEvents.TapToPlay -= OnTapToPlay;
    }

    private void Awake()
    {
        Application.targetFrameRate = 300;
    }

    void Start()
    {
        SwitchState(new DriveState());
        forward = true;
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;
        _uiManager = FindObjectOfType<UiManager>();
        _GearShift = FindObjectOfType<GearShift>();
        WheelSetup();
        JoyStickDesider();
        if (carEngineSound != null)
        {
            initialCarEngineSoundPitch = carEngineSound.pitch;
        }
        else if (!useUI)
        {
            if (carSpeedText != null)
            {
                carSpeedText.text = "0";
            }
        }

        if (useSounds)
        {
            InvokeRepeating("CarSounds", 0f, 0.1f);
        }
        else if (!useSounds)
        {
            if (carEngineSound != null)
            {
                carEngineSound.Stop();
            }

            if (tireScreechSound != null)
            {
                tireScreechSound.Stop();
            }
        }

        if (!useEffects)
        {
            if (RLWParticleSystem != null)
            {
                RLWParticleSystem.Stop();
            }

            if (RRWParticleSystem != null)
            {
                RRWParticleSystem.Stop();
            }

            if (RLWTireSkid != null)
            {
                RLWTireSkid.emitting = false;
            }

            if (RRWTireSkid != null)
            {
                RRWTireSkid.emitting = false;
            }
        }

        if (useTouchControls)
        {
            if (throttleButton != null && reverseButton != null &&
                turnRightButton != null && turnLeftButton != null
                && handbrakeButton != null)
            {
                throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
                reversePTI = reverseButton.GetComponent<PrometeoTouchInput>();
                turnLeftPTI = turnLeftButton.GetComponent<PrometeoTouchInput>();
                turnRightPTI = turnRightButton.GetComponent<PrometeoTouchInput>();
                handbrakePTI = handbrakeButton.GetComponent<PrometeoTouchInput>();
                touchControlsSetup = true;
            }
            else
            {
                String ex =
                    "Touch controls are not completely set up. You must drag and drop your scene buttons in the" +
                    " PrometeoCarController component.";
            }
        }
    }

    void Update()
    {
        // Perform actions based on the current state
        currentState.UpdateState();
    }


    public void SwitchState(ITruckState newState)
    {
        SetState(newState);
    }

    private void SetState(ITruckState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    private void OnTapToPlay()
    {
    }

    public void DecelerateCar()
    {
        if (throttleAxis != 0f)
        {
            if (throttleAxis > 0f)
            {
                throttleAxis = throttleAxis - (Time.deltaTime * 10f);
            }
            else if (throttleAxis < 0f)
            {
                throttleAxis = throttleAxis + (Time.deltaTime * 10f);
            }

            if (Mathf.Abs(throttleAxis) < 0.15f)
            {
                throttleAxis = 0f;
            }
        }

        carRigidbody.velocity =
            carRigidbody.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
        if (carRigidbody.velocity.magnitude < 0.25f)
        {
            carRigidbody.velocity = Vector3.zero;
            CancelInvoke("DecelerateCar");
        }
    }

    private void JoyStickDesider()
    {
        if (Steering)
        {
            throttleButton = _uiManager.WheelControl;
            reverseButton = _uiManager.WheelControl;
            turnRightButton = _uiManager.WheelControl;
            turnLeftButton = _uiManager.WheelControl;
            handbrakeButton = _uiManager.WheelControl;
        }
        else
        {
            throttleButton = _uiManager.Joystick;
            reverseButton = _uiManager.Joystick;
            turnRightButton = _uiManager.Joystick;
            turnLeftButton = _uiManager.Joystick;
            handbrakeButton = _uiManager.Joystick;
        }
    }

    private void WheelSetup()
    {
        FLwheelFriction = new WheelFrictionCurve();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;
        FRwheelFriction = new WheelFrictionCurve();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;
        RLwheelFriction = new WheelFrictionCurve();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;
        RRwheelFriction = new WheelFrictionCurve();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;
    }

    public void CarSounds()
    {
        if (useSounds)
        {
            try
            {
                if (carEngineSound != null)
                {
                    float engineSoundPitch =
                        initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.velocity.magnitude) / 25f);
                    carEngineSound.pitch = engineSoundPitch;
                }

                if ((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f))
                {
                    if (!tireScreechSound.isPlaying)
                    {
                        tireScreechSound.Play();
                    }
                }
                else if ((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f))
                {
                    tireScreechSound.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
        else if (!useSounds)
        {
            if (carEngineSound != null && carEngineSound.isPlaying)
            {
                carEngineSound.Stop();
            }

            if (tireScreechSound != null && tireScreechSound.isPlaying)
            {
                tireScreechSound.Stop();
            }
        }
    }
}

public interface ITruckState
{
    void EnterState(Truck truck);
    void UpdateState();
    void ExitState();
}