using UnityEngine;

public class Interactable3D : MonoBehaviour
{
    private WorldObject worldObject;

    private void Start()
    {
        worldObject = GetComponent<WorldObject>();
    }

    public void Interact()
    {
        worldObject.SetState(WorldObjectState.Open);
    }
}
