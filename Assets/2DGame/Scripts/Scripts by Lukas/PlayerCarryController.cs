using UnityEngine;

public class PlayerCarryController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform carryPoint;

    private CarryableItem carriedItem;

    public bool IsCarrying => carriedItem != null;
    public Transform CarryPoint => carryPoint;

    /// <summary>
    /// Attempts to pick up the given item.
    /// Returns true if successful.
    /// </summary>
    public bool Pickup(CarryableItem item)
    {
        if (IsCarrying || item == null)
            return false;

        carriedItem = item;
        return true;
    }

    /// <summary>
    /// Clears the currently carried item.
    /// The CarryableItem itself handles restoring physics.
    /// </summary>
    public void ClearCarriedItem()
    {
        carriedItem = null;
    }

    public CarryableItem GetCarriedItem()
    {
        return carriedItem;
    }

    public void Drop()
    {
        if (!IsCarrying)
            return;

        carriedItem.Drop();
    }
}