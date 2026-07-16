using UnityEngine;

public class PlayerInteract3D : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float range;           // Interaction range
    [SerializeField] private float sphereRadius;    // Size of sphere cast

    private IInteractable currentInteractable;      // Stores hit object

    private void Update()
    {
        if (PlayerState.Instance.isPCMode)
        {
            InteractionUI3D.Instance.HideText();
            currentInteractable = null;
            return;
        }

        DetectInteraction();

        if (UserInput.Instance.interactPressed)
        {
            Interact();
        }
    }

    private void DetectInteraction()
    {
        currentInteractable = null;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, range);

        float closestToCenter = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            // Look for an interactable on this object or any of its parents
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable == null)
            {
                continue;
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.point);
            float distanceToCenter = Vector2.Distance(screenCenter, screenPos);

            if (distanceToCenter < closestToCenter)
            {
                closestToCenter = distanceToCenter;
                currentInteractable = interactable;
            }
        }

        if (currentInteractable != null)
        {
            InteractionUI3D.Instance.ShowText(currentInteractable.GetInteractionText());
        }
        else
        {
            InteractionUI3D.Instance.HideText();
        }
    }

    private void Interact()
    {
        currentInteractable?.Interact();
    }
}