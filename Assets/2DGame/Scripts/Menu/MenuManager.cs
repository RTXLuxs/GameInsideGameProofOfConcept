using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool opening = !menuCanvas.activeSelf;
            menuCanvas.SetActive(opening);

            if (playerMovement != null)
            {
                if (opening)
                    playerMovement.DisableControls();
                else
                    playerMovement.EnableControls();
            }
        }
    }
}
