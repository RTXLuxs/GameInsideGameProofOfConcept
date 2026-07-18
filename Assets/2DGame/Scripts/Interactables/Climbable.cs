using UnityEngine;

/// <summary>
/// Marks a 2D object as something the player can climb onto (a table, shelf, etc.).
/// Attach to any object with a Collider2D. The <see cref="ClimbController"/> on the
/// player looks for these nearby when the climb key is pressed.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Climbable : MonoBehaviour
{
    public Collider2D Col { get; private set; }

    private void Awake()
    {
        Col = GetComponent<Collider2D>();
    }
}
