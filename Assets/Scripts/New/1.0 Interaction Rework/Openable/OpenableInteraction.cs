using UnityEngine;

[RequireComponent(typeof(OpenableState))]
public class OpenableInteraction : MonoBehaviour, IInteractable
{
    private OpenableState openableState;
    private InteractionRequirements requirements;

    [Header("Interaction")]
    [SerializeField] private string openVerb = "Open";
    [SerializeField] private string closeVerb = "Close";
    [SerializeField] private string interactionName = "Object";

    private void Awake()
    {
        openableState = GetComponent<OpenableState>();
        requirements = GetComponent<InteractionRequirements>();
    }

    public string GetInteractionText()
    {
        if (openableState.IsOpen)
        {
            return $"{closeVerb} {interactionName} [E]";
        }

        return $"{openVerb} {interactionName} [E]";
    }

    public void Interact()
    {
        // Only check requirements when opening
        if (!openableState.IsOpen)
        {
            if (requirements != null &&
                !requirements.AreSatisfied(out string failureReason))
            {
                Debug.Log(failureReason);
                return;
            }
        }

        openableState.Toggle();
    }
}