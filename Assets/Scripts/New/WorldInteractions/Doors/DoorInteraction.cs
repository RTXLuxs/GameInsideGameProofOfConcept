using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    private WorldObject thisObject;
    private DoorState doorState;
    private InteractionRequirements requirements;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
        doorState = GetComponent<DoorState>();
        requirements = GetComponent<InteractionRequirements>();
    }

    public string GetInteractionText()
    {
        var state = thisObject.GetState();

        return state == WorldObjectState.Open
            ? "Close DOOR [E]"
            : "Open DOOR [E]";
    }

    public void Interact()
    {
        var currentState = thisObject.GetState();

        // Door is currently open -> Always allow closing
        if (currentState == WorldObjectState.Open)
        {
            thisObject.SetState(WorldObjectState.Closed);
            doorState.CloseDoor();

            Debug.Log($"[{thisObject.objectId}] CLOSED");
            return;
        }

        // Door is currently closed -> Check requirements before opening
        if (requirements != null &&
            !requirements.AreSatisfied(out string failureReason))
        {
            Debug.Log(failureReason);
            return;
        }

        thisObject.SetState(WorldObjectState.Open);
        doorState.OpenDoor();

        Debug.Log($"[{thisObject.objectId}] OPEN");
    }
}
