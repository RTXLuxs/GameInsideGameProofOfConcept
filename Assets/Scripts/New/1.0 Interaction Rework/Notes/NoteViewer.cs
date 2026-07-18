using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject notePanel;

    [Header("Shared")]
    [SerializeField] private Image backgroundImage;

    [Header("Image Notes")]
    [SerializeField] private GameObject imageContainer;
    [SerializeField] private Image noteImage;

    [Header("Text Notes")]
    [SerializeField] private GameObject textContainer;
    [SerializeField] private TMP_Text noteText;

    private NoteInteraction currentNote;

    public bool IsOpen => notePanel.activeSelf;

    private void Awake()
    {
        notePanel.SetActive(false);
    }

    private void Update()
    {
        if (notePanel.activeSelf && UserInput.Instance.pausePressed)
        {
            Debug.Log("Escape pressed -> Hide()");
            Hide();
        }
    }

    public void Show(NoteInteraction note)
    {
        // Prevent opening the note again while it is already open.
        if (notePanel.activeSelf)
            return;

        Debug.Log($"Show called for {note.name}");
        currentNote = note;
        Debug.Log($"Stored currentNote = {currentNote.name}");

        notePanel.SetActive(true);

        PlayerState.Instance.DisableControls();

        currentNote.DisableInteraction();

        switch (note.Type)
        {
            case NoteInteraction.NoteType.Image:

                imageContainer.SetActive(true);
                textContainer.SetActive(false);

                noteImage.sprite = note.NoteImage;

                break;

            case NoteInteraction.NoteType.Text:

                imageContainer.SetActive(false);
                textContainer.SetActive(true);

                backgroundImage.sprite = note.BackgroundImage;
                noteText.text = note.NoteBody;

                break;
        }
    }

    public void Hide()
    {
        Debug.Log($"Hide called. currentNote = {(currentNote ? currentNote.name : "NULL")}");

        notePanel.SetActive(false);

        PlayerState.Instance.EnableControls();

        Debug.Log($"Current note: {(currentNote != null ? currentNote.name : "NULL")}");

        if (currentNote != null)
        {
            Debug.Log("Calling EnableInteraction");
            currentNote.EnableInteraction();
            currentNote = null;
        }
    }
}