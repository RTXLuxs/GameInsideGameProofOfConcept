using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveLocation;
    private InventoryManager inventoryManager;
    private HotbarManager hotbarManager;
    public Container[] containersInScene;

    void Awake()
    {
        InitializeComponents();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created     
    void Start()
    {
        LoadGame();
    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        hotbarManager = FindAnyObjectByType<HotbarManager>();
        containersInScene = FindObjectsByType<Container>();
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("SaveManager: Player with tag 'Player' not found. Cannot save game.");
            return;
        }

        inventoryManager ??= FindAnyObjectByType<InventoryManager>();
        hotbarManager ??= FindAnyObjectByType<HotbarManager>();

        SaveData saveData = new SaveData
        {
            playerPos = player.transform.position,
            mapBoundry = string.Empty,
            inventorySaveData = inventoryManager != null ? inventoryManager.GetInventoryItems() : new System.Collections.Generic.List<InventorySaveData>(),
            hotbarSaveData = hotbarManager != null ? hotbarManager.GetHotbarItems() : new System.Collections.Generic.List<InventorySaveData>(),
            equippedItemID = hotbarManager != null ? hotbarManager.GetEquippedItemID() : -1,
            containerSaveData = GetContainerState()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData, true));
        Debug.Log($"SaveManager: Game saved to {saveLocation}");
    }

    private List<ContainerSaveData> GetContainerState()
    {
        List<ContainerSaveData> containers = new List<ContainerSaveData>();

        foreach (Container container in containersInScene)
        {
            ContainerSaveData containerSaveData = new ContainerSaveData
            {
                containerID = container.ContainerID,
                isOpened = container.IsOpened
            };
            containers.Add(containerSaveData);
        }
        return containers;  
    }

    public void ResetGame()
    {
        if (File.Exists(saveLocation))
            File.Delete(saveLocation);

        // Reset player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.transform.position = Vector3.zero;

        // Reset inventory and hotbar
        inventoryManager?.SetInventoryItems(new List<InventorySaveData>());
        hotbarManager?.SetHotbarItems(new List<InventorySaveData>());
        hotbarManager?.SetEquippedItem(-1);

        // Reset all containers
        foreach (Container container in containersInScene)
            container.SetOpened(false);

        // Write the clean state to disk
        SaveGame();
    }

    // Load the game data from the save file
    public void LoadGame()
    {
        inventoryManager ??= FindAnyObjectByType<InventoryManager>();
        hotbarManager ??= FindAnyObjectByType<HotbarManager>();

        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            if (saveData == null)
            {
                Debug.LogWarning($"SaveManager: Save file at {saveLocation} could not be parsed. Creating a fresh save.");
                SaveGame();
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = saveData.playerPos;

            if (inventoryManager != null)
                inventoryManager.SetInventoryItems(saveData.inventorySaveData);
            else
                Debug.LogWarning("SaveManager: InventoryManager not found when loading saved inventory.");

            if (hotbarManager != null)
            {
                hotbarManager.SetHotbarItems(saveData.hotbarSaveData);
                hotbarManager.SetEquippedItem(saveData.equippedItemID);
            }
            else
                Debug.LogWarning("SaveManager: HotbarManager not found when loading saved hotbar.");

            // Load container states
            LoadContainerStates(saveData.containerSaveData);
        }
        else
        {
            SaveGame();

            inventoryManager.SetInventoryItems(new List<InventorySaveData>());
            hotbarManager.SetHotbarItems(new List<InventorySaveData>());
            Debug.LogWarning("No save file found at " + saveLocation);
        }
    }

    private void LoadContainerStates(List<ContainerSaveData> containerStates)
    {
        foreach (Container container in containersInScene)
        {
            ContainerSaveData containerSaveData = containerStates.FirstOrDefault(c => c.containerID == container.ContainerID);
            if (containerSaveData != null)
            {
                container.SetOpened(containerSaveData.isOpened);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
