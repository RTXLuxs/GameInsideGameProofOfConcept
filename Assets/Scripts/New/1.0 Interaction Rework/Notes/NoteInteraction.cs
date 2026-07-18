using UnityEngine;

public class NoteInteraction : MonoBehaviour, IInteractable
{
    public enum NoteType
    {
        Image,
        Text
    }

    [Header("Interaction")]
    [SerializeField] private string interactionText = "Read Note";
    private string interactionTextPassed;
    [SerializeField] private NoteViewer noteViewer;

    [Header("Type")]
    [SerializeField] private NoteType noteType;

    [Header("Image Note")]
    [SerializeField] private Sprite noteImage;

    [Header("Text Note")]
    [SerializeField] private Sprite backgroundImage;

    [TextArea(10, 30)]
    [SerializeField] private string noteBody;

    public NoteType Type => noteType;
    public Sprite NoteImage => noteImage;
    public Sprite BackgroundImage => backgroundImage;
    public string NoteBody => noteBody;

    private void Awake()
    {
        interactionTextPassed = interactionText;
    }

    public string GetInteractionText()
    {
        return interactionTextPassed;
    }

    public void Interact()
    {
        noteViewer.Show(this);
    }

    public void DisableInteraction()
    {
        interactionTextPassed = "";
    }

    public void EnableInteraction()
    {
        interactionTextPassed = interactionText;
    }
}