using Pathfinding;
using UnityEngine;

public class UpdateAstarGrid : MonoBehaviour
{
    public static UpdateAstarGrid Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GridUpdate(Bounds bounds)
    {
        GraphUpdateObject guo =
            new GraphUpdateObject(bounds);

        AstarPath.active.UpdateGraphs(guo);
    }
}
