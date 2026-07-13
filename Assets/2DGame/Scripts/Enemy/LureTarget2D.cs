using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks a 2D object as a lure target that a paired 3D interactable can point the enemy
/// toward. Both objects share the same <see cref="lureId"/>. Registered at runtime because
/// the 2D and 3D scenes are loaded together but can't reference each other in the editor.
///
/// Exposes the object's *current* position, so luring still works after the item is pushed
/// around in the 2D world.
/// </summary>
public class LureTarget2D : MonoBehaviour
{
    [Tooltip("Must match the Lure Id on the paired 3D LureSoundInteractable.")]
    [SerializeField] private string lureId;

    private static readonly Dictionary<string, LureTarget2D> registry =
        new Dictionary<string, LureTarget2D>();

    public Vector2 Position2D => transform.position;

    public static LureTarget2D Get(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return registry.TryGetValue(id, out LureTarget2D target) ? target : null;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(lureId))
        {
            Debug.LogWarning($"LureTarget2D on '{name}' has no lureId set.", this);
            return;
        }

        if (registry.ContainsKey(lureId) && registry[lureId] != this)
            Debug.LogWarning($"Duplicate lureId '{lureId}' on '{name}'.", this);

        registry[lureId] = this;
    }

    private void OnDisable()
    {
        if (!string.IsNullOrEmpty(lureId) &&
            registry.TryGetValue(lureId, out LureTarget2D target) && target == this)
        {
            registry.Remove(lureId);
        }
    }
}
