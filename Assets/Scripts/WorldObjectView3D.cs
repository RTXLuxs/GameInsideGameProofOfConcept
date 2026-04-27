using UnityEngine;

public class WorldObjectView3D : MonoBehaviour
{
    private WorldObject worldObject;
    public GameObject targetToDisable; // lid, door mesh, etc

    private void Start()
    {
        worldObject = GetComponent<WorldObject>();
    }

    private void Update()
    {
        if (worldObject.GetState() == WorldObjectState.Open)
        {
            if (targetToDisable != null)
                Destroy(targetToDisable);
        }
    }
}
