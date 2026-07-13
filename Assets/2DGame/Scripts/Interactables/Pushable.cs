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
