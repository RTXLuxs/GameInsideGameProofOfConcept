using UnityEngine;
using Pathfinding;

public class EnemyAnimator : MonoBehaviour
{
    
    public AIPath aiPath;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null) return;

        bool isMoving = aiPath.desiredVelocity.magnitude > 0.01f;
        animator.SetBool("isWalking", isMoving);

        if (isMoving)
        {
            animator.SetFloat("InputX", aiPath.desiredVelocity.x);
            animator.SetFloat("InputY", aiPath.desiredVelocity.y);
            animator.SetFloat("LastInputX", aiPath.desiredVelocity.x);
            animator.SetFloat("LastInputY", aiPath.desiredVelocity.y);
        }
    }
}
