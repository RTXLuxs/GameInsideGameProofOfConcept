using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory3D : MonoBehaviour
{
    public static PlayerInventory3D Instance;

    public event Action<ItemDefinition> OnItemAdded;
    public event Action<ItemDefinition> OnItemRemoved;

    private readonly HashSet<ItemDefinition> ownedItems = new();

    public int Count => ownedItems.Count;

    public IEnumerable<ItemDefinition> Items => ownedItems;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool TryAddItem(ItemDefinition item)
    {
        if (item == null)
            return false;

        if (!ownedItems.Add(item))
            return false;

        Debug.Log($"Inventory: Added '{item.ItemName}'.");

        OnItemAdded?.Invoke(item);

        return true;
    }

    public bool RemoveItem(ItemDefinition item)
    {
        if (item == null)
            return false;

        if (!ownedItems.Remove(item))
            return false;

        OnItemRemoved?.Invoke(item);

        return true;
    }

    public bool HasItem(ItemDefinition item)
    {
        if (item == null)
            return false;

        return ownedItems.Contains(item);
    }

    public void ClearInventory()
    {
        ownedItems.Clear();
    }
}