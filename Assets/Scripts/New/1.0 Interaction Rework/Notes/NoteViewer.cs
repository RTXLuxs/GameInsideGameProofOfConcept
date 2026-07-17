using UnityEngine;
using UnityEngine.UI;

public class NoteViewer : MonoBehaviour
{
    private NoteInteraction noteInteraction;

    public string interaction;

    [SerializeField] private GameObject notePanel;

    private void Awake()
    {
        noteInteraction = GetComponent<NoteInteraction>();
        
        noteInteraction.noteText = interaction;

        notePanel.SetActive(false);
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
