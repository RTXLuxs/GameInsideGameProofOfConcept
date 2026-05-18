using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable2D interactableInRange = null; 
    public GameObject interactionPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
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
