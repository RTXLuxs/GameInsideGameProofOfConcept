using UnityEngine;

public class OpenChest2D : MonoBehaviour, IInteractable2D
{
    private WorldObject thisObject;

    private void Awake()
    {
        thisObject = GetComponent<WorldObject>();
    }

    public bool CanInteract()
    {
        return this;
    }

    public void Interact()
    {
        if (GameManager.Instance.hasKey)
        {
            thisObject.SetState(WorldObjectState.Open);
            Debug.Log("This");
        }
        else
        {
            Debug.Log("no key");
            return;
        }
    }
}
