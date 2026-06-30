using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    //Makes this script a singleton
    public static UserInput instance;

    //Reference to PlayerInput component found on player
    private PlayerInput playerInput;

    //Player Inputs; Assigns all player inputs
    private InputAction movementAction;
    private InputAction aimAction;
    private InputAction interactAction;
    private InputAction pauseAction;
    private InputAction tabletAction;

    //Input variables (READ ONLY); Allows usage outside of this script
    public Vector2 moveInput { get; private set; }
    public Vector2 aimInput { get; private set; }
    public bool interactPressed { get; private set; }
    public bool pausePressed { get; private set; }
    public bool tabletPressed { get; private set; }

    //Input device (READ ONLY); Contains device specific settings
    public bool isGamepad { get; private set; }
    public float gamepadSensitivity;
    public float deadzone = 0.1f;

    public float mouseSensitivity;

    public float sensitivity {  get; private set; } //Reference this value for sensitivity


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();

        SetupInputs();
    }

    private void Update()
    {
        UpdateInputs();
    }

    //Assigns variables to corresponding input actions
    private void SetupInputs()
    {
        movementAction = playerInput.actions["Movement"];
        aimAction = playerInput.actions["Aim"];
        interactAction = playerInput.actions["Interact"];
        pauseAction = playerInput.actions["Exit/Pause"];
        tabletAction = playerInput.actions["Tablet"];
    }

    //Keeps variables updated with current input state
    private void UpdateInputs()
    {
        isGamepad = playerInput.currentControlScheme.Equals("Gamepad");

        moveInput = movementAction.ReadValue<Vector2>();
        aimInput = aimAction.ReadValue<Vector2>();
        interactPressed = interactAction.WasPressedThisFrame();
        pausePressed = pauseAction.WasPressedThisFrame();
        tabletPressed = tabletAction.WasPressedThisFrame();

        //Assigns correct sensitivity based on input
        if (isGamepad)
        {
            sensitivity = gamepadSensitivity;
        }
        else
        {
            sensitivity = mouseSensitivity;
        }
    }
}
