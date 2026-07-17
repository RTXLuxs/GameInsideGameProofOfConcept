using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class KeypadInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] public string interactionText = "Use Keypad [E]";

    [SerializeField] private CinemachineCamera keypadCam;

    private bool isUsingKeypad;

    private Collider thisCollider;

    public bool IsUsingKeypad => isUsingKeypad;

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact()
    {
        if (isUsingKeypad)
            return;

        EnterKeypad();
    }

    private void Update()
    {
        if (!isUsingKeypad)
            return;

        // Exit key (Escape / Pause)
        if (UserInput.Instance.pausePressed)
        {
            ExitKeypad();
        }
    }

    private void EnterKeypad()
    {
        isUsingKeypad = true;

        SwitchCameras.Instance.interactionCamera = keypadCam; //Assigns interaction cam
        SwitchCameras.Instance.SwitchToInteraction(); //Switches to that cam

        PlayerState.Instance.DisableControls();

        // TODO:
        // Disable player interaction

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        interactionText = "";

        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;

        Debug.Log("Entered keypad.");
    }

    public void ExitKeypad()
    {
        isUsingKeypad = false;

        SwitchCameras.Instance.SwitchToFPS();

        PlayerState.Instance.EnableControls();

        // TODO:
        // Re-enable player interaction

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        interactionText = "Use Keypad[E]";

        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = true;


        Debug.Log("Exited keypad.");
    }
}
