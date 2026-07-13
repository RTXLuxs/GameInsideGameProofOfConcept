using UnityEngine;

public class WheelchairInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string interactionText = "Sit in Wheelchair [E]";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "SitInWheelchair";

    [Header("Player")]
    [SerializeField] private PlayerIntroEvents playerIntroEvents;

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
            // Tell the player which wheelchair should be removed
            playerIntroEvents.BeginWheelchairSequence(gameObject);

            animator.enabled = true;
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning($"{name}: No Animator assigned.");
        }

        Debug.Log("Wheelchair interaction triggered.");
    }
}
