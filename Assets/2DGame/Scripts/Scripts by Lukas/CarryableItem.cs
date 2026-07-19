using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CarryableItem : MonoBehaviour, IInteractable2D
{
    private Rigidbody2D rb;
    private Collider2D col;

    private PlayerCarryController carrier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public bool CanInteract()
    {
        // Can't interact with an item that's already being carried.
        return carrier == null;
    }

    public void Interact()
    {
        // Find the player.
        PlayerCarryController playerCarry = FindFirstObjectByType<PlayerCarryController>();

        if (playerCarry == null)
        {
            Debug.LogError("No PlayerCarryController found in the scene.");
            return;
        }

        // Already carrying something?
        if (!playerCarry.Pickup(this))
            return;

        carrier = playerCarry;

        // Disable physics while carried.
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;

        // Disable collisions.
        col.enabled = false;

        // Attach to the player's carry point.
        transform.SetParent(playerCarry.CarryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        Debug.Log("Drop() called");

        if (carrier == null)
        {
            Debug.Log("Carrier is null!");
            return;
        }

        Vector3 dropPosition = transform.position;

        transform.SetParent(null);
        transform.position = dropPosition;

        rb.simulated = true;
        col.enabled = true;

        carrier.ClearCarriedItem();
        carrier = null;

        Debug.Log("Dropped successfully");
    }
}