using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour, IInteractable
{
    public event Action<ItemPickup> OnPickedUp;

    [Header("Item")]
    [SerializeField] private ItemDefinition item;

    [Header("Settings")]
    [SerializeField] private PickupBehaviour pickupBehaviour = PickupBehaviour.Destroy;

    private Collider pickupCollider;

    public ItemDefinition Item => item;

    private void Awake()
    {
        pickupCollider = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (item == null)
        {
            Debug.LogWarning($"{name}: No ItemDefinition assigned.");
            return;
        }

        bool added = PlayerInventory3D.Instance.TryAddItem(item);

        if (!added)
        {
            Debug.Log($"{item.ItemName} is already owned.");
            return;
        }

        Debug.Log($"Picked up: {item.ItemName}");

        OnPickedUp?.Invoke(this);

        ApplyPickupBehaviour();
    }

    private void ApplyPickupBehaviour()
    {
        switch (pickupBehaviour)
        {
            case PickupBehaviour.Destroy:
                Destroy(gameObject);
                break;

            case PickupBehaviour.DisablePickup:

                if (pickupCollider != null)
                    pickupCollider.enabled = false;

                enabled = false;
                break;
        }
    }

    public string GetInteractionText()
    {
        if (item == null)
            return "Pick Up";

        return $"Pick Up {item.ItemName}";
    }
}
