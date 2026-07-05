using UnityEngine;

public class ItemRequirement : RequirementBase
{
    [Header("Item Requirement")]
    [SerializeField] private ItemDefinition requiredItem;

    [Header("Feedback")]
    [SerializeField] private string customFailureMessage = "";

    public ItemDefinition RequiredItem => requiredItem;

    protected override bool CheckRequirement(out string failureReason)
    {
        if (requiredItem == null)
        {
            failureReason = "No required item assigned.";
            return false;
        }

        if (PlayerInventory3D.Instance.HasItem(requiredItem))
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
            failureReason = $"You need {requiredItem.ItemName}.";
        }

        return false;
    }
}