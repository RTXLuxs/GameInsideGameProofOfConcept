using UnityEngine;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    private NoteInteraction noteInteraction;

    public string interaction;

    [SerializeField] private GameObject notePanel;

    AudioSource audioSource;

    private void Awake()
    {
        noteInteraction = GetComponent<NoteInteraction>();
        
        noteInteraction.noteText = interaction;

        notePanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (UserInput.Instance.pausePressed)
        {
            Hide();
            Debug.Log("this");
        }
    }

    public void Show(Sprite note)
    {
        notePanel.SetActive(true);

        audioSource.PlayOneShot(audioSource.clip);

        PlayerState.Instance.DisableControls();

        noteInteraction.noteText = "";

        notePanel.GetComponent<Image>().sprite = note;

        // TODO:
        // Set the UI Image sprite

        // TODO:
        // Unlock cursor

        // TODO:
        // Disable player controls
    }

    public void Hide()
    {
        notePanel.SetActive(false);

        noteInteraction.noteText = interaction;

        PlayerState.Instance.EnableControls();
        // TODO:
        // Lock cursor

        // TODO:
        // Enable player controls
    }
}
