using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform spawnPoint;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private bool canMove = false;

    // When true, PushPullController drives the Rigidbody directly (grid-cell push/pull),
    // so PlayerMovement stops applying its own velocity. Input is still read for direction/animation.
    [HideInInspector] public bool gridLocked = false;

    public Vector2 MoveInput => moveInput;

    private AudioSource footstepAudioSource;
    private AudioSource gameOverSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        footstepAudioSource = GameObject.Find("FootstepProxy").GetComponent<AudioSource>();
        gameOverSource = GameObject.Find("GameOverSound").GetComponent<AudioSource>();

        footstepAudioSource.enabled = false;
    }

    void FixedUpdate()
    {
        // While attached to a pushable object, the PushPullController moves the body.
        if (gridLocked) return;

        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            // If the movement input is performed, start the walking animation
            animator.SetBool("isWalking", true);

            // If the movement input is canceled, stop the walking animation and set the last input direction
            if (context.canceled)
            {
                animator.SetBool("isWalking", false);
                animator.SetFloat("LastInputX", moveInput.x);
                animator.SetFloat("LastInputY", moveInput.y);
            }
            // Read the movement input
            moveInput = context.ReadValue<Vector2>();
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            if (moveInput.x != 0 || moveInput.y != 0)
            {
                footstepAudioSource.enabled = true;
            }
            else
            {
                footstepAudioSource.enabled = false;
            }
        }
        else
        {
            footstepAudioSource.enabled = false;
        }
    }

    public void Respawn()
    {
        gameOverSource.Play();
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        transform.position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
    }

    public void EnableControls()
    {
        canMove = true;
    }

    //Used to enable 3D controls
    public void DisableControls()
    {
        canMove = false;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        footstepAudioSource.enabled = false;
        animator.SetBool("isWalking", false);
        animator.SetFloat("InputX", 0f);
        animator.SetFloat("InputY", 0f);
    }
}
