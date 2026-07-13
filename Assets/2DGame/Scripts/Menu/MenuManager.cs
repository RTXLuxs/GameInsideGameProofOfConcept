using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas;

    private PlayerMovement playerMovement;
    private InventoryManager inventoryManager;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        // Only allow the mini game controls while the player is looking at the tablet.
        bool inTabletView = PlayerState.Instance != null && PlayerState.Instance.isPCMode;

        // If the player put the tablet away while the menu was open, force it closed.
        // PlayerState.ExitPC already disabled 2D movement, so don't re-enable it here.
        if (!inTabletView)
        {
            if (menuCanvas.activeSelf)
                CloseMenu(restoreMovement: false);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool opening = !menuCanvas.activeSelf;
            if (opening)
                OpenMenu();
            else
                CloseMenu(restoreMovement: true);
        }
    }

    private void OpenMenu()
    {
        menuCanvas.SetActive(true);

        if (playerMovement != null)
            playerMovement.DisableControls();

        if (inventoryManager != null)
            inventoryManager.OpenInventory();
    }

    private void CloseMenu(bool restoreMovement)
    {
        menuCanvas.SetActive(false);

        if (restoreMovement && playerMovement != null)
            playerMovement.EnableControls();

        if (inventoryManager != null)
            inventoryManager.CloseInventory();
    }
}
