using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
[UniqueComponent(tag = "ai.destination")]
public class EnemyController : MonoBehaviour
{
    private enum State { Patrol, Chase, Investigate }

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float investigateSpeed = 3f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float loseRadius = 7f;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointDelay = 0f;

    [Header("Hearing")]
    [SerializeField] private float hearingRadius = 8f;
    [SerializeField] private float investigateTimeout = 5f;

    private AIPath aiPath;
    private IAstarAI ai;
    private Transform player;
    private State state = State.Patrol;
    private int waypointIndex;
    private Vector3 distractionPoint;
    private float investigateTimer;
    private float waypointWaitTimer;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        ai = aiPath;
    }

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        GoToCurrentWaypoint();
    }

    void OnEnable()  => ThrownItem.Landed += OnDistractionSound;
    void OnDisable() => ThrownItem.Landed -= OnDistractionSound;

    void Update()
    {
        UpdateStateTransitions();

        switch (state)
        {
            case State.Patrol:      UpdatePatrol();      break;
            case State.Chase:       UpdateChase();       break;
            case State.Investigate: UpdateInvestigate(); break;
        }
    }

    private void UpdateStateTransitions()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRadius && state != State.Chase)
        {
            EnterChase();
            return;
        }

        if (state == State.Chase && dist > loseRadius)
            EnterPatrol();
    }

    private void EnterChase()
    {
        state = State.Chase;
        aiPath.maxSpeed = chaseSpeed;
        ai.isStopped = false;
        if (player != null)
        {
            ai.destination = player.position;
            ai.SearchPath();
        }
    }

    private void EnterPatrol()
    {
        state = State.Patrol;
        waypointWaitTimer = 0f;
        GoToCurrentWaypoint();
    }

    private void UpdatePatrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (waypointWaitTimer > 0f)
        {
            waypointWaitTimer -= Time.deltaTime;
            if (waypointWaitTimer <= 0f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                GoToCurrentWaypoint();
            }
            return;
        }

        if (ai.reachedEndOfPath && !ai.pathPending)
        {
            if (waypointDelay > 0f)
                waypointWaitTimer = waypointDelay;
            else
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                GoToCurrentWaypoint();
            }
        }
    }

    private void GoToCurrentWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        aiPath.maxSpeed = patrolSpeed;
        ai.isStopped = false;
        ai.destination = waypoints[waypointIndex].position;
        ai.SearchPath();
    }

    private void UpdateChase()
    {
        if (player == null) return;
        // AIPath's repathRate handles recalculation; just keep destination current
        ai.destination = player.position;
    }

    private void UpdateInvestigate()
    {
        investigateTimer += Time.deltaTime;
        if (investigateTimer >= investigateTimeout)
        {
            EnterPatrol();
            return;
        }

        if (ai.reachedEndOfPath && !ai.pathPending)
            ai.isStopped = true;
    }

    private void OnDistractionSound(Vector2 soundPosition)
    {
        if (state == State.Chase) return;
        if (Vector2.Distance((Vector2)transform.position, soundPosition) > hearingRadius) return;

        distractionPoint = new Vector3(soundPosition.x, soundPosition.y, 0f);
        investigateTimer = 0f;
        state = State.Investigate;
        aiPath.maxSpeed = investigateSpeed;
        ai.isStopped = false;
        ai.destination = distractionPoint;
        ai.SearchPath();
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

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

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
