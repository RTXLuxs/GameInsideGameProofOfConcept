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

    private AudioSource audioSource;

    private NoteInteraction currentNote;

    private void Awake()
    {
        notePanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (notePanel.activeSelf && UserInput.Instance.pausePressed)
        {
            Hide();
        }
    }

    public void Show(NoteInteraction note)
    {
        currentNote = note;

        notePanel.SetActive(true);

        audioSource.PlayOneShot(audioSource.clip);

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
        notePanel.SetActive(false);

        PlayerState.Instance.EnableControls();

        if (currentNote != null)
        {
            currentNote.EnableInteraction();
            currentNote = null;
        }
    }
}