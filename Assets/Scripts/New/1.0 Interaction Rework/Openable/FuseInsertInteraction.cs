using UnityEngine;

public class FuseInsertInteraction : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private MaintenancePanelInteraction maintenancePanel;
    [SerializeField] private OpenableState fuseState;

    private InteractionRequirements requirements;

    [Header("Interaction")]
    [SerializeField] private string interactionName = "Fuse";

    public bool FuseInserted { get; private set; }

    private void Awake()
    {
        requirements = GetComponent<InteractionRequirements>();
    }

    public string GetInteractionText()
    {
        if (FuseInserted)
            return string.Empty;

        if (!maintenancePanel.IsOpen)
            return string.Empty;

        return $"Insert {interactionName} [E]";
    }

    public void Interact()
    {
        if (FuseInserted)
            return;

        if (!maintenancePanel.IsOpen)
            return;

        if (requirements != null &&
            !requirements.AreSatisfied(out string failureReason))
        {
            Debug.Log(failureReason);
            return;
        }

        requirements?.NotifyRequirementsSatisfied();

        FuseInserted = true;

        if (fuseState != null)
        {
            fuseState.Open();
        }

        Debug.Log("Fuse inserted.");
    }
}