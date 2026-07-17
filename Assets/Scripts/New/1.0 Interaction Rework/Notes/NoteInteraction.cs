using UnityEngine;

public class NoteInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite noteImage;
    [SerializeField] private string noteText;

    public string GetInteractionText()
    {
        return noteText;
    }

    public void Interact()
    {
        NoteViewer.Instance.Show(noteImage);
    }
}