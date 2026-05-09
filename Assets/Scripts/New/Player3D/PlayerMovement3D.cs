using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;

    [Header("Aim Settings")]
    public Transform cameraPivot;

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

    //Required components
    private CharacterController controller;

    //Input variables
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canLook = true;
    private Vector2 movement; //Raw movement input
    private Vector2 aim; //Raw aim input

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
    }

    //Assings variables to UserInput equivalent; Simplifies referencing
    private void GetInputs()
    {
        movement = UserInput.instance.moveInput;
        aim = UserInput.instance.aimInput;
    }

    //Used for player Movement
    private void Move()
    {
        if (canMove)
        {
            Vector3 move = transform.right * movement.x + transform.forward * movement.y;

            controller.Move(move * speed * Time.deltaTime);
        }
    }

    //Used for player Aim
    private void Look()
    {
        if (canLook)
        {
            Vector2 finalLookInput; //Variables that is used in aiming after calculations

            if (UserInput.instance.isGamepad)
            {
                //Adjusts sensitivity curve and adds deadzone
                Vector2 processedInput = ProcessControllerLook(aim);

                //Smooths out acceleration/deceleration
                currentLook = Vector2.SmoothDamp(currentLook,processedInput,ref currentLookVelocity,lookSmoothTime);

                //Combines calculations with sensitivity
                finalLookInput = currentLook * UserInput.instance.sensitivity * Time.deltaTime;
            }
            else
            {
                finalLookInput = aim * UserInput.instance.sensitivity * Time.deltaTime; //Value used when using MnK
            }

            xRotation -= finalLookInput.y;
            xRotation = Mathf.Clamp(xRotation, -90, 90); //Clamps camera look rotation

            cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0); //Rotates camera only

            transform.Rotate(Vector3.up * finalLookInput.x); //Rotates entire player
        }
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

    //Processes custom controller input behaviour
    private Vector2 ProcessControllerLook(Vector2 input)
    {
        //Adds deadzone
        if (input.magnitude < UserInput.instance.deadzone)
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
}
