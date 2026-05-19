using Pathfinding;
using UnityEngine;


//This script should be merged with DoorState.cs later
public class DoorState2D : MonoBehaviour
{
    private WorldObject thisObject;

    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();

        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        WorldState.Instance.OnStateChanged += OnWorldStateChanged;
    }

    private void OnDisable()
    {
        WorldState.Instance.OnStateChanged -= OnWorldStateChanged;
    }

    private void Start()
    {
        ApplyState();
    }

    private void OnWorldStateChanged(string objectId,WorldObjectState state)
    {
        // Ignore unrelated objects
        if (objectId != thisObject.objectId)
            return;

        ApplyState();
    }

    private void ApplyState()
    {
        bool isOpen =
            thisObject.GetState()
            == WorldObjectState.Open;

        // Get bounds BEFORE disabling collider
        Bounds bounds = col.bounds;

        col.enabled = !isOpen;
        sr.enabled = !isOpen;

        // Update A* graph
        UpdateAstarGrid.Instance.GridUpdate(bounds);
    }
}
