using System.Collections;
using UnityEngine;

/// <summary>
/// Lets the 2D player push and pull <see cref="Pushable"/> objects.
///
/// Flow:
///  1. Walk into a pushable and keep holding the direction against it for <see cref="pushHoldTime"/>.
///  2. The player "attaches" to it along that axis.
///  3. While attached, holding toward the object pushes it, holding away pulls it, one grid
///     cell at a time, chaining continuously while the key is held (each step is smoothed).
///  4. Releasing the input or pressing a perpendicular direction detaches.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PushPullController : MonoBehaviour
{
    [Header("Grid")]
    [Tooltip("Size of one grid cell in world units. The scene Grid uses 0.5.")]
    [SerializeField] private float cellSize = 0.5f;

    [Header("Feel")]
    [Tooltip("How long the player must push against an object before attaching (seconds).")]
    [SerializeField] private float pushHoldTime = 0.5f;
    [Tooltip("Time to slide one cell. Lower = snappier, higher = smoother/slower.")]
    [SerializeField] private float stepDuration = 0.15f;
    [Tooltip("How long the player can release the input before detaching. Gives time to reverse from push to pull.")]
    [SerializeField] private float detachGrace = 0.2f;

    [Header("Collision")]
    [Tooltip("Layers that block a push/pull (walls, other objects). Leave as Everything unless the floor has a solid collider.")]
    [SerializeField] private LayerMask obstacleMask = ~0;
    [Tooltip("How far ahead to look for a pushable object.")]
    [SerializeField] private float probeDistance = 0.1f;

    [Header("UI")]
    [Tooltip("Optional 'Attached' label shown while the player is attached to an object.")]
    [SerializeField] private GameObject attachedPrompt;

    private Rigidbody2D playerRb;
    private Collider2D playerCol;
    private PlayerMovement playerMovement;
    private ClimbController climb;

    private Pushable attached;
    private Vector2 axis;        // unit direction from player toward the attached object

    /// <summary>True while the player is attached to (pushing/pulling) an object.</summary>
    public bool IsAttached => attached != null;
    private float holdTimer;
    private float releaseTimer;  // how long input has been released while attached
    private bool stepping;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        climb = GetComponent<ClimbController>();

        if (attachedPrompt != null)
            attachedPrompt.SetActive(false);
    }

    private void Update()
    {
        // No pushing/pulling while climbing on top of an object.
        if (climb != null && climb.IsClimbing)
        {
            if (attached != null)
                Detach();
            return;
        }

        if (attached != null)
            HandleAttached();
        else
            HandleDetection();
    }

    // --- Detection ------------------------------------------------------------

    private void HandleDetection()
    {
        Vector2 dir = DominantDir(playerMovement.MoveInput);
        if (dir == Vector2.zero)
        {
            holdTimer = 0f;
            return;
        }

        Pushable target = ProbePushable(dir);
        if (target == null)
        {
            holdTimer = 0f;
            return;
        }

        holdTimer += Time.deltaTime;
        if (holdTimer >= pushHoldTime)
            Attach(target, dir);
    }

    private Pushable ProbePushable(Vector2 dir)
    {
        Vector2 size = playerCol.bounds.size * 0.8f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRb.position, size, 0f, dir, probeDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || hit.collider.isTrigger) continue;

            Pushable p = hit.collider.GetComponent<Pushable>();
            if (p != null) return p;
        }
        return null;
    }

    private void Attach(Pushable target, Vector2 dir)
    {
        attached = target;
        axis = dir;
        holdTimer = 0f;
        releaseTimer = 0f;

        playerRb.linearVelocity = Vector2.zero;
        playerMovement.gridLocked = true;

        if (attachedPrompt != null)
            attachedPrompt.SetActive(true);

        // Align the player onto the object's line so push/pull travels straight.
        Vector2 perp = new Vector2(axis.y, axis.x);
        Vector2 boxPos = attached.Body.position;
        Vector2 playerPos = playerRb.position;
        float offset = Vector2.Dot(boxPos - playerPos, perp);
        playerRb.position = playerPos + perp * offset;
    }

    // --- Attached: push / pull ------------------------------------------------

    private void HandleAttached()
    {
        // Guard against the object being destroyed while attached.
        if (attached == null || attached.Body == null)
        {
            Detach();
            return;
        }

        // Let the current cell-slide finish before reacting to new input,
        // so the object always ends up cell-aligned.
        if (stepping) return;

        Vector2 dir = DominantDir(playerMovement.MoveInput);

        if (dir == axis)
        {
            releaseTimer = 0f;
            TryStep(+1);        // push
        }
        else if (dir == -axis)
        {
            releaseTimer = 0f;
            TryStep(-1);        // pull
        }
        else if (dir == Vector2.zero)
        {
            // Released: wait a short grace so a quick push -> pull reversal doesn't detach.
            releaseTimer += Time.deltaTime;
            if (releaseTimer >= detachGrace)
                Detach();
        }
        else
        {
            Detach();           // pressed a perpendicular direction
        }
    }

    private void TryStep(int sign)
    {
        Vector2 move = axis * sign * cellSize;
        Vector2 boxTarget = attached.Body.position + move;
        Vector2 playerTarget = playerRb.position + move;

        // Pushing: the object leads, so its destination cell must be clear.
        // Pulling: the player leads, so the cell behind the player must be clear.
        bool clear = sign > 0
            ? CellClear(boxTarget, attached.Col)
            : CellClear(playerTarget, playerCol);

        if (!clear) return;

        StartCoroutine(StepRoutine(playerTarget, boxTarget));
    }

    private IEnumerator StepRoutine(Vector2 playerTarget, Vector2 boxTarget)
    {
        stepping = true;

        Vector2 playerStart = playerRb.position;
        Vector2 boxStart = attached.Body.position;
        Vector2 boxPrev = boxStart;

        float t = 0f;
        while (t < stepDuration)
        {
            t += Time.fixedDeltaTime;
            float k = Mathf.Clamp01(t / stepDuration);
            k = k * k * (3f - 2f * k); // smoothstep for a soft ease in/out

            Vector2 boxNow = Vector2.Lerp(boxStart, boxTarget, k);
            playerRb.MovePosition(Vector2.Lerp(playerStart, playerTarget, k));
            attached.Body.MovePosition(boxNow);

            // Mirror the incremental motion onto any linked 3D object.
            attached.ReportMoved(boxNow - boxPrev);
            boxPrev = boxNow;

            yield return new WaitForFixedUpdate();
        }

        playerRb.MovePosition(playerTarget);
        attached.Body.MovePosition(boxTarget);
        attached.ReportMoved(boxTarget - boxPrev);
        yield return new WaitForFixedUpdate();

        stepping = false;
    }

    private void Detach()
    {
        attached = null;
        holdTimer = 0f;
        releaseTimer = 0f;
        if (playerMovement != null)
            playerMovement.gridLocked = false;

        if (attachedPrompt != null)
            attachedPrompt.SetActive(false);
    }

    // --- Helpers --------------------------------------------------------------

    private bool CellClear(Vector2 center, Collider2D sizeSource)
    {
        Vector2 size = sizeSource.bounds.size * 0.9f;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, obstacleMask);

        foreach (Collider2D h in hits)
        {
            if (h == null || h.isTrigger) continue;
            if (h == playerCol || h == attached.Col) continue;
            return false;
        }
        return true;
    }

    private static Vector2 DominantDir(Vector2 v)
    {
        if (v.sqrMagnitude < 0.01f) return Vector2.zero;

        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            return new Vector2(Mathf.Sign(v.x), 0f);
        return new Vector2(0f, Mathf.Sign(v.y));
    }
}
