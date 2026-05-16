using UnityEngine;

public class WorldMapper : MonoBehaviour
{
    public static WorldMapper Instance;

    [Header("Vertical offset between 2D/3D")]
    [SerializeField] private float worldOffsetY;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector3 ConvertTo3D(Vector2 position2D)
    {
        return new Vector3(position2D.x, 0, position2D.y);
    }
}
