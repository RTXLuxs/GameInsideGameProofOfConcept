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

            if (inventoryManager != null)
            {
                if (opening)
                    inventoryManager.OpenInventory();
                else
                    inventoryManager.CloseInventory();
            }
        }
    }
}
