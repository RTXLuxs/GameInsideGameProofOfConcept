using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Inspectable : MonoBehaviour, IInteractable
{
    [TextArea]
    public string interactionText = "Inspect";

    public void Interact()
    {
        InspectionManager.Instance.StartInspection(this);
    }

    public string GetInteractionText()
    {
        return interactionText;
    }
}
