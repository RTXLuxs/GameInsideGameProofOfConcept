using UnityEngine;

public class PowerRequirement : RequirementBase
{
    [Header("Power Requirement")]
    [SerializeField] private string requiredCircuitName;

    [Header("Feedback")]
    [SerializeField] private string customFailureMessage = "";

    protected override bool CheckRequirement(out string failureReason)
    {
        if (!PowerManager.Instance.HasCircuit(requiredCircuitName))
        {
            failureReason = $"Power circuit '{requiredCircuitName}' does not exist.";
            return false;
        }

        if (PowerManager.Instance.IsPowered(requiredCircuitName))
        {
            failureReason = string.Empty;
            return true;
        }

        if (!string.IsNullOrWhiteSpace(customFailureMessage))
        {
            failureReason = customFailureMessage;
        }
        else
        {
            failureReason = $"Power circuit '{requiredCircuitName}' is not powered.";
        }

        return false;
    }
}