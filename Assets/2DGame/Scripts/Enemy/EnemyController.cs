using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum State { Patrol, Chase, Investigate }

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float waypointStopDistance = 0.2f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float loseRadius = 7f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Hearing")]
    [SerializeField] private float hearingRadius = 8f;
    [SerializeField] private float investigateSpeed = 3f;
    [SerializeField] private float investigateTimeout = 5f;

    [Header("Obstacle Avoidance")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleDetectionRange = 1f;

    [Header("Stuck Detection")]
    [SerializeField] private float stuckCheckInterval = 1f;
    [SerializeField] private float stuckDistanceThreshold = 0.1f;

    private static readonly float[] SteerAngles = { 0f, 30f, -30f, 60f, -60f, 90f, -90f };

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private State state = State.Patrol;
    private int waypointIndex;
    private float stuckTimer;
    private Vector2 lastCheckedPosition;
    private Vector2 distractionPoint;
    private float investigateTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (waypoints != null && waypoints.Length > 0)
            lastCheckedPosition = transform.position;
    }

    void OnEnable()  => ThrownItem.Landed += OnDistractionSound;
    void OnDisable() => ThrownItem.Landed -= OnDistractionSound;

    void Update()
    {
        UpdateState();
        if (state == State.Patrol)
            CheckIfStuck();
        if (state == State.Investigate)
            investigateTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol:     
                Patrol(); break;
            case State.Chase:      
                Chase(); break;
            case State.Investigate: 
                Investigate(); break;
        }
    }

    private void UpdateState()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Chase always takes priority
        if (dist <= detectionRadius)
        {
            if (state != State.Chase)
            {
                state = State.Chase;
                ResetStuckTimer();
            }
            return;
        }

        if (state == State.Chase && dist > loseRadius)
        {
            state = State.Patrol;
            ResetStuckTimer();
        }
    }

    private void OnDistractionSound(Vector2 soundPosition)
    {
        if (state == State.Chase) return;
        if (Vector2.Distance(transform.position, soundPosition) > hearingRadius) return;

        distractionPoint = soundPosition;
        investigateTimer = 0f;
        state = State.Investigate;
    }

    private void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            SetVelocity(Vector2.zero);
            return;
        }

        Transform target = waypoints[waypointIndex];
        Vector2 desired = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 steered = Steer(desired);

        SetVelocity(steered * patrolSpeed);

        if (Vector2.Distance(transform.position, target.position) <= waypointStopDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            ResetStuckTimer();
        }
    }

    private void Chase()
    {
        if (player == null) return;

        Vector2 desired = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 steered = Steer(desired);

        SetVelocity(steered * chaseSpeed);
    }

    private void Investigate()
    {
        if (investigateTimer >= investigateTimeout)
        {
            state = State.Patrol;
            ResetStuckTimer();
            return;
        }

        if (Vector2.Distance(transform.position, distractionPoint) <= waypointStopDistance)
        {
            SetVelocity(Vector2.zero);
            return;
        }

        Vector2 desired = (distractionPoint - (Vector2)transform.position).normalized;
        SetVelocity(Steer(desired) * investigateSpeed);
    }

    // Tries the desired direction first, then progressively wider angles until a clear path is found.
    private Vector2 Steer(Vector2 desired)
    {
        foreach (float angle in SteerAngles)
        {
            Vector2 candidate = Rotate(desired, angle);
            if (!Physics2D.Raycast(transform.position, candidate, obstacleDetectionRange, obstacleLayer))
                return candidate;
        }
        return desired;
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }

    private void CheckIfStuck()
    {
        stuckTimer += Time.deltaTime;
        if (stuckTimer < stuckCheckInterval) return;

        if (Vector2.Distance(transform.position, lastCheckedPosition) < stuckDistanceThreshold)
            waypointIndex = (waypointIndex + 1) % waypoints.Length;

        ResetStuckTimer();
    }

    private void ResetStuckTimer()
    {
        stuckTimer = 0f;
        lastCheckedPosition = transform.position;
    }

    private void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
        UpdateAnimator(velocity);
    }

    private void UpdateAnimator(Vector2 velocity)
    {
        if (animator == null) return;

        bool isMoving = velocity.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isMoving);

        if (isMoving)
        {
            animator.SetFloat("InputX", velocity.x);
            animator.SetFloat("InputY", velocity.y);
            animator.SetFloat("LastInputX", velocity.x);
            animator.SetFloat("LastInputY", velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.transform.position = Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRadius);

        if (waypoints == null) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawSphere(waypoints[i].position, 0.15f);
            Gizmos.DrawLine(waypoints[i].position, waypoints[(i + 1) % waypoints.Length].position);
        }
    }
}
