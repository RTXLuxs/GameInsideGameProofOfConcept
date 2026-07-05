using UnityEngine;

public class PlayerInteract3D : MonoBehaviour
{
    [Header("Interaction Settigns")]
    [SerializeField] private float range; //Interaction range
    [SerializeField] private float sphereRadius; //Size of sphere cast

    private IInteractable currentInteractable; //Stores hit object

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

    void DetectInteraction()
    {
        currentInteractable = null;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f); //Calculates sceen center

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, range);

        float closestToCenter = float.MaxValue;

        foreach (RaycastHit hit in hits ) //Go through objects hit by raycast
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>(); //Check object for interaction script

            if (interactable == null) //Ignore objects without interaction script
            {
                continue;
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.point); //Converts 3D hitpoint to screenpoint

            float distanceToCenter = Vector2.Distance(screenCenter, screenPos); //Calculate distance to center

            //Compare object to previous object and store if closer to center
            if (distanceToCenter < closestToCenter)
            {
                closestToCenter = distanceToCenter;
                currentInteractable = interactable;
            }
        }

        //Enable or disable UI Text
        if (currentInteractable != null)
        {
            InteractionUI3D.Instance.ShowText(currentInteractable.GetInteractionText());
        }
        else
        {
            InteractionUI3D.Instance.HideText();
        }
    }

    public void Interact()
    {
        currentInteractable?.Interact();
    }
}
