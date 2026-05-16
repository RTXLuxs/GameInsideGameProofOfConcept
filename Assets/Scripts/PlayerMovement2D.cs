using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    public bool canMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            movement.x = 0;
            movement.y = 0;
            return;
        }
        
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Normalize so diagonal isn't faster
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * moveSpeed;
    }

    public void EnableControls()
    {
        canMove = true;
    }

    //Used to enable 3D controls
    public void DisableControls()
    {
        canMove = false;
    }
}
