using UnityEngine;

public class NoteInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite noteImage;
    [HideInInspector] public string noteText;
    public NoteViewer noteViewer;

    public string GetInteractionText()
    {
        return noteText;
    }

    public void Interact()
    {
        noteViewer.Show(noteImage);
    }
}