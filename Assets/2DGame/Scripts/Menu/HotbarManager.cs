using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarManager : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 6;
    [SerializeField] private Transform equippedItemPosition;
    [SerializeField] private Transform equippedItemPositionLeft;
    private GameObject currentlyEquippedItem;

    private ItemDictionary itemDictionary;
    private PlayerMovement playerMovement;
    private Vector2 lastMoveDirection;

    [SerializeField] private float throwSpeed = 8f;
    [SerializeField] private float throwDistance = 5f;
    [SerializeField] private float dropDistance = 0.8f;
    [SerializeField] private LayerMask obstacleLayer;

    private Key[] hotbarKeys;
    private int selectedSlotIndex = -1;

    private void Awake()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        hotbarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            // Map hotbar keys to 1, 2, 3, etc., with the last slot mapped to 6
            hotbarKeys[i] = i < (slotCount - 1) ? (Key)((int)Key.Digit1 + i) : Key.Digit6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLastMoveDirection();
        RefreshEquippedItemPosition();

        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
               SelectItemInSlot(i);
        }

        if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
            CycleSelection(1);

        if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
            CycleSelection(-1);

        if (Keyboard.current[Key.Space].wasPressedThisFrame)
            ThrowSelectedItem();

        if (Keyboard.current[Key.G].wasPressedThisFrame)
            DropSelectedItem();
    }

    private void CycleSelection(int direction)
    {
        int next = (selectedSlotIndex + direction + slotCount) % slotCount;
        SelectItemInSlot(next);
    }

    private void UpdateLastMoveDirection()
    {
        if (playerMovement == null)
            return;

        Vector2 currentInput = playerMovement.MoveInput;
        if (currentInput != Vector2.zero)
            lastMoveDirection = currentInput.normalized;
    }

    // Method to select the item in the specified hotbar slot and equip it on the player
    void SelectItemInSlot(int slotIndex)
    {
        Slot slot = hotbarPanel.transform.GetChild(slotIndex).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            selectedSlotIndex = slotIndex;
            Item item = slot.currentItem.GetComponent<Item>();
            EquipItem(item);
        }
        else
        {
            selectedSlotIndex = -1;
            UnequipCurrentItem();
        }
    }

    private void DropSelectedItem()
    {
        if (selectedSlotIndex < 0 || playerMovement == null) return;

        Slot slot = hotbarPanel.transform.GetChild(selectedSlotIndex).GetComponent<Slot>();
        if (slot.currentItem == null) return;

        Item item = slot.currentItem.GetComponent<Item>();
        GameObject prefab = itemDictionary.GetItemPrefab(item.ID);
        if (prefab == null) return;

        Vector2 dropPosition = (Vector2)playerMovement.transform.position + Random.insideUnitCircle.normalized * dropDistance;

        GameObject dropped = Instantiate(prefab, dropPosition, Quaternion.identity);
        dropped.name = item.name;

        Destroy(slot.currentItem);
        slot.currentItem = null;
        selectedSlotIndex = -1;
        UnequipCurrentItem();
    }

    private void ThrowSelectedItem()
    {
        if (selectedSlotIndex < 0 || playerMovement == null) return;

        Slot slot = hotbarPanel.transform.GetChild(selectedSlotIndex).GetComponent<Slot>();
        if (slot.currentItem == null) return;

        Item item = slot.currentItem.GetComponent<Item>();
        GameObject prefab = itemDictionary.GetItemPrefab(item.ID);
        if (prefab == null) return;

        Vector2 spawnPos = playerMovement.transform.position;
        Vector2 facing = lastMoveDirection != Vector2.zero ? lastMoveDirection : Vector2.down;
        Vector2 throwTarget = spawnPos + facing * throwDistance;

        GameObject thrown = Instantiate(prefab, spawnPos, Quaternion.identity);
        thrown.AddComponent<ThrownItem>().Init(throwTarget, throwSpeed, obstacleLayer);
        thrown.GetComponent<ThrownItem>().audioProxy = thrown.GetComponent<Item>().audioProxy;
        thrown.GetComponent<ThrownItem>().impactClip = thrown.GetComponent<Item>().impactClip;

        Destroy(slot.currentItem);
        slot.currentItem = null;
        selectedSlotIndex = -1;
        UnequipCurrentItem();
    }

    private void EquipItem(Item item)
    {
        // Destroy currently equipped item if any
        UnequipCurrentItem();

        Transform targetPosition = GetCurrentEquipTransform();
        if (targetPosition != null && itemDictionary != null)
        {
            GameObject itemPrefab = itemDictionary.GetItemPrefab(item.ID);
            if (itemPrefab != null)
            {
                currentlyEquippedItem = Instantiate(itemPrefab, targetPosition);
                ApplyEquippedItemTransform(currentlyEquippedItem);
            }
        }
    }

    private void RefreshEquippedItemPosition()
    {
        if (currentlyEquippedItem == null)
            return;

        Transform targetPosition = GetCurrentEquipTransform();
        if (targetPosition == null)
            return;

        if (currentlyEquippedItem.transform.parent != targetPosition)
        {
            currentlyEquippedItem.transform.SetParent(targetPosition, false);
        }

        ApplyEquippedItemTransform(currentlyEquippedItem);
    }

    private void ApplyEquippedItemTransform(GameObject equippedItem)
    {
        float scaleMultiplier = 0.6f; // Adjust this value as needed for proper sizing
        equippedItem.transform.localPosition = Vector2.zero;
        equippedItem.transform.localRotation = Quaternion.identity;
        equippedItem.transform.localScale = Vector2.one * scaleMultiplier;
        SetLayerRecursive(equippedItem, 5);
        SetSortingOrderRecursive(equippedItem, 5);
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }

    private void SetSortingOrderRecursive(GameObject obj, int sortingOrder)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
        foreach (Transform child in obj.transform)
        {
            SetSortingOrderRecursive(child.gameObject, sortingOrder);
        }
    }

    private Transform GetCurrentEquipTransform()
    {
        if (lastMoveDirection.x < 0f && equippedItemPositionLeft != null)
            return equippedItemPositionLeft;

        return equippedItemPosition;
    }

    private void UnequipCurrentItem()
    {
        if (currentlyEquippedItem != null)
        {
            Destroy(currentlyEquippedItem);
            currentlyEquippedItem = null;
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in hotbarPanel.transform)
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
        return false;
    }

    public int GetEquippedItemID()
    {
        if (currentlyEquippedItem != null)
        {
            Item item = currentlyEquippedItem.GetComponent<Item>();
            return item != null ? item.ID : -1;
        }
        return -1;
    }

    public void SetEquippedItem(int itemID)
    {
        if (itemID == -1)
        {
            UnequipCurrentItem();
            return;
        }

        if (itemDictionary != null)
        {
            GameObject itemPrefab = itemDictionary.GetItemPrefab(itemID);
            if (itemPrefab != null)
            {
                Item item = itemPrefab.GetComponent<Item>();
                if (item != null)
                {
                    EquipItem(item);
                }
            }
        }
    }

    // Method to retrieve the current inventory items and their slot indices for saving
    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach(Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
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

    // Method to set inventory items based on saved data
    public void SetHotbarItems(List<InventorySaveData> hotbarSaveData)
    {
        // Clear existing items from the inventory
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Recreate inventory slots 
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        // Populate inventory slots with items based on the saved data
        foreach (InventorySaveData data in hotbarSaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
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
