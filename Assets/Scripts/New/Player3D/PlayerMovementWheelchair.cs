using UnityEngine;

public class PlayerMovementWheelchair : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float reverseSpeed;
    //Seperates forward and reverse speed
    [SerializeField] private float acceleration; //Determines how fast the wheelchair reaches maximum speed
    [SerializeField] private float deceleration; //How fast the wheelchair stops
    private float currentSpeed; //Stores current speed calculation

    [Header("Wheelchair Settings")]
    [SerializeField] private float turnSpeed;

    [Header("Aim Settings")]
    public Transform cameraPivot;
    [SerializeField] float maxHeadRotation; //Clamps the maximum head rotation

    [Header("Camera Recentering Settings")]
    [SerializeField] private float recenterDelay;
    [SerializeField] private float recenterSpeed;
    [SerializeField] private float recenterSmoothTime;
    private float recenterVelocity;
    private float lastLookTime;

    //Controller specific settings

    //Adjusts sensitivity curve for controllers
    [Header("Controller Aim Curve")]
    [Range(1f, 5f)]
    public float aimExponent = 2f;

    //Adds slight delay to controller inputs
    [Header("Controller Smoothing")]
    public float lookSmoothTime = 0.08f;

    //SmoothDamp state
    private Vector2 currentLook;
    private Vector2 currentLookVelocity;

    //Rotation
    private float xRotation;
    private float yRotation;

    //Required components
    private CharacterController controller;

    //Input variables
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canLook = true;
    private Vector2 movement; //Raw movement input
    private Vector2 aim; //Raw aim input

    [Header("Gravity")]
    public float gravity = -9.81f;

    private float verticalVelocity;

    public AudioSource wheelchairAudio;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        canMove = true;
        canLook = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        GetInputs();
        Move();
        Look();
        RecenterHead();
        ApplyGravity();
    }

    //Assings variables to UserInput equivalent; Simplifies referencing
    private void GetInputs()
    {
        movement = UserInput.Instance.moveInput;
        aim = UserInput.Instance.aimInput;
    }

    //Used for player Movement
    private void Move()
    {
        if (!canMove)
            return;

        float moveInput = movement.y;
        float turnInput = movement.x;

        if (moveInput == 0)
        {
            wheelchairAudio.enabled = false;
        }
        else
        {
            wheelchairAudio.enabled = true;
        }

            // Rotate wheelchair
            transform.Rotate(
                Vector3.up *
                turnInput *
                turnSpeed *
                Time.deltaTime
            );

        // Acceleration while moving,
        // deceleration when stopping
        float rate =
            Mathf.Abs(moveInput) > 0.01f
            ? acceleration
            : deceleration;

        // Different forward/reverse speeds
        float targetSpeed;

        if (moveInput > 0)
        {
            targetSpeed = moveInput * forwardSpeed;
        }
        else
        {
            targetSpeed = moveInput * reverseSpeed;
        }

        // Smooth acceleration
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            rate * Time.deltaTime
        );

        // Move in facing direction
        Vector3 move =
            transform.forward *
            currentSpeed;

        controller.Move(
            move *
            Time.deltaTime
        );
    }

    //Used for player Aim
    private void Look()
    {
        if (canLook)
        {
            Vector2 finalLookInput; //Variables that is used in aiming after calculations

            if (UserInput.Instance.isGamepad)
            {
                //Adjusts sensitivity curve and adds deadzone
                Vector2 processedInput = ProcessControllerLook(aim);

                //Smooths out acceleration/deceleration
                currentLook = Vector2.SmoothDamp(currentLook, processedInput, ref currentLookVelocity, lookSmoothTime);

                //Combines calculations with sensitivity
                finalLookInput = currentLook * UserInput.Instance.sensitivity * Time.deltaTime;
            }
            else
            {
                finalLookInput = aim * UserInput.Instance.sensitivity * Time.deltaTime; //Value used when using MnK
            }

            if (Mathf.Abs(finalLookInput.x) > 0.01f ||
            Mathf.Abs(finalLookInput.y) > 0.01f)
            {
                lastLookTime = Time.time;

                recenterVelocity = 0f;
            }

            xRotation -= finalLookInput.y;
            xRotation = Mathf.Clamp(xRotation, -90, 90); //Clamps camera look rotation

            yRotation += finalLookInput.x;
            yRotation = Mathf.Clamp(yRotation, -maxHeadRotation, maxHeadRotation); //Clamps camera look rotation

            cameraPivot.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    private void RecenterHead()
    {
        bool isMoving =
        Mathf.Abs(currentSpeed) > 0.05f;

        bool delayPassed =
            Time.time - lastLookTime >
            recenterDelay;

        if (!isMoving || !delayPassed)
            return;

        yRotation = Mathf.SmoothDamp(
            yRotation,
            0f,
            ref recenterVelocity,
            recenterSmoothTime
        );
    }

    //Used to disable 3D controls
    public void EnableControls()
    {
        canMove = true;
        canLook = true;
    }

    //Used to enable 3D controls
    public void DisableControls()
    {
        canMove = false;
        canLook = false;
    }

    public void LookOnly()
    {
        canMove = false;
        canLook = true;
    }

    //Processes custom controller input behaviour
    private Vector2 ProcessControllerLook(Vector2 input)
    {
        //Adds deadzone
        if (input.magnitude < UserInput.Instance.deadzone)
        {
            return Vector2.zero;
        }

        //Applies sensitivity curve adjustments
        input.x = ApplyCurve(input.x);
        input.y = ApplyCurve(input.y);

        return input;
    }

    //Calculates sensitivity curve
    private float ApplyCurve(float value)
    {
        float sign = Mathf.Sign(value);

        value = Mathf.Abs(value);

        //Changes linear curve based on aimExponent
        value = Mathf.Pow(value, aimExponent);

        return value * sign;
    }

    private void ApplyGravity()
    {
        // Prevent infinite downward buildup
        if (controller.isGrounded &&
            verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        // Apply gravity acceleration
        verticalVelocity +=
            gravity * Time.deltaTime;

        Vector3 gravityMove =
            Vector3.up *
            verticalVelocity;

        controller.Move(
            gravityMove *
            Time.deltaTime
        );
    }
}
