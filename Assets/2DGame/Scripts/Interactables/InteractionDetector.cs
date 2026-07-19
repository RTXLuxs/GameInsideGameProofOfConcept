using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable2D interactableInRange = null;
    public GameObject interactionPrompt;

    private PlayerCarryController carryController;

    void Start()
    {
        interactionPrompt.SetActive(false);

        // PlayerCarryController is on the parent (Player2D)
        carryController = GetComponentInParent<PlayerCarryController>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // If there is something to interact with, do that first.
        if (interactableInRange != null)
        {
            interactableInRange.Interact();
            return;
        }

        // Otherwise drop the currently carried item.
        if (carryController != null && carryController.IsCarrying)
        {
            carryController.Drop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable2D interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable2D interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionPrompt.SetActive(false);
        }
    }
}