using UnityEngine;

public class GrabTabletInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string interactionText = "Pick up Tablet [E]";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "PickupTablet";

    [Header("Player Tablet")]
    [SerializeField] private GameObject playerTablet;

    private bool hasInteracted = false;

    public string GetInteractionText()
    {
        return hasInteracted ? string.Empty : interactionText;
    }

    public void Interact()
    {
        if (hasInteracted)
            return;

        hasInteracted = true;

        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning($"{name}: No Animator assigned.");
        }

        Debug.Log("Tablet interaction triggered.");
    }

    // Called by an Animation Event at the end of the pickup animation
    public void FinishPickup()
    {
        if (playerTablet != null)
        {
            playerTablet.SetActive(true);
        }

        Debug.Log("Tablet pickup completed.");

        Destroy(gameObject);
    }
}
