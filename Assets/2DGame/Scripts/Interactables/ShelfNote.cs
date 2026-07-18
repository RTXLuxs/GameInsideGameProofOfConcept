using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// A note resting on top of a shelf that the player can only grab after climbing a box
/// pushed next to it. The note's trigger only reaches the top of the box once that box is
/// adjacent to the shelf, so "box next to the note" is enforced by proximity.
///
/// When grabbed the note falls to the floor in 2D (and stays there) and calls the paired
/// 3D <see cref="NoteDropAnchor"/> so the 3D drop animation plays.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ShelfNote : MonoBehaviour
{
    [Header("Grab")]
    [Tooltip("Key that grabs the note when eligible.")]
    [SerializeField] private Key grabKey = Key.E;
    [Tooltip("If true, the note can only be grabbed while the player is climbing (on the box).")]
    [SerializeField] private bool requireClimbing = true;

    [Header("Drop (2D)")]
    [Tooltip("Where the note lands. If empty, it falls straight down by Drop Distance.")]
    [SerializeField] private Transform dropTarget;
    [Tooltip("Fallback fall distance (world units, downward) when no Drop Target is set.")]
    [SerializeField] private float dropDistance = 1f;
    [Tooltip("Seconds for the note to fall. 0 = snap instantly.")]
    [SerializeField] private float dropDuration = 0.4f;
    [Tooltip("Sorting order applied to the note once on the floor, so it sits under furniture again.")]
    [SerializeField] private int groundedSortingOrder = 0;

    [Header("3D link")]
    [Tooltip("Matches the Note Id on the paired 3D NoteDropAnchor. Leave empty for no 3D link.")]
    [SerializeField] private string noteId;

    [Header("Prompt / events")]
    [Tooltip("Optional prompt (e.g. 'E to grab') shown while the note can be grabbed.")]
    [SerializeField] private GameObject grabPrompt;
    [Tooltip("Invoked in the 2D scene the moment the note is grabbed (sound, VFX, etc.).")]
    public UnityEvent onGrabbed;

    private Collider2D col;
    private ClimbController climber;   // the player's climb controller while in range
    private bool dropped;

    private void Reset()
    {
        // Detection needs a trigger; default it when the component is first added.
        Collider2D c = GetComponent<Collider2D>();
        if (c != null) c.isTrigger = true;
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"ShelfNote on '{name}': collider should be Is Trigger for grab detection.", this);
        }

        if (grabPrompt != null)
            grabPrompt.SetActive(false);
    }

    private void Update()
    {
        if (dropped) return;

        bool eligible = climber != null && (!requireClimbing || climber.IsClimbing);

        if (grabPrompt != null && grabPrompt.activeSelf != eligible)
            grabPrompt.SetActive(eligible);

        if (!eligible) return;

        Keyboard kb = Keyboard.current;
        if (kb != null && kb[grabKey].wasPressedThisFrame)
            Grab();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ClimbController c = other.GetComponentInParent<ClimbController>();
        if (c != null)
            climber = c;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ClimbController c = other.GetComponentInParent<ClimbController>();
        if (c != null && c == climber)
        {
            climber = null;
            if (grabPrompt != null)
                grabPrompt.SetActive(false);
        }
    }

    private void Grab()
    {
        dropped = true;
        if (grabPrompt != null)
            grabPrompt.SetActive(false);

        onGrabbed?.Invoke();

        // Tell the 3D counterpart to play its drop animation.
        if (!string.IsNullOrEmpty(noteId))
            NoteDropAnchor.Get(noteId)?.Drop();

        // 2D: fall to the floor and rest there, drawn under furniture again.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = groundedSortingOrder;

        Vector3 target = dropTarget != null
            ? dropTarget.position
            : transform.position + Vector3.down * dropDistance;

        if (dropDuration <= 0f)
            transform.position = target;
        else
            StartCoroutine(FallTo(target));
    }

    private IEnumerator FallTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < dropDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / dropDuration);
            k = k * k;   // ease-in, so it accelerates like a fall
            transform.position = Vector3.Lerp(start, target, k);
            yield return null;
        }
        transform.position = target;
    }
}
