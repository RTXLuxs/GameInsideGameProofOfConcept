using UnityEngine;

/// <summary>
/// A stairs trigger that teleports the player to another floor built elsewhere in the same
/// scene, snapping the camera so there's no long pan across the map.
///
/// Place one on each floor's stairs and point its <see cref="destination"/> at the OTHER
/// floor's landing spot. Put that landing just OFF the other stairs' trigger so the player
/// doesn't immediately teleport back (a short shared cooldown guards against that too).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class StairsTeleport : MonoBehaviour
{
    [Tooltip("Where the player arrives — usually a landing next to the other floor's stairs.")]
    [SerializeField] private Transform destination;
    [Tooltip("Seconds after any teleport during which no stairs will fire again (prevents bounce-back).")]
    [SerializeField] private float retriggerCooldown = 0.4f;

    // Shared across all stairs so arriving on the far stairs can't instantly send you back.
    private static float cooldownUntil;

    private void Reset()
    {
        // Detection needs a trigger; default it when the component is first added.
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < cooldownUntil || destination == null) return;

        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player == null) return;

        cooldownUntil = Time.time + retriggerCooldown;

        Vector3 dest = destination.position;
        dest.z = player.transform.position.z;   // keep the player's original depth

        // Move the rigidbody (kills momentum) and the transform (so the camera reads the new spot now).
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = dest;
            rb.linearVelocity = Vector2.zero;
        }
        player.transform.position = dest;

        // Cut the camera to the new floor instead of letting it pan the whole map.
        CameraFollow cam = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
        if (cam == null) cam = FindAnyObjectByType<CameraFollow>();
        if (cam != null) cam.SnapToTarget();
    }
}
