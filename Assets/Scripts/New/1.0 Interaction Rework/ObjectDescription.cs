using UnityEngine;

public class ObjectDescription : MonoBehaviour, IInteractable
{
    [TextArea]
    [SerializeField] private string text;

    public string GetInteractionText()
    {
        return text;
    }

    public void Interact()
    {
        // Intentionally left blank.
    }
}
