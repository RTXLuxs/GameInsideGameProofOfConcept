using UnityEngine;

/// <summary>
/// Marks a 2D object as push/pull-able by the player. Attach this to any object that
/// has a Collider2D. A kinematic Rigidbody2D is added automatically so the object is
/// only ever moved deliberately by the PushPullController (never nudged by collisions).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Pushable : MonoBehaviour
{
    public Rigidbody2D Body { get; private set; }
    public Collider2D Col { get; private set; }

    /// <summary>
    /// Raised whenever the object is moved by the player, carrying the world-space
    /// delta of this step/frame. A linked 3D object can subscribe to mirror the motion.
    /// </summary>
    public event System.Action<Vector2> Moved;

    public void ReportMoved(Vector2 worldDelta)
    {
        if (worldDelta != Vector2.zero)
            Moved?.Invoke(worldDelta);
    }

    private void Awake()
    {
        Col = GetComponent<Collider2D>();

        Body = GetComponent<Rigidbody2D>();
        if (Body == null)
            Body = gameObject.AddComponent<Rigidbody2D>();

        // Kinematic so the dynamic player can't shove it around by walking into it;
        // it only moves when the controller calls MovePosition during a step.
        Body.bodyType = RigidbodyType2D.Kinematic;
        Body.interpolation = RigidbodyInterpolation2D.Interpolate;
        Body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
}
