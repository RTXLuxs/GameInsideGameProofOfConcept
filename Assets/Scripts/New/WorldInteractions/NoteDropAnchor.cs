using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Marks a 3D note object as the counterpart of a 2D minigame note. Both objects share
/// the same <see cref="noteId"/>; the 2D <c>ShelfNote</c> looks this up at runtime and
/// calls <see cref="Drop"/> when the player grabs the note (the two live in different
/// scenes, so they can't be wired directly in the editor).
///
/// Hook your 3D drop animation to the <see cref="onDrop"/> event in the inspector.
/// </summary>
public class NoteDropAnchor : MonoBehaviour
{
    [Tooltip("Must match the Note Id on the paired 2D ShelfNote.")]
    [SerializeField] private string noteId;

    [Tooltip("Invoked when the 2D player grabs the note. Hook your drop animation here " +
             "(e.g. call Animator.SetTrigger).")]
    public UnityEvent onDrop;

    private static readonly Dictionary<string, NoteDropAnchor> registry =
        new Dictionary<string, NoteDropAnchor>();

    public static NoteDropAnchor Get(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return registry.TryGetValue(id, out NoteDropAnchor anchor) ? anchor : null;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(noteId))
        {
            Debug.LogWarning($"NoteDropAnchor on '{name}' has no noteId set.", this);
            return;
        }

        if (registry.ContainsKey(noteId) && registry[noteId] != this)
            Debug.LogWarning($"Duplicate noteId '{noteId}' on '{name}'.", this);

        registry[noteId] = this;
    }

    private void OnDisable()
    {
        if (!string.IsNullOrEmpty(noteId) &&
            registry.TryGetValue(noteId, out NoteDropAnchor anchor) && anchor == this)
        {
            registry.Remove(noteId);
        }
    }

    /// <summary>Called from the 2D side when the note is grabbed; plays the drop animation.</summary>
    public void Drop()
    {
        onDrop?.Invoke();
    }
}
