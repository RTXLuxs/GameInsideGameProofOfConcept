using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks a 3D furniture object as the counterpart of a 2D minigame object.
/// Both objects share the same <see cref="furnitureId"/>; the 2D <c>Furniture2DLink</c>
/// looks this up at runtime (the two live in different scenes, so they can't be
/// wired directly in the editor).
/// </summary>
public class Furniture3DAnchor : MonoBehaviour
{
    [Tooltip("Must match the Furniture Id on the paired 2D object.")]
    [SerializeField] private string furnitureId;

    private static readonly Dictionary<string, Furniture3DAnchor> registry =
        new Dictionary<string, Furniture3DAnchor>();

    public static Furniture3DAnchor Get(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return registry.TryGetValue(id, out Furniture3DAnchor anchor) ? anchor : null;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(furnitureId))
        {
            Debug.LogWarning($"Furniture3DAnchor on '{name}' has no furnitureId set.", this);
            return;
        }

        if (registry.ContainsKey(furnitureId) && registry[furnitureId] != this)
            Debug.LogWarning($"Duplicate furnitureId '{furnitureId}' on '{name}'.", this);

        registry[furnitureId] = this;
    }

    private void OnDisable()
    {
        if (!string.IsNullOrEmpty(furnitureId) &&
            registry.TryGetValue(furnitureId, out Furniture3DAnchor anchor) && anchor == this)
        {
            registry.Remove(furnitureId);
        }
    }

    /// <summary>Applies a world-space movement delta to this 3D object.</summary>
    public void ApplyDelta(Vector3 worldDelta)
    {
        transform.position += worldDelta;
    }
}
