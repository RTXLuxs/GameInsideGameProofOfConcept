using UnityEngine;

public class PlayerIntroEvents : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerWheelchair;
    [SerializeField] private Animator animator;

    private GameObject worldWheelchair;

    public void BeginWheelchairSequence(GameObject wheelchair)
    {
        worldWheelchair = wheelchair;
    }

    // Animation Event
    public void FinishWheelchairInteraction()
    {
        if (playerWheelchair != null)
        {
            playerWheelchair.SetActive(true);
            animator.enabled = false;
        }

        if (worldWheelchair != null)
        {
            Destroy(worldWheelchair);
        }

        Debug.Log("Player entered the wheelchair.");
    }
}