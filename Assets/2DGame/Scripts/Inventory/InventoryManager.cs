using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    private PlayerMovement playerMovement;
    private HotbarManager hotbarManager;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public GameObject[] itemPrefabs;
    public int slotCount;
    [SerializeField] private int columns = 5;
    [SerializeField] private float dropDistance = 0.8f;

    private int selectedIndex = -1;

    public bool IsInventoryOpen
    {
        get
        {
            return inventoryPanel != null && inventoryPanel.activeInHierarchy;
        }
    }

    private void Awake()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        hotbarManager = FindAnyObjectByType<HotbarManager>();
    }

    private void Update()
    {
        if (!IsInventoryOpen) return;

        if (Input.GetKeyDown(KeyCode.D)) Navigate(1);
        if (Input.GetKeyDown(KeyCode.A)) Navigate(-1);
        if (Input.GetKeyDown(KeyCode.S)) Navigate(columns);
        if (Input.GetKeyDown(KeyCode.W)) Navigate(-columns);
        if (Input.GetKeyDown(KeyCode.G)) DropSelectedItem();

        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwapWithHotbar(i);
                break;
            }
        }
    }

    private int GetColumns()
    {
        GridLayoutGroup grid = inventoryPanel.GetComponent<GridLayoutGroup>();
        if (grid != null && grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
            return grid.constraintCount;
        return columns;
    }

    private void Navigate(int delta)
    {
        int count = inventoryPanel.transform.childCount;
        if (count == 0) return;

        int cols = GetColumns();
        int current = Mathf.Max(selectedIndex, 0);
        int currentRow = current / cols;
        int currentCol = current % cols;

        int rows = Mathf.CeilToInt((float)count / cols);

        int nextRow = currentRow;
        int nextCol = currentCol;

        if (delta == 1)       nextCol = Mathf.Min(currentCol + 1, cols - 1);
        else if (delta == -1) nextCol = Mathf.Max(currentCol - 1, 0);
        else if (delta > 1)   nextRow = Mathf.Min(currentRow + 1, rows - 1);
        else if (delta < -1)  nextRow = Mathf.Max(currentRow - 1, 0);

        int next = Mathf.Min(nextRow * cols + nextCol, count - 1);
        SetSelectedSlot(next);
    }

    private void SwapWithHotbar(int hotbarIndex)
    {
        if (selectedIndex < 0 || hotbarManager == null) return;

        Slot invSlot = inventoryPanel.transform.GetChild(selectedIndex).GetComponent<Slot>();
        if (invSlot == null || invSlot.currentItem == null) return;

        Slot hotbarSlot = hotbarManager.hotbarPanel.transform.GetChild(hotbarIndex).GetComponent<Slot>();
        if (hotbarSlot == null) return;

        int invItemID = invSlot.currentItem.GetComponent<Item>().ID;
        int hotbarItemID = hotbarSlot.currentItem != null ? hotbarSlot.currentItem.GetComponent<Item>().ID : -1;

        Destroy(invSlot.currentItem);
        invSlot.currentItem = null;

        if (hotbarSlot.currentItem != null)
        {
            Destroy(hotbarSlot.currentItem);
            hotbarSlot.currentItem = null;
        }

        // Place inventory item into hotbar slot
        GameObject hotbarPrefab = itemDictionary.GetItemPrefab(invItemID);
        if (hotbarPrefab != null)
        {
            GameObject newHotbarItem = Instantiate(hotbarPrefab, hotbarSlot.transform);
            newHotbarItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            hotbarSlot.currentItem = newHotbarItem;
        }

        // Place old hotbar item into inventory slot
        if (hotbarItemID != -1)
        {
            GameObject invPrefab = itemDictionary.GetItemPrefab(hotbarItemID);
            if (invPrefab != null)
            {
                GameObject newInvItem = Instantiate(invPrefab, invSlot.transform);
                newInvItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                invSlot.currentItem = newInvItem;
            }
        }
    }

    private void DropSelectedItem()
    {
        if (selectedIndex < 0 || playerMovement == null) return;

        Slot slot = inventoryPanel.transform.GetChild(selectedIndex).GetComponent<Slot>();
        if (slot == null || slot.currentItem == null) return;

        Item item = slot.currentItem.GetComponent<Item>();
        GameObject prefab = itemDictionary.GetItemPrefab(item.ID);
        if (prefab == null) return;

        Vector2 dropPosition = (Vector2)playerMovement.transform.position + Random.insideUnitCircle.normalized * dropDistance;

        GameObject dropped = Instantiate(prefab, dropPosition, Quaternion.identity);
        dropped.name = item.name;

        Destroy(slot.currentItem);
        slot.currentItem = null;
    }

    private void SetSelectedSlot(int index)
    {
        int count = inventoryPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Slot slot = inventoryPanel.transform.GetChild(i).GetComponent<Slot>();
            if (slot != null) slot.SetHighlight(i == index);
        }
        selectedIndex = index;
    }

    private void ClearSelection()
    {
        int count = inventoryPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Slot slot = inventoryPanel.transform.GetChild(i).GetComponent<Slot>();
            if (slot != null) slot.SetHighlight(false);
        }
        selectedIndex = -1;
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            SetSelectedSlot(0);
        }
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            ClearSelection();
            inventoryPanel.SetActive(false);
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            if (inventoryPanel.activeSelf)
                CloseInventory();
            else
                OpenInventory();
        }
    }
    
    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.LogWarning("Inventory is full! Cannot add item: " + itemPrefab.name);
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                invData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex()
                });
            }
        }

        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform, false);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}