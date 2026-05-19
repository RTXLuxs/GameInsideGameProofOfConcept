using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    private WorldObject thisObject;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
    }

    public string GetInteractionText()
    {
        var state = thisObject.GetState();

        return state == WorldObjectState.Open? "Close Object [E]": "Open Object [E]";
    }

    public void Interact()
    {
        var currentState = thisObject.GetState();

        if (currentState == WorldObjectState.Open) //Door is currently open
        {
            thisObject.SetState(WorldObjectState.Closed);
            thisObject.GetComponent<DoorState>().CloseDoor();
            Debug.Log($"[{thisObject.objectId}] CLOSED");
        }
        else //Door is currently closed
        {
            thisObject.SetState(WorldObjectState.Open);
            thisObject.GetComponent<DoorState>().OpenDoor();
            Debug.Log($"[{thisObject.objectId}] OPEN");
        }
    }
}
