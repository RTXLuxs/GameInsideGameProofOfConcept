using UnityEngine;

public class NoteViewer : MonoBehaviour
{
    public static NoteViewer Instance;

    [SerializeField] private GameObject notePanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        notePanel.SetActive(false);
    }

    private void Update()
    {
        if (UserInput.Instance.pausePressed)
        {
            Hide();
        }
    }

    public void Show(Sprite note)
    {
        notePanel.SetActive(true);

        PlayerState.Instance.DisableControls();

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

        PlayerState.Instance.EnableControls();
        // TODO:
        // Lock cursor

        // TODO:
        // Enable player controls
    }
}
