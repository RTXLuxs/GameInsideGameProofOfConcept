using System;
using UnityEngine;

/// <summary>
/// Central hub for "distraction" sounds that lure the 2D enemy. Any source — a thrown
/// item landing in the 2D world, or a 3D appliance the player switches on — emits a 2D
/// world position here, and the enemy investigates it (subject to its own hearing range).
/// </summary>
public static class DistractionSound
{
    /// <summary>Raised with the 2D world position the enemy should investigate.</summary>
    public static event Action<Vector2> Emitted;

    public static void Emit(Vector2 position2D)
    {
        Emitted?.Invoke(position2D);
    }
}
