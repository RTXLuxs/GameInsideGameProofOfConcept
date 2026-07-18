using UnityEngine;

[RequireComponent(typeof(OpenableState))]
public class MaintenancePanelInteraction : MonoBehaviour, IInteractable
{
    private OpenableState openableState;
    private InteractionRequirements requirements;

    [Header("Interaction")]
    [SerializeField] private string interactionName = "Maintenance Panel";

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        openableState = GetComponent<OpenableState>();
        requirements = GetComponent<InteractionRequirements>();
    }

    public string GetInteractionText()
    {
        if (IsOpen)
            return string.Empty;

        return $"Open {interactionName} [E]";
    }

    public void Interact()
    {
        if (IsOpen)
            return;

        if (requirements != null &&
            !requirements.AreSatisfied(out string failureReason))
        {
            Debug.Log(failureReason);
            return;
        }

        requirements?.NotifyRequirementsSatisfied();

        openableState.Open();
        IsOpen = true;
    }
}