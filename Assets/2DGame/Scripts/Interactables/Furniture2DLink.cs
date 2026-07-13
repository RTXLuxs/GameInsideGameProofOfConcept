using UnityEngine;

/// <summary>
/// Links a 2D minigame furniture object (a <see cref="Pushable"/>) to its 3D
/// counterpart. When the player moves the 2D object, the matching 3D object
/// (found by <see cref="furnitureId"/>) moves by the equivalent world delta.
///
/// The sync is relative (delta based), so the two objects just need to *start*
/// at corresponding positions — origins don't have to be perfectly aligned.
/// </summary>
[RequireComponent(typeof(Pushable))]
public class Furniture2DLink : MonoBehaviour
{
    [Tooltip("Must match the Furniture Id on the paired 3D Furniture3DAnchor.")]
    [SerializeField] private string furnitureId;

    [Header("2D -> 3D mapping")]
    [Tooltip("3D world units moved per 1 unit of 2D movement. 2D cells are 0.5, so scale 2 = one 3D unit per cell.")]
    [SerializeField] private float scale = 2f;
    [Tooltip("3D direction that a 2D +X (right) movement maps to.")]
    [SerializeField] private Vector3 map2DxTo3D = new Vector3(1f, 0f, 0f);
    [Tooltip("3D direction that a 2D +Y (up) movement maps to.")]
    [SerializeField] private Vector3 map2DyTo3D = new Vector3(0f, 0f, 1f);

    private Pushable pushable;

    private void Awake()
    {
        pushable = GetComponent<Pushable>();
    }

    private void OnEnable()
    {
        pushable.Moved += OnMoved;
    }

    private void OnDisable()
    {
        pushable.Moved -= OnMoved;
    }

    private void OnMoved(Vector2 delta2D)
    {
        Furniture3DAnchor anchor = Furniture3DAnchor.Get(furnitureId);
        if (anchor == null) return;

        Vector3 delta3D =
            (map2DxTo3D.normalized * delta2D.x + map2DyTo3D.normalized * delta2D.y) * scale;

        anchor.ApplyDelta(delta3D);
    }
}
