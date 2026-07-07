using UnityEngine;

public class ContainerInteraction : MonoBehaviour, IInteractable
{
    private ContainerState containerState;
    private InteractionRequirements requirements;

    [Header("Interaction")]
    [SerializeField] private string openVerb = "Open";
    [SerializeField] private string interactionName = "Container";

    private void Awake()
    {
        containerState = GetComponent<ContainerState>();
        requirements = GetComponent<InteractionRequirements>();
    }

    public string GetInteractionText()
    {
        if (containerState.IsOpen)
        {
            return $"Close {interactionName} [E]";
        }

        return $"{openVerb} {interactionName} [E]";
    }

    public void Interact()
    {
        // Only check requirements when opening
        if (!containerState.IsOpen)
        {
            if (requirements != null &&
                !requirements.AreSatisfied(out string failureReason))
            {
                Debug.Log(failureReason);
                return;
            }
        }

        containerState.Toggle();
    }
}
