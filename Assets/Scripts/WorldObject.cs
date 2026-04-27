using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public string objectId;

    public WorldObjectState GetState()
    {
        return WorldState.Instance.GetState(objectId);
    }

    public void SetState(WorldObjectState state)
    {
        WorldState.Instance.SetState(objectId, state);
    }
}
