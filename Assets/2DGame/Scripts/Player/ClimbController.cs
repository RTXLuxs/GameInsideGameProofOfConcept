using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lets the 2D player climb onto <see cref="Climbable"/> objects.
///
/// Flow:
///  1. Walk near a climbable object; a prompt (optional) shows it can be climbed.
///  2. Press the climb key (default C) to mount: the player's sprite is raised so it
///     draws above furniture, and collisions with climbables are disabled so the player
///     can walk on top. Movement is clamped to the mounted object's collider bounds, so
///     the player can roam across its surface but not step off into the room.
///  3. Press the climb key again to dismount and restore normal sorting/collision.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ClimbController : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Key that mounts/dismounts a nearby climbable object.")]
    [SerializeField] private Key climbKey = Key.C;

    [Header("Detection")]
    [Tooltip("How far from the player (world units) to look for a climbable object.")]
    [SerializeField] private float reach = 0.6f;
    [Tooltip("Layers to search for climbable objects. Leave as Everything unless climbables are on a dedicated layer.")]
    [SerializeField] private LayerMask climbableMask = ~0;

    [Header("On top")]
    [Tooltip("Added to the player sprite's sorting order while climbed, so it draws above furniture.")]
    [SerializeField] private int mountedSortingBoost = 100;
    [Tooltip("Shrinks the walkable area inside the object's collider. 0 = the player can reach the object's edges; larger keeps the player more toward the centre.")]
    [SerializeField] private float edgePadding = 0f;
    [Tooltip("Optional 'Press C to climb' prompt, shown while a climbable is in reach.")]
    [SerializeField] private GameObject climbPrompt;

    private Collider2D playerCol;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;

    private bool isClimbing;
    private Climbable current;   // the object currently mounted
    private int baseSortingOrder;
    private readonly List<Climbable> ignored = new();

    public bool IsClimbing => isClimbing;

    private void Awake()
    {
        playerCol = GetComponent<Collider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        if (climbPrompt != null)
            climbPrompt.SetActive(false);
    }

    private void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // Only respond while the 2D player is the one being controlled.
        if (playerMovement != null && !playerMovement.CanMove)
        {
            if (climbPrompt != null) climbPrompt.SetActive(false);
            return;
        }

        // Show/hide the prompt based on whether a climbable is in reach (only when grounded).
        if (!isClimbing && climbPrompt != null)
            climbPrompt.SetActive(FindClimbableInReach() != null);

        if (!kb[climbKey].wasPressedThisFrame) return;

        if (isClimbing)
            Dismount();
        else
        {
            Climbable target = FindClimbableInReach();
            if (target != null)
                Mount(target);
        }
    }

    // Keep the player within the mounted object's bounds. Runs after movement each frame
    // so the visible position never leaves the object, regardless of script order.
    private void LateUpdate()
    {
        if (!isClimbing || current == null || current.Col == null) return;

        Bounds area = current.Col.bounds;
        Vector2 center = playerCol.bounds.center;

        float minX = area.min.x + edgePadding;
        float maxX = area.max.x - edgePadding;
        float minY = area.min.y + edgePadding;
        float maxY = area.max.y - edgePadding;

        // If padding leaves no room on an axis, pin to the object's centre there.
        float cx = (minX <= maxX) ? Mathf.Clamp(center.x, minX, maxX) : area.center.x;
        float cy = (minY <= maxY) ? Mathf.Clamp(center.y, minY, maxY) : area.center.y;
        Vector2 clampedCenter = new Vector2(cx, cy);

        if (clampedCenter == center) return;

        // Convert the clamped collider centre back into a body position (accounts for collider offset).
        Vector2 offset = (Vector2)playerCol.bounds.center - playerRb.position;
        playerRb.position = clampedCenter - offset;
        playerRb.linearVelocity = Vector2.zero;
    }

    private Climbable FindClimbableInReach()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerCol.bounds.center, reach, climbableMask);
        foreach (Collider2D h in hits)
        {
            if (h == null || h == playerCol) continue;

            Climbable c = h.GetComponentInParent<Climbable>();
            if (c != null) return c;
        }
        return null;
    }

    private void Mount(Climbable target)
    {
        isClimbing = true;
        current = target;

        // Draw the player above furniture while up top.
        if (playerSprite != null)
        {
            baseSortingOrder = playerSprite.sortingOrder;
            playerSprite.sortingOrder = baseSortingOrder + mountedSortingBoost;
        }

        // Ignore collisions with every climbable so the player can walk on/across them.
        ignored.Clear();
        foreach (Climbable c in FindObjectsByType<Climbable>(FindObjectsSortMode.None))
        {
            if (c.Col == null) continue;
            Physics2D.IgnoreCollision(playerCol, c.Col, true);
            ignored.Add(c);
        }

        if (climbPrompt != null)
            climbPrompt.SetActive(false);
    }

    private void Dismount()
    {
        isClimbing = false;
        current = null;

        if (playerSprite != null)
            playerSprite.sortingOrder = baseSortingOrder;

        foreach (Climbable c in ignored)
        {
            if (c != null && c.Col != null)
                Physics2D.IgnoreCollision(playerCol, c.Col, false);
        }
        ignored.Clear();
    }
}
