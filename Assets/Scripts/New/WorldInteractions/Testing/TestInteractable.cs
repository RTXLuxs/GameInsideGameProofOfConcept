using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{

    private WorldObject thisObject;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
    }

    // Called when player interacts with this object
    public void Interact()
    {
        var currentState = thisObject.GetState();

        if (currentState == WorldObjectState.Open)
        {
            thisObject.SetState(WorldObjectState.Closed);
            Debug.Log($"[{thisObject.objectId}] CLOSED");
        }
        else
        {
            thisObject.SetState(WorldObjectState.Open);
            Debug.Log($"[{thisObject.objectId}] OPEN");
        }
    }

    // Text shown when player looks at the object
    public string GetInteractionText()
    {
        var state = thisObject.GetState();

        return state == WorldObjectState.Open
            ? "Close Object [E]"
            : "Open Object [E]";
    }
}
